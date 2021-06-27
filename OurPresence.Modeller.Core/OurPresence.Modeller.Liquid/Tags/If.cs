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
    /// If is the conditional block
    ///
    /// {% if user.admin %}
    ///   Admin user!
    /// {% else %}
    ///   Not admin user
    /// {% endif %}
    ///
    ///  There are {% if count &lt; 5 %} less {% else %} more {% endif %} items than you need.
    /// </summary>
    public class If : Modeller.Liquid.Block
    {
        private readonly string _syntaxHelp = Liquid.ResourceManager.GetString("IfTagSyntaxException");
        private readonly string _tooMuchConditionsHelp = Liquid.ResourceManager.GetString("IfTagTooMuchConditionsException");
        private static readonly Regex s_syntax = R.B(R.Q(@"({0})\s*([=!<>a-zA-Z_]+)?\s*({0})?"), Liquid.QuotedFragment);

        private static readonly string s_expressionsAndOperators = string.Format(R.Q(@"(?:\b(?:\s?and\s?|\s?or\s?)\b|(?:\s*(?!\b(?:\s?and\s?|\s?or\s?)\b)(?:{0}|\S+)\s*)+)"), Liquid.QuotedFragment);
        private static readonly Regex s_expressionsAndOperatorsRegex = R.C(s_expressionsAndOperators);

        /// <summary>
        /// 
        /// </summary>
        protected List<Condition> Blocks { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        protected If(Template template, string tagName, string markup)
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public override void Initialize(IEnumerable<string> tokens)
        {
            Blocks = new List<Condition>();
            PushBlock("if", Markup);
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
            // Ruby version did not include "elseif", but I've added that to make it more C#-friendly.
            if (tag == "elsif" || tag == "elseif" || tag == "else")
            {
                PushBlock(tag, markup);
            }
            else
            {
                base.UnknownTag(tag, markup, tokens);
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
                foreach (var block in Blocks)
                {
                    if (block.Evaluate(context.Template, context, result.FormatProvider))
                    {
                        RenderAll(new NodeList(block.Attachment), context, result);
                        return;
                    }
                }
            });
        }

        private void PushBlock(string tag, string markup)
        {
            Condition block;
            if (tag == "else")
            {
                block = new ElseCondition();
            }
            else
            {
                var expressions = R.Scan(markup, s_expressionsAndOperatorsRegex);

                // last item in list
                var syntax = expressions.TryGetAtIndexReverse(0);

                if (string.IsNullOrEmpty(syntax))
                {
                    throw new SyntaxException(_syntaxHelp);
                }

                var syntaxMatch = s_syntax.Match(syntax);
                if (!syntaxMatch.Success)
                {
                    throw new SyntaxException(_syntaxHelp);
                }

                var condition = new Condition(syntaxMatch.Groups[1].Value,
                    syntaxMatch.Groups[2].Value, syntaxMatch.Groups[3].Value);

                var conditionCount = 1;
                // continue to process remaining items in the list backwards, in pairs
                for (var i = 1; i < expressions.Count; i += 2)
                {
                    var @operator = expressions.TryGetAtIndexReverse(i).Trim();

                    var expressionMatch = s_syntax.Match(expressions.TryGetAtIndexReverse(i + 1));
                    if (!expressionMatch.Success)
                    {
                        throw new SyntaxException(_syntaxHelp);
                    }

                    if (++conditionCount > 500)
                    {
                        throw new SyntaxException(_tooMuchConditionsHelp);
                    }

                    var newCondition = new Condition(expressionMatch.Groups[1].Value,
                        expressionMatch.Groups[2].Value, expressionMatch.Groups[3].Value);
                    switch (@operator)
                    {
                        case "and":
                            newCondition.And(condition);
                            break;
                        case "or":
                            newCondition.Or(condition);
                            break;
                    }
                    condition = newCondition;
                }
                block = condition;
            }

            Blocks.Add(block);
        }
    }
}
