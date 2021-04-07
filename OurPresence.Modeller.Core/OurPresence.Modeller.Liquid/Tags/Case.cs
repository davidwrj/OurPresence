using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class Case : Modeller.Liquid.Block
    {
        private static readonly Regex Syntax = R.B(@"({0})", Liquid.QuotedFragment);
        private static readonly Regex WhenSyntax = R.B(@"({0})(?:(?:\s+or\s+|\s*\,\s*)({0}.*))?", Liquid.QuotedFragment);

        private List<Condition> _blocks;
        private string _left;
        public Case(Template template, string tagName, string markup) : base(template, tagName, markup)
        { }

        public override void Initialize(IEnumerable<string> tokens)
        {
            _blocks = new List<Condition>();

            Match syntaxMatch = Syntax.Match(Markup);
            if (syntaxMatch.Success)
                _left = syntaxMatch.Groups[1].Value;
            else
                throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagSyntaxException"));

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
                bool executeElseBlock = true;
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
                    else if (block.Evaluate(context, result.FormatProvider))
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
                Match whenSyntaxMatch = WhenSyntax.Match(markup);
                if (!whenSyntaxMatch.Success)
                    throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagWhenSyntaxException"));

                markup = whenSyntaxMatch.Groups[2].Value;
                if (string.IsNullOrEmpty(markup))
                    markup = null;

                Condition block = new Condition(_left, "==", whenSyntaxMatch.Groups[1].Value);
                block.Attach(NodeList);
                _blocks.Add(block);
            }
        }

        private void RecordElseCondition(string markup)
        {
            if (markup.Trim() != string.Empty)
                throw new SyntaxException(Liquid.ResourceManager.GetString("CaseTagElseSyntaxException"));

            ElseCondition block = new ElseCondition();
            block.Attach(NodeList);
            _blocks.Add(block);
        }
    }
}
