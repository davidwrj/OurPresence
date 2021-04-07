using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Liquid.Exceptions;
using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags.Html
{
    /// <summary>
    /// TablerRow tag
    /// </summary>
    /// <example>
    /// &lt;table&gt;
    ///   {% tablerow product in collection.products %}
    ///     {{ product.title }}
    ///   {% endtablerow %}
    /// &lt;/table&gt;
    /// </example>
    public class TableRow : OurPresence.Liquid.Block
    {
        private static readonly Regex s_syntax = R.B(R.Q(@"(\w+)\s+in\s+({0}+)"), Liquid.VariableSignature);

        private string _variableName, _collectionName;
        private Dictionary<string, string> _attributes;

        /// <summary>
        /// Initializes the tablerow tag
        /// </summary>
        /// <param name="tagName">Name of the parsed tag</param>
        /// <param name="markup">Markup of the parsed tag</param>
        /// <param name="tokens">Toeksn of the parsed tag</param>
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = s_syntax.Match(markup);
            if (syntaxMatch.Success)
            {
                _variableName = syntaxMatch.Groups[1].Value;
                _collectionName = syntaxMatch.Groups[2].Value;
                _attributes = new Dictionary<string, string>(Template.NamingConvention.StringComparer);
                R.Scan(markup, Liquid.TagAttributes, (key, value) => _attributes[key] = value);
            }
            else
                throw new SyntaxException("Syntax Error in 'tablerow' tag - Valid syntax: tablerow [item] in [collection] cols=[number]");

            base.Initialize(tagName, markup, tokens);
        }

        /// <summary>
        /// Renders the tablerow tag
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            var coll = context[_collectionName];

            if (!(coll is IEnumerable))
                return;
            var collection = ((IEnumerable) coll).Cast<object>();

            if (_attributes.ContainsKey("offset"))
            {
                var offset = Convert.ToInt32(_attributes["offset"]);
                collection = collection.Skip(offset);
            }

            if (_attributes.ContainsKey("limit"))
            {
                var limit = Convert.ToInt32(_attributes["limit"]);
                collection = collection.Take(limit);
            }

            collection = collection.ToList();
            var length = collection.Count();

            var cols = Convert.ToInt32(context[_attributes["cols"]]);

            var row = 1;
            var col = 0;

            result.WriteLine("<tr class=\"row1\">");
            context.Stack(() => collection.EachWithIndex((item, index) =>
            {
                context[_variableName] = item;
                context["tablerowloop"] = Hash.FromAnonymousObject(new
                {
                    length = length,
                    index = index + 1,
                    index0 = index,
                    col = col + 1,
                    col0 = col,
                    rindex = length - index,
                    rindex0 = length - index - 1,
                    first = (index == 0),
                    last = (index == length - 1),
                    col_first = (col == 0),
                    col_last = (col == cols - 1)
                });

                ++col;

                using (TextWriter temp = new StringWriter(result.FormatProvider))
                {
                    RenderAll(NodeList, context, temp);
                    result.Write("<td class=\"col{0}\">{1}</td>", col, temp.ToString());
                }

                if (col == cols && index != length - 1)
                {
                    col = 0;
                    ++row;
                    result.WriteLine("</tr>");
                    result.Write("<tr class=\"row{0}\">", row);
                }
            }));
            result.WriteLine("</tr>");
        }
    }
}
