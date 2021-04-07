using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// Raw
    /// Raw outputs text as is, usefull if your template contains Liquid syntax.
    ///
    /// {% raw %}{% if user = 'tobi' %}hi{% endif %}{% endraw %}
    /// </summary>
    public class Raw : Modeller.Liquid.Block
    {
        public Raw(Template template, string tagName, string markup) : base(template, tagName, markup)
        { }

        protected override void Parse(IEnumerable<string> tokens)
        {
            NodeList.Clear();

            string token;
            var t = tokens as List<string>;
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
