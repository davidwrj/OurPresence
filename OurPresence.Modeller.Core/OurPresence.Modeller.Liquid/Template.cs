// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.FileSystems;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Templates are central to liquid.
    /// Interpreting templates is a two step process. First you compile the
    /// source code. During compile time some extensive error checking is performed.
    ///
    /// After you have a compiled template you can then <tt>render</tt> it.
    /// You can use a compiled template over and over again and keep it cached.
    /// <example>
    /// template = Liquid::Template.parse(source)
    /// template.render('user_name' => 'bob')
    /// </example>
    /// </summary>
    public class Template
    {
        private readonly Dictionary<Type, Func<object, object>> _safeTypeTransformers = new();
        private readonly Dictionary<Type, Func<object, object>> _valueTypeTransformers = new();
        private readonly Dictionary<string, (ITagFactory factory, Type type)> _tags = new();
        private readonly Hash _registers;
        private readonly Hash _assigns;
        private readonly Hash _instanceAssigns;
        private readonly List<Exception> _errors = new();

        /// <summary>
        ///
        /// </summary>
        public Template()
            : this(string.Empty, null)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        public Template(string source)
            : this(source, null)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fileSystem"></param>
        public Template(string source, IFileSystem fileSystem)
        {
            _registers = new Hash();
            _assigns = new Hash();
            _instanceAssigns = new Hash();

            if(fileSystem is null)
            {
                fileSystem = new BlankFileSystem();
            }

            FileSystem = fileSystem;

            if(!string.IsNullOrEmpty(source))
            {
                ParseInternal(source);
            }
        }

        /// <summary>
        /// Creates a new <tt>Template</tt> object from liquid source code
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Template Parse(string source)
        {
            Template template = new Template(source, new BlankFileSystem());
            template.ParseInternal(source);
            return template;
        }

        /// <summary>
        /// Creates a new <tt>Template</tt> object from liquid source code
        /// </summary>
        /// <param name="source">template source code</param>
        /// <param name="fileSystem"></param>
        /// <returns>Template instance that has been parsed.</returns>
        public static Template Parse(string source, IFileSystem fileSystem)
        {
            return new Template(source, fileSystem);
        }

        /// <summary>
        /// Filesystem used for template reading
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <summary>
        /// Register a tag
        /// </summary>
        /// <typeparam name="T">Type of the tag</typeparam>
        /// <param name="name">Name of the tag</param>
        public void RegisterTag<T>(string name)
            where T : Tag
        {
            var tagType = typeof(T);
            _tags.Add(name, (new ActivatorTagFactory(tagType, name), tagType));
        }

        /// <summary>
        /// Registers a tag factory.
        /// </summary>
        /// <param name="tagFactory">The ITagFactory to be registered</param>
        public void RegisterTagFactory(ITagFactory tagFactory)
        {
            _tags[tagFactory.TagName] = (tagFactory, null);
        }

        /// <summary>
        /// Get the tag type from it's name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetTagType(string name)
        {
            _tags.TryGetValue(name, out var result);
            return result.type;
        }

        internal Tag CreateTag(string name, string markup)
        {
            return !_tags.TryGetValue(name, out var result) ? null : result.factory.Create(this, markup);
        }

        /// <summary>
        /// Pass a module with filter methods which should be available
        ///  to all liquid views. Good for registering the standard library
        /// </summary>
        /// <param name="filter"></param>
        public static void RegisterFilter(Type filter)
        {
            Strainer.GlobalFilter(filter);
        }

        /// <summary>
        /// Registers a simple type. OurPresence.Modeller.Liquid will wrap the object in a <see cref="DropProxy"/> object.
        /// </summary>
        /// <param name="type">The type to register</param>
        /// <param name="allowedMembers">An array of property and method names that are allowed to be called on the object.</param>
        public void RegisterSafeType(Type type, string[] allowedMembers)
        {
            RegisterSafeType(type, x => new DropProxy(this, x, allowedMembers));
        }

        /// <summary>
        /// Registers a simple type. OurPresence.Modeller.Liquid will wrap the object in a <see cref="DropProxy"/> object.
        /// </summary>
        /// <param name="type">The type to register</param>
        /// <param name="allowedMembers">An array of property and method names that are allowed to be called on the object.</param>
        /// <param name="func">Function that converts the specified type into a Liquid Drop-compatible object (eg, implements ILiquidizable)</param>
        public void RegisterSafeType(Type type, string[] allowedMembers, Func<object, object> func)
        {
            RegisterSafeType(type, x => new DropProxy(this, x, allowedMembers, func));
        }

        /// <summary>
        /// Registers a simple type using the specified transformer.
        /// </summary>
        /// <param name="type">The type to register</param>
        /// <param name="func">Function that converts the specified type into a Liquid Drop-compatible object (eg, implements ILiquidizable)</param>
        public void RegisterSafeType(Type type, Func<object, object> func)
        {
            _safeTypeTransformers[type] = func;
        }

        /// <summary>
        /// Registers a simple value type transformer.  Used for rendering a variable to the output stream
        /// </summary>
        /// <param name="type">The type to register</param>
        /// <param name="func">Function that converts the specified type into a Liquid Drop-compatible object (eg, implements ILiquidizable)</param>
        public void RegisterValueTypeTransformer(Type type, Func<object, object> func)
        {
            _valueTypeTransformers[type] = func;
        }

        /// <summary>
        /// Gets the corresponding value type converter
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<object, object> GetValueTypeTransformer(Type type)
        {
            // Check for concrete types
            if(_valueTypeTransformers.TryGetValue(type, out var transformer))
            {
                return transformer;
            }

            // Check for interfaces
            var interfaces = type.GetTypeInfo().ImplementedInterfaces;
            foreach(var interfaceType in interfaces)
            {
                if(_valueTypeTransformers.TryGetValue(interfaceType, out transformer))
                {
                    return transformer;
                }

                if(interfaceType.GetTypeInfo().IsGenericType && _valueTypeTransformers.TryGetValue(
                    interfaceType.GetGenericTypeDefinition(), out transformer))
                {
                    return transformer;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the corresponding safe type transformer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<object, object> GetSafeTypeTransformer(Type type)
        {
            // Check for concrete types
            if(_safeTypeTransformers.TryGetValue(type, out var transformer))
            {
                return transformer;
            }

            // Check for interfaces
            var interfaces = type.GetTypeInfo().ImplementedInterfaces;
            foreach(var interfaceType in interfaces)
            {
                if(_safeTypeTransformers.TryGetValue(interfaceType, out transformer))
                {
                    return transformer;
                }

                if(interfaceType.GetTypeInfo().IsGenericType && _safeTypeTransformers.TryGetValue(
                    interfaceType.GetGenericTypeDefinition(), out transformer))
                {
                    return transformer;
                }
            }
            return null;
        }

        /// <summary>
        /// Liquid document
        /// </summary>
        public Document Root { get; set; }

        /// <summary>
        /// Hash of user-defined, internally-available variables
        /// </summary>
        public Hash Registers => _registers;

        /// <summary>
        ///
        /// </summary>
        public Hash Assigns => _assigns;

        /// <summary>
        ///
        /// </summary>
        public Hash InstanceAssigns => _instanceAssigns;

        /// <summary>
        /// Exceptions that have been raised during template rendering
        /// </summary>
        public IEnumerable<Exception> Errors => _errors;

        /// <summary>
        /// Parse source code.
        /// Returns self for easy chaining
        /// </summary>
        /// <param name="source">The source code.</param>
        /// <returns>The template.</returns>
        internal Template ParseInternal(string source)
        {
            source = Tags.Literal.FromShortHand(source);
            source = Tags.Comment.FromShortHand(source);

            Root = new Document(this, null, null);
            Root.Initialize(Tokenize(source));
            return this;
        }

        /// <summary>
        /// Renders the template using default parameters and the current culture and returns a string containing the result.
        /// </summary>
        /// <returns>The rendering result as string.</returns>
        public string Render(IFormatProvider formatProvider = null)
        {
            formatProvider ??= CultureInfo.CurrentCulture;
            return Render(new RenderParameters(formatProvider));
        }

        /// <summary>
        /// Renders the template using the specified local variables and returns a string containing the result.
        /// </summary>
        /// <param name="localVariables">Local variables.</param>
        /// <param name="formatProvider">String formatting provider.</param>
        /// <returns>The rendering result as string.</returns>
        public string Render(Hash localVariables, IFormatProvider formatProvider = null)
        {
            using var writer = new StringWriter(formatProvider ?? CultureInfo.CurrentCulture);
            return Render(
                writer: writer,
                parameters: new RenderParameters(writer.FormatProvider)
                {
                    LocalVariables = localVariables
                });
        }


        /// <summary>
        /// Renders the template using the specified parameters and returns a string containing the result.
        /// </summary>
        /// <param name="parameters">Render parameters.</param>
        /// <returns>The rendering result as string.</returns>
        public string Render(RenderParameters parameters)
        {
            using var writer = new StringWriter(parameters.FormatProvider);
            return Render(writer, parameters);
        }

        /// <summary>
        /// Renders the template using the specified parameters and returns a string containing the result.
        /// </summary>
        /// <param name="writer">Render parameters.</param>
        /// <param name="parameters"></param>
        /// <returns>The rendering result as string.</returns>
        public string Render(TextWriter writer, RenderParameters parameters)
        {
            if(writer == null)
            {
                throw new ArgumentNullException(paramName: nameof(writer));
            }

            if(parameters == null)
            {
                throw new ArgumentNullException(paramName: nameof(parameters));
            }

            RenderInternal(writer, parameters);
            return writer.ToString();
        }

        /// <inheritdoc />
        private class StreamWriterWithFormatProvider : StreamWriter
        {
            public StreamWriterWithFormatProvider(Stream stream, IFormatProvider formatProvider) : base(stream) => FormatProvider = formatProvider;

            public override IFormatProvider FormatProvider { get; }
        }

        /// <summary>
        /// Renders the template into the specified Stream.
        /// </summary>
        /// <param name="stream">The stream to render into.</param>
        /// <param name="parameters">The render parameters.</param>
        public void Render(Stream stream, RenderParameters parameters)
        {
            //todo:validate the statement below

            // Can't dispose this new StreamWriter, because it would close the
            // passed-in stream, which isn't up to us.

            StreamWriter streamWriter = new StreamWriterWithFormatProvider(stream, parameters.FormatProvider);
            RenderInternal(streamWriter, parameters);
            streamWriter.Flush();
        }

        /// <summary>
        /// Render takes a hash with local variables.
        ///
        /// if you use the same filters over and over again consider registering them globally
        /// with <tt>Template.register_filter</tt>
        ///
        /// Following options can be passed:
        ///
        /// * <tt>filters</tt> : array with local filters
        /// * <tt>registers</tt> : hash with register variables. Those can be accessed from
        /// filters and tags and might be useful to integrate liquid more with its host application
        /// </summary>
        private void RenderInternal(TextWriter result, RenderParameters parameters)
        {
            if(Root is null)
            {
                return;
            }

            parameters.Evaluate(this, out var context, out var registers, out var filters);

            if(registers != null)
            {
                Registers.Merge(registers);
            }

            if(filters != null)
            {
                context.AddFilters(filters);
            }

            try
            {
                // Render the nodelist.
                Root.Render(context, result);
            }
            finally
            {
                _errors.AddRange(context.Errors);
            }
        }

        /// <summary>
        /// Uses the <tt>Liquid::TemplateParser</tt> regexp to tokenize the passed source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static IEnumerable<string> Tokenize(string source)
        {
            if(string.IsNullOrEmpty(source))
            {
                return new List<string>();
            }

            // Trim leading whitespace.
            source = Regex.Replace(source, string.Format(@"([ \t]+)?({0}|{1})-", Liquid.VariableStart, Liquid.TagStart), "$2", RegexOptions.None);

            // Trim trailing whitespace.
            source = Regex.Replace(source, string.Format(@"-({0}|{1})(\n|\r\n|[ \t]+)?", Liquid.VariableEnd, Liquid.TagEnd), "$1", RegexOptions.None);

            var tokens = Regex.Split(source, Liquid.TemplateParser).ToList();

            // Trim any whitespace elements from the end of the array.
            for(var i = tokens.Count - 1; i > 0; --i)
            {
                if(tokens[i] == string.Empty)
                {
                    tokens.RemoveAt(i);
                }
            }

            // Removes the rogue empty element at the beginning of the array
            if(tokens[0] is not null && tokens[0] == string.Empty)
            {
                tokens.Shift();
            }

            return tokens;
        }
    }
}
