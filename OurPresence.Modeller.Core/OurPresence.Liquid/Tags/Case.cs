using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Liquid.Exceptions;
using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags
{
    public class Case : OurPresence.Liquid.Block
    {
        private static readonly Regex s_syntax = R.B(@"({0})", Liquid.QuotedFragment);
        private static readonly Regex s_whenSyntax = R.B(@"({0})(?:(?:\s+or\s+|\s*\,\s*)({0}.*))?", Liquid.QuotedFragment);

        private List<Condition> _blocks;
        private string _left;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            _blocks = new List<Condition>();

            var syntaxMatch = s_syntax.Match(markup);
            if (syntaxMatch.Success)
                _left = syntaxMatch.Groups[1].Value;
            else
                throw new SyntaxException("Syntax Error in 'case' tag - Valid syntax: case [condition]");

            base.Initialize(tagName, markup, tokens);
        }

        public override void UnknownTag(string tag, string markup, List<string> tokens)
        {
            NodeList = new List<object>();
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
                            RenderAll(block.Attachment, context, result);
                            return;
                        }
                    }
                    else if (block.Evaluate(context, result.FormatProvider))
                    {
                        executeElseBlock = false;
                        RenderAll(block.Attachment, context, result);
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
                    throw new SyntaxException("Syntax Error in 'case' tag - Valid when condition: {{% when [condition] [or condition2...] %}}");

                markup = whenSyntaxMatch.Groups[2].Value;
                if (string.IsNullOrEmpty(markup))
                    markup = null;

                var block = new Condition(_left, "==", whenSyntaxMatch.Groups[1].Value);
                block.Attach(NodeList);
                _blocks.Add(block);
            }
        }

        private void RecordElseCondition(string markup)
        {
            if (markup.Trim() != string.Empty)
                throw new SyntaxException("Syntax Error in 'case' tag - Valid else condition: {{% else %}} (no parameters)");

            var block = new ElseCondition();
            block.Attach(NodeList);
            _blocks.Add(block);
        }
    }
}
