// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Context keeps the variable stack and resolves variables, as well as keywords
    /// </summary>
    public class Context
    {
        private static readonly Regex s_singleQuotedRegex = R.C(R.Q(@"^'(.*)'$"));
        private static readonly Regex s_doubleQuotedRegex = R.C(R.Q(@"^""(.*)""$"));
        private static readonly Regex s_integerRegex = R.C(R.Q(@"^([+-]?\d+)$"));
        private static readonly Regex s_rangeRegex = R.C(R.Q(@"^\((\S+)\.\.(\S+)\)$"));
        private static readonly Regex s_numericRegex = R.C(R.Q(@"^([+-]?\d[\d\.|\,]+)$"));
        private static readonly Regex s_squareBracketedRegex = R.C(R.Q(@"^\[(.*)\]$"));
        private static readonly Regex s_variableParserRegex = R.C(Liquid.VariableParser);

        private readonly ErrorsOutputMode _errorsOutputMode;

        private readonly Condition _condition = new Condition();
        private readonly int _maxIterations;
        private Strainer _strainer;
        private readonly List<Hash> _scopes = new();
        private readonly List<Exception> _errors = new();
        private readonly List<Hash> _environments = new();

        /// <summary>
        /// 
        /// </summary>
        public int MaxIterations => _maxIterations;

        /// <summary>
        /// 
        /// </summary>
        public Condition Condition => _condition;


        /// <summary>
        /// Environments
        /// </summary>
        public IEnumerable<Hash> Environments => _environments;

        /// <summary>
        /// Scopes
        /// </summary>
        public IEnumerable<Hash> Scopes => _scopes;

        /// <summary>
        /// Hash of user-defined, internally-available variables
        /// </summary>
        public Hash Registers { get; private set; }


        /// <summary>
        /// Exceptions that have been raised during rendering
        /// </summary>
        public IEnumerable<Exception> Errors => _errors;

        /// <summary>
        /// Creates a new rendering context
        /// </summary>
        /// <param name="template"></param>
        /// <param name="environments"></param>
        /// <param name="outerScope"></param>
        /// <param name="registers"></param>
        /// <param name="errorsOutputMode"></param>
        /// <param name="maxIterations"></param>
        /// <param name="formatProvider"></param>
        /// <param name="cancellationToken"></param>
        public Context(Template template, IEnumerable<Hash> environments, Hash outerScope, Hash registers, ErrorsOutputMode errorsOutputMode, int maxIterations, IFormatProvider formatProvider, CancellationToken cancellationToken)
        {
            _environments.AddRange(environments);

            if (outerScope is not null)
            {
                _scopes.Add(outerScope);
            }

            Template = template;
            Registers = registers;

            _errorsOutputMode = errorsOutputMode;
            _maxIterations = maxIterations;
            _cancellationToken = cancellationToken;
            FormatProvider = formatProvider;

            SquashInstanceAssignsWithEnvironments();
        }

        /// <summary>
        /// Creates a new rendering context
        /// </summary>
        public Context(Template template, IFormatProvider formatProvider)
            : this(template, new List<Hash>(), new Hash(template), new Hash(template), ErrorsOutputMode.Display, 0, formatProvider, default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Template Template { get; }

        /// <summary>
        /// Strainer for the current context
        /// </summary>
        public Strainer Strainer
        {
            get { return _strainer ??= Strainer.Create(this); }
        }

        /// <summary>
        /// Adds a filter from a function
        /// </summary>
        /// <typeparam name="TIn">Type of the parameter</typeparam>
        /// <typeparam name="TOut">Type of the returned value</typeparam>
        /// <param name="filterName">Filter name</param>
        /// <param name="func">Filter function</param>
        public void AddFilter<TIn, TOut>(string filterName, Func<TIn, TOut> func)
        {
            Strainer.AddFunction(Template, filterName, func);
        }

        /// <summary>
        /// Adds a filter from a function
        /// </summary>
        /// <typeparam name="TIn">Type of the first parameter</typeparam>
        /// <typeparam name="TIn2">Type of the second paramter</typeparam>
        /// <typeparam name="TOut">Type of the returned value</typeparam>
        /// <param name="filterName">Filter name</param>
        /// <param name="func">Filter function</param>
        public void AddFilter<TIn, TIn2, TOut>(string filterName, Func<TIn, TIn2, TOut> func)
        {
            Strainer.AddFunction(Template, filterName, func);
        }

        /// <summary>
        /// Adds filters to this context.
        /// this does not register the filters with the main Template object. see <tt>Template.register_filter</tt>
        /// for that
        /// </summary>
        /// <param name="filters"></param>
        public void AddFilters(IEnumerable<Type> filters)
        {
            foreach (var f in filters)
            {
                Strainer.Extend(Template, f);
            }
        }

        /// <summary>
        /// Add filters from a list of types
        /// </summary>
        /// <param name="filters"></param>
        public void AddFilters(params Type[] filters)
        {
            if (filters != null)
            {
                AddFilters(filters.AsEnumerable());
            }
        }

        /// <summary>
        /// Handles error during rendering
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string HandleError(Exception ex)
        {
            if (ex is InterruptException || ex is TimeoutException || ex is RenderException || ex is OperationCanceledException)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }

            _errors.Add(ex);

            if (_errorsOutputMode == ErrorsOutputMode.Suppress)
            {
                return string.Empty;
            }

            if (_errorsOutputMode == ErrorsOutputMode.Rethrow)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }

            return ex is SyntaxException
                ? string.Format(Liquid.ResourceManager.GetString("ContextLiquidSyntaxError"), ex.Message)
                : string.Format(Liquid.ResourceManager.GetString("ContextLiquidError"), ex.Message);
        }

        /// <summary>
        /// Invokes a strainer method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(string method, List<object> args)
        {
            return Strainer.RespondTo(method) ? Strainer.Invoke(method, args) : args.First();
        }

        /// <summary>
        /// Push new local scope on the stack. use <tt>Context#stack</tt> instead
        /// </summary>
        /// <param name="newScope"></param>
        public void Push(Hash newScope)
        {
            if (_scopes.Count > 80)
            {
                throw new StackLevelException(Liquid.ResourceManager.GetString("ContextStackException"));
            }

            _scopes.Insert(0, newScope);
        }

        /// <summary>
        /// Merge a hash of variables in the current local scope
        /// </summary>
        /// <param name="newScopes"></param>
        public void Merge(Hash newScopes)
        {
            _scopes[0].Merge(newScopes);
        }

        /// <summary>
        /// Pop from the stack. use <tt>Context#stack</tt> instead
        /// </summary>
        public Hash Pop()
        {
            if (_scopes.Count == 1)
            {
                throw new ContextException();
            }

            var result = _scopes[0];
            _scopes.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Pushes a new local scope on the stack, pops it at the end of the block
        ///
        /// Example:
        ///
        /// context.stack do
        /// context['var'] = 'hi'
        /// end
        /// context['var] #=> nil
        /// </summary>
        /// <param name="newScope"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void Stack(Hash newScope, Action callback)
        {
            Push(newScope);
            try
            {
                callback();
            }
            finally
            {
                Pop();
            }
        }

        /// <summary>
        /// Pushes a new hash on the stack, pops it at the end of the block
        /// </summary>
        /// <param name="callback"></param>
        public void Stack(Action callback)
        {
            Stack(new Hash(Template), callback);
        }

        /// <summary>
        /// Clear the current instance assigns
        /// </summary>
        public void ClearInstanceAssigns()
        {
            _scopes[0].Clear();
        }

        /// <summary>
        /// Only allow String, Numeric, Hash, Array, Proc, Boolean or <tt>Liquid::Drop</tt>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="notifyNotFound">True to notify if variable is not found; Default true.</param>
        /// <returns></returns>
        public object this[string key, bool notifyNotFound = true]
        {
            get { return Resolve(key, notifyNotFound); }
            set { _scopes[0][key] = value; }
        }

        /// <summary>
        /// Checks if a variable key exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            return Resolve(key, false) != null;
        }

        /// <summary>
        /// Look up variable, either resolve directly after considering the name. We can directly handle
        /// Strings, digits, floats and booleans (true,false). If no match is made we lookup the variable in the current scope and
        /// later move up to the parent blocks to see if we can resolve the variable somewhere up the tree.
        /// Some special keywords return symbols. Those symbols are to be called on the rhs object in expressions
        ///
        /// Example:
        ///
        /// products == empty #=> products.empty?
        /// </summary>
        /// <param name="key"></param>
        /// <param name="notifyNotFound">True to notify if variable is not found; Default true.</param>
        /// <returns></returns>
        private object Resolve(string key, bool notifyNotFound = true)
        {
            switch (key)
            {
                case null:
                case "nil":
                case "null":
                case "":
                    return null;
                case "true":
                    return true;
                case "false":
                    return false;
                case "blank":
                case "empty":
                    return new Symbol(o => o is IEnumerable value && !value.Cast<object>().Any());
            }

            // Single quoted strings.
            var match = s_singleQuotedRegex.Match(key);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // Double quoted strings.
            match = s_doubleQuotedRegex.Match(key);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // Integer.
            match = s_integerRegex.Match(key);
            if (match.Success)
            {
                try
                {
                    return Convert.ToInt32(match.Groups[1].Value);
                }
                catch (OverflowException)
                {
                    return Convert.ToInt64(match.Groups[1].Value);
                }
            }

            // Ranges.
            match = s_rangeRegex.Match(key);
            if (match.Success)
            {
                return Util.Range.Inclusive(Convert.ToInt32(Resolve(match.Groups[1].Value)),
                    Convert.ToInt32(Resolve(match.Groups[2].Value)));
            }

            // Floating point numbers.
            match = s_numericRegex.Match(key);
            if (match.Success)
            {
                // For cultures with "," as the decimal separator, allow
                // both "," and "." to be used as the separator.
                // First try to parse using current culture.
                // If that fails, try to parse using invariant culture.
                // Also, first try higher precision decimal.
                // If that fails, try to parse as double (precision float).
                // Double is less precise but has a larger range.
                return decimal.TryParse(match.Groups[1].Value, NumberStyles.Number | NumberStyles.Float, FormatProvider, out var parsedDecimalCurrentCulture)
                    ? parsedDecimalCurrentCulture
                    : decimal.TryParse(match.Groups[1].Value, NumberStyles.Number | NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedDecimalInvariantCulture)
                    ? parsedDecimalInvariantCulture
                    : double.TryParse(match.Groups[1].Value, NumberStyles.Number | NumberStyles.Float, FormatProvider, out var parsedDouble)
                    ? parsedDouble
                    : (object)double.Parse(match.Groups[1].Value, NumberStyles.Number | NumberStyles.Float, CultureInfo.InvariantCulture);
            }

            return Variable(key, notifyNotFound);
        }

        /// <summary>
        /// 
        /// </summary>
        public IFormatProvider FormatProvider { get; }

        /// <summary>
        /// Fetches an object starting at the local scope and then moving up
        /// the hierarchy
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private object FindVariable(string key)
        {
            var scope = Scopes.FirstOrDefault(s => s.ContainsKey(key));
            object variable = null;
            if (scope == null)
            {
                foreach (var e in Environments)
                {
                    if ((variable = LookupAndEvaluate(e, key)) != null)
                    {
                        scope = e;
                        break;
                    }
                }
            }
            scope ??= Environments.LastOrDefault() ?? Scopes.Last();
            variable ??= LookupAndEvaluate(scope, key);

            variable = Liquidize(variable);
            if (variable is IContextAware contextAwareVariable)
            {
                contextAwareVariable.Context = this;
            }
            return variable;
        }

        /// <summary>
        /// Resolves namespaced queries gracefully.
        ///
        /// Example
        ///
        /// @context['hash'] = {"name" => 'tobi'}
        /// assert_equal 'tobi', @context['hash.name']
        /// assert_equal 'tobi', @context['hash["name"]']
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="notifyNotFound"></param>
        /// <returns></returns>
        private object Variable(string markup, bool notifyNotFound)
        {
            var parts = R.Scan(markup, s_variableParserRegex);

            // first item in list, if any
            var firstPart = parts.TryGetAtIndex(0);

            var firstPartSquareBracketedMatch = s_squareBracketedRegex.Match(firstPart);
            if (firstPartSquareBracketedMatch.Success)
            {
                firstPart = Resolve(firstPartSquareBracketedMatch.Groups[1].Value).ToString();
            }

            object @object;
            if ((@object = FindVariable(firstPart)) == null)
            {
                if (notifyNotFound)
                {
                    _errors.Add(new VariableNotFoundException(string.Format(Liquid.ResourceManager.GetString("VariableNotFoundException"), markup)));
                }

                return null;
            }

            // try to resolve the rest of the parts (starting from the second item in the list)
            for (var i = 1; i < parts.Count; ++i)
            {
                var forEachPart = parts[i];
                var partSquareBracketedMatch = s_squareBracketedRegex.Match(forEachPart);
                var partResolved = partSquareBracketedMatch.Success;

                object part = forEachPart;
                if (partResolved)
                {
                    part = Resolve(partSquareBracketedMatch.Groups[1].Value);
                }

                // If object is a KeyValuePair, we treat it a bit differently - we might be rendering
                // an included template.
                if (IsKeyValuePair(@object) && (part.SafeTypeInsensitiveEqual(0L) || part.Equals("Key")))
                {
                    var res = @object.GetType().GetRuntimeProperty("Key").GetValue(@object);
                    @object = Liquidize(res);
                }
                // If object is a hash- or array-like object we look for the
                // presence of the key and if its available we return it
                else if (IsKeyValuePair(@object) && (part.SafeTypeInsensitiveEqual(1L) || part.Equals("Value")))
                {
                    // If its a proc we will replace the entry with the proc
                    var res = @object.GetType().GetRuntimeProperty("Value").GetValue(@object);
                    @object = Liquidize(res);
                }
                // No key was present with the desired value and it wasn't one of the directly supported
                // keywords either. The only thing we got left is to return nil
                else
                {
                    // If object is a KeyValuePair, we treat it a bit differently - we might be rendering
                    // an included template.
                    if (@object is KeyValuePair<string, object> && ((KeyValuePair<string, object>)@object).Key == (string)part)
                    {
                        var res = ((KeyValuePair<string, object>)@object).Value;
                        @object = Liquidize(res);
                    }
                    // If object is a hash- or array-like object we look for the
                    // presence of the key and if its available we return it
                    else if (IsHashOrArrayLikeObject(@object, part))
                    {
                        // If its a proc we will replace the entry with the proc
                        var res = LookupAndEvaluate(@object, part);
                        @object = Liquidize(res);
                    }
                    // Some special cases. If the part wasn't in square brackets and
                    // no key with the same name was found we interpret following calls
                    // as commands and call them on the current object
                    else if (!partResolved && @object is IEnumerable && (part == "size" || part == "first" || part == "last"))
                    {
                        var castCollection = ((IEnumerable)@object).Cast<object>();
                        if (part == "size")
                        {
                            @object = castCollection.Count();
                        }
                        else if (part =="first")
                        {
                            var res = castCollection.FirstOrDefault();
                            @object = Liquidize(res);
                        }
                        else if (part =="last")
                        {
                            var res = castCollection.LastOrDefault();
                            @object = Liquidize(res);
                        }
                    }
                    // No key was present with the desired value and it wasn't one of the directly supported
                    // keywords either. The only thing we got left is to return nil
                    else
                    {
                        _errors.Add(new VariableNotFoundException(string.Format(Liquid.ResourceManager.GetString("VariableNotFoundException"), markup)));
                        return null;
                    }
                }

                // If we are dealing with a drop here we have to
                if (@object is IContextAware contextAwareObject)
                {
                    contextAwareObject.Context = this;
                }
            }

            return @object;
        }

        private static bool IsHashOrArrayLikeObject(object obj, object part)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IDictionary && ((IDictionary)obj).Contains(part))
            {
                return true;
            }

            if (obj is IList && (part is int || part is long))
            {
                return true;
            }

            if (TypeUtility.IsAnonymousType(obj.GetType()) && obj.GetType().GetRuntimeProperty((string)part) != null)
            {
                return true;
            }

            if (obj is IIndexable && ((IIndexable)obj).ContainsKey(part))
            {
                return true;
            }

            return false;
        }

        private object LookupAndEvaluate(object obj, object key)
        {
            object value;
            if (obj is IDictionary dictionaryObj)
            {
                value = dictionaryObj[key];
            }
            else if (obj is IList listObj)
            {
                value = listObj[Convert.ToInt32(key)];
            }
            else if (TypeUtility.IsAnonymousType(obj.GetType()))
            {
                value = obj.GetType().GetRuntimeProperty((string)key).GetValue(obj, null);
            }
            else
            {
                value = obj is IIndexable indexableObj ? indexableObj[key] : throw new NotSupportedException();
            }

            if (value is Proc procValue)
            {
                var newValue = procValue.Invoke(this);
                if (obj is IDictionary dicObj)
                {
                    dicObj[key] = newValue;
                }
                else if (obj is IList listObj)
                {
                    listObj[Convert.ToInt32(key)] = newValue;
                }
                else if (TypeUtility.IsAnonymousType(obj.GetType()))
                {
                    obj.GetType().GetRuntimeProperty((string)key).SetValue(obj, newValue, null);
                }
                else
                {
                    throw new NotSupportedException();
                }
                return newValue;
            }

            return value;
        }

        private object Liquidize(object obj)
        {
            if (obj == null)
            {
                return obj;
            }
            if (obj is ILiquidizable liquidizableObj)
            {
                return liquidizableObj.ToLiquid();
            }
            if (obj is string)
            {
                return obj;
            }
            if (obj is IEnumerable)
            {
                return obj;
            }
            if (obj.GetType().GetTypeInfo().IsPrimitive)
            {
                return obj;
            }
            if (obj is decimal)
            {
                return obj;
            }
            if (obj is DateTime)
            {
                return obj;
            }
            if (obj is DateTimeOffset)
            {
                return obj;
            }
            if (obj is TimeSpan)
            {
                return obj;
            }
            if (obj is Guid)
            {
                return obj;
            }
            if (TypeUtility.IsAnonymousType(obj.GetType()))
            {
                return obj;
            }

            var safeTypeTransformer = Template.GetSafeTypeTransformer(obj.GetType());
            if (safeTypeTransformer != null)
            {
                return safeTypeTransformer(obj);
            }

            if (obj.GetType().GetTypeInfo().GetCustomAttributes(typeof(LiquidTypeAttribute), false).Any())
            {
                var attr = (LiquidTypeAttribute)obj.GetType().GetTypeInfo().GetCustomAttributes(typeof(LiquidTypeAttribute), false).First();
                return new DropProxy(Template, obj, attr.AllowedMembers);
            }

            return IsKeyValuePair(obj)
                ? obj
                : throw new SyntaxException(Liquid.ResourceManager.GetString("ContextObjectInvalidException"), obj.ToString());
        }

        private static bool IsKeyValuePair(object obj)
        {
            if (obj != null)
            {
                var valueType = obj.GetType();
                if (valueType.GetTypeInfo().IsGenericType)
                {
                    var baseType = valueType.GetGenericTypeDefinition();
                    if (baseType == typeof(KeyValuePair<,>))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SquashInstanceAssignsWithEnvironments()
        {
            var tempAssigns = new Dictionary<string, object>();

            var lastScope = Scopes.Last();
            foreach (var k in lastScope.Keys)
            {
                foreach (var env in Environments)
                {
                    if (env.ContainsKey(k))
                    {
                        tempAssigns[k] = LookupAndEvaluate(env, k);
                        break;
                    }
                }
            }

            foreach (var k in tempAssigns.Keys)
            {
                lastScope[k] = tempAssigns[k];
            }
        }

        private readonly int _timeout;
        private readonly Stopwatch _stopwatch = new();
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        /// <summary>
        /// 
        /// </summary>
        public void RestartTimeout()
        {
            _stopwatch.Restart();
            _cancellationToken.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckTimeout()
        {
            if (_timeout > 0 && _stopwatch.ElapsedMilliseconds > _timeout)
            {
                throw new TimeoutException();
            }

            _cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
