// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid
{
    internal static class DictionaryExtensions
    {
        public static V TryAdd<K, V>(this IDictionary<K, V> dic, K key, Func<V> factory)
        {
            if (!dic.TryGetValue(key, out var found))
            {
                return dic[key] = factory();
            }

            return found;
        }
    }

    /// <summary>
    /// Strainer is the parent class for the filters system.
    /// New filters are mixed into the strainer class which is then instanciated for each liquid template render run.
    ///
    /// One of the strainer's responsibilities is to keep malicious method calls out
    /// </summary>
    public class Strainer
    {
        private static readonly Dictionary<string, Type> s_filters = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Tuple<object, MethodInfo>> s_filterFuncs = new Dictionary<string, Tuple<object, MethodInfo>>();
        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public static void GlobalFilter(Type filter)
        {
            s_filters[filter.AssemblyQualifiedName] = filter;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="template"></param>
        /// <param name="rawName"></param>
        /// <param name="target"></param>
        /// <param name="methodInfo"></param>
        public static void GlobalFilter(Template template, string rawName, object target, MethodInfo methodInfo)
        {
            var name = rawName;

            s_filterFuncs[name] = Tuple.Create(target, methodInfo);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Strainer Create(Context context)
        {
            var strainer = new Strainer(context);

            foreach (var keyValue in s_filters)
            {
                strainer.Extend(context.Template,keyValue.Value);
            }

            foreach (var keyValue in s_filterFuncs)
            {
                strainer.AddMethodInfo(context.Template,keyValue.Key, keyValue.Value.Item1, keyValue.Value.Item2);
            }

            return strainer;
        }

        private readonly Context _context;
        private readonly Dictionary<string, IList<Tuple<object, MethodInfo>>> _methods = new Dictionary<string, IList<Tuple<object, MethodInfo>>>();
        /// <summary>
        ///
        /// </summary>
        public IEnumerable<MethodInfo> Methods
        {
            get { return _methods.Values.SelectMany(m => m.Select(x => x.Item2)); }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public Strainer(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// In this C# implementation, we can't use mixins. So we grab all the static
        /// methods from the specified type and use them instead.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="type"></param>
        public void Extend(Template template, Type type)
        {
            // From what I can tell, calls to Extend should replace existing filters. So be it.
            var methods = type.GetRuntimeMethods().Where(m => m.IsPublic && m.IsStatic);
            var methodNames = methods.Select(m => m.Name);

            foreach (var methodName in methodNames)
            {
                _methods.Remove(methodName);
            }

            foreach (var methodInfo in methods)
            {
                AddMethodInfo(template, methodInfo.Name, null, methodInfo);
            } // foreach
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="template"></param>
        /// <param name="rawName"></param>
        /// <param name="func"></param>
        public void AddFunction<TIn, TOut>(Template template, string rawName, Func<TIn, TOut> func)
        {
            AddMethodInfo(template, rawName, func.Target, func.GetMethodInfo());
        }
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TIn2"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="template"></param>
        /// <param name="rawName"></param>
        /// <param name="func"></param>
        public void AddFunction<TIn, TIn2, TOut>(Template template, string rawName, Func<TIn, TIn2, TOut> func)
        {
            AddMethodInfo(template, rawName, func.Target, func.GetMethodInfo());
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="template"></param>
        /// <param name="rawName"></param>
        /// <param name="target"></param>
        /// <param name="method"></param>
        public void AddMethodInfo(Template template, string rawName, object target, MethodInfo method)
        {
            var name = rawName;
            _methods.TryAdd(name, () => new List<Tuple<object, MethodInfo>>()).Add(Tuple.Create(target, method));
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool RespondTo(string method)
        {
            return _methods.ContainsKey(method);
        }

        /// <summary>
        /// Invoke specified method with provided arguments
        /// </summary>
        /// <param name="method">The method token.</param>
        /// <param name="args">The arguments for invoking the method</param>
        /// <returns>The method's return.</returns>
        public object Invoke(string method, List<object> args)
        {
            // First, try to find a method with the same number of arguments minus context which we set automatically further down.
            var methodInfo = _methods[method].FirstOrDefault(m =>
                m.Item2.GetParameters().Count(p => p.ParameterType != typeof(Context)) == args.Count);

            // If we failed to do so, try one with max numbers of arguments, hoping
            // that those not explicitly specified will be taken care of
            // by default values
            if (methodInfo == null)
            {
                methodInfo = _methods[method].OrderByDescending(m => m.Item2.GetParameters().Length).First();
            }

            var parameterInfos = methodInfo.Item2.GetParameters();

            // If first parameter is Context, send in actual context.
            if (parameterInfos.Length > 0 && parameterInfos[0].ParameterType == typeof(Context))
            {
                args.Insert(0, _context);
            }

            // Add in any default parameters - .NET won't do this for us.
            if (parameterInfos.Length > args.Count)
            {
                for (var i = args.Count; i < parameterInfos.Length; ++i)
                {
                    if ((parameterInfos[i].Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.HasDefault)
                    {
                        throw new SyntaxException(Liquid.ResourceManager.GetString("StrainerFilterHasNoValueException"), method, parameterInfos[i].Name);
                    }

                    args.Add(parameterInfos[i].DefaultValue);
                }
            }

            // Attempt conversions where required by type mismatch and possible by value range.
            // These may be narrowing conversions (e.g. Int64 to Int32) when the actual range doesn't cause an overflow.
            for (var argumentIndex = 0; argumentIndex < parameterInfos.Length; argumentIndex++)
            {
                if (args[argumentIndex] is IConvertible convertibleArg)
                {
                    var parameterType = parameterInfos[argumentIndex].ParameterType;
                    if (convertibleArg.GetType() != parameterType
                        && !parameterType
                            .GetTypeInfo()
                            .IsAssignableFrom(
                                convertibleArg
                                    .GetType()
                                    .GetTypeInfo()
                                    )
                        )
                    {
                        args[argumentIndex] = Convert.ChangeType(convertibleArg, parameterType);
                    }
                }
            }

            try
            {
                return methodInfo.Item2.Invoke(methodInfo.Item1, args.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
