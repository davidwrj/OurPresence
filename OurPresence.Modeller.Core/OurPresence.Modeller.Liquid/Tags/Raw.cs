using System.Collections.Generic;
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
    public class Raw : OurPresence.Modeller.Liquid.Block
    {
        protected override void Parse(List<string> tokens)
        {
            NodeList = NodeList ?? new List<object>();
            NodeList.Clear();

            string token;
            while ((token = tokens.Shift()) != null)
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
