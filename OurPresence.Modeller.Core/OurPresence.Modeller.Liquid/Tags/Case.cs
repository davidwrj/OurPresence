// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// 
    /// </summary>
    public class Case : Modeller.Liquid.Block
    {
        private static readonly Regex s_syntax = R.B(@"({0})", Liquid.QuotedFragment);
        private static readonly Regex s_whenSyntax = R.B(@"({0})(?:(?:\s+or\s+|\s*\,\s*)({0}.*))?", Liquid.QuotedFragment);

        private List<Condition> _blocks;
        private string _left;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Case(Template template, string tagName, string markup) 
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(IEnumerable<string> tokens)
        {
            _blocks = new List<Condition>();

            var syntaxMatch = s_syntax.Match(Markup);
            _left = syntaxMatch.Success
                ? syntaxMatch.Groups[1].Value
                : throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagSyntaxException"));

            base.Initialize(tokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="markup"></param>
        /// <param name="tokens"></param>
        public override void UnknownTag(string tag, string markup, IEnumerable<string> tokens)
        {
            NodeList.Clear();
            switch (tag)
            {
                case "when":
                    RecordWhenCondition(markup);
                    break;
                case "else":
                    RecordElseCondition(markup);
                    break;
                default:
                    base.UnknownTag(tag, markup, tokens);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            context.Stack(() =>
            {
                var executeElseBlock = true;
                _blocks.ForEach(block =>
                {
                    if (block.IsElse)
                    {
                        if (executeElseBlock)
                        {
                            RenderAll(new NodeList(block.Attachment), context, result);
                            return;
                        }
                    }
                    else if (block.Evaluate(Template, context, result.FormatProvider))
                    {
                        executeElseBlock = false;
                        RenderAll(new NodeList(block.Attachment), context, result);
                    }
                });
            });
        }

        private void RecordWhenCondition(string markup)
        {
            while (markup != null)
            {
                // Create a new nodelist and assign it to the new block
                var whenSyntaxMatch = s_whenSyntax.Match(markup);
                if (!whenSyntaxMatch.Success)
                {
                    throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagWhenSyntaxException"));
                }

                markup = whenSyntaxMatch.Groups[2].Value;
                if (string.IsNullOrEmpty(markup))
                {
                    markup = null;
                }

                var block = new Condition(_left, "==", whenSyntaxMatch.Groups[1].Value);
                block.Attach(NodeList);
                _blocks.Add(block);
            }
        }

        private void RecordElseCondition(string markup)
        {
            if (markup.Trim() != string.Empty)
            {
                throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagElseSyntaxException"));
            }

            var block = new ElseCondition();
            block.Attach(NodeList);
            _blocks.Add(block);
        }
    }
}
