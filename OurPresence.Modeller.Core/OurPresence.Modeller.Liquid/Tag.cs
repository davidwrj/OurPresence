// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Represents a tag in Liquid: {% cycle 'one', 'two', 'three' %}
    /// </summary>
    public abstract class Tag : IRenderable
    {
        private readonly Template _template;

        /// <summary>
        /// Only want to allow Tags to be created in inherited classes or tests.
        /// </summary>
        protected Tag(Template template, string tagName, string markup)
        {
            _template = template;
            TagName = tagName;
            Markup = markup;
        }

        /// <summary>
        /// The owning template for this tag
        /// </summary>
        public Template Template => _template;

        /// <summary>
        /// List of the nodes composing the tag
        /// </summary>
        public NodeList NodeList { get; } = new NodeList();

        /// <summary>
        /// Name of the tag
        /// </summary>
        protected string TagName { get; }

        /// <summary>
        /// Content of the tag node except the name.
        /// E.g. for {% tablerow n in numbers cols:3%} {{n}} {% endtablerow %}
        /// It is "n in numbers cols:3"
        /// </summary>
        protected string Markup { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootNodeList"></param>
        internal virtual void AssertTagRulesViolation(NodeList rootNodeList) { }

        /// <summary>
        /// Parses the tag
        /// </summary>
        /// <param name="tokens"></param>
        protected virtual void Parse(IEnumerable<string> tokens) { }

        /// <summary>
        /// Initializes the tag
        /// </summary>
        /// <param name="tokens">Tokens of the parsed tag</param>
        public virtual void Initialize(IEnumerable<string> tokens)
        {
            Parse(tokens);
        }

        /// <summary>
        /// Name of the tag, usually the type name in lowercase
        /// </summary>
        public string Name => GetType().Name.ToLower();

        /// <summary>
        /// Renders the tag
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public abstract void Render(Context context, TextWriter result);

        /// <summary>
        /// Primarily intended for testing.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal string Render(Context context)
        {
            using (TextWriter result = new StringWriter(context.FormatProvider))
            {
                Render(context, result);
                return result.ToString();
            }
        }
    }
}
