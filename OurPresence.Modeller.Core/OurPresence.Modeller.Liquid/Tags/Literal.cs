using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// Literal
    /// Literal outputs text as is, usefull if your template contains Liquid syntax.
    ///
    /// {% literal %}{% if user = 'tobi' %}hi{% endif %}{% endliteral %}
    ///
    /// or (shorthand version)
    ///
    /// {{{ {% if user = 'tobi' %}hi{% endif %} }}}
    /// </summary>
    public class Literal : Modeller.Liquid.Block
    {
        private static readonly Regex LiteralRegex = R.C(Liquid.LiteralShorthand);

        public Literal(Template template, string tagName, string markup) : base(template, tagName, markup)
        { }

        /// <summary>
        /// Creates a literal from shorthand
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string FromShortHand(string @string)
        {
            if (@string == null)
                return @string;

            Match match = LiteralRegex.Match(@string);
            return match.Success ? string.Format(@"{{% literal %}}{0}{{% endliteral %}}", match.Groups[1].Value) : @string;
        }

        /// <summary>
        /// Parses the tag
        /// </summary>
        /// <param name="tokens"></param>
        protected override void Parse(IEnumerable<string> tokens)
        {
            NodeList.Clear();

            string token;
            var t = tokens.ToList();
            while ((token = t.Shift()) != null)
            {
                Match fullTokenMatch = FullToken.Match(token);
                if (fullTokenMatch.Success && BlockDelimiter == fullTokenMatch.Groups[1].Value)
                {
                    EndTag();
                    return;
                }
                else
                    NodeList.Add(token);
            }

            AssertMissingDelimitation();
        }
    }
}
