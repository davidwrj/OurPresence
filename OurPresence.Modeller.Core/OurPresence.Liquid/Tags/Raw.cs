using System.Collections.Generic;
using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags
{
    /// <summary>
    /// Raw
    /// Raw outputs text as is, usefull if your template contains Liquid syntax.
    ///
    /// {% raw %}{% if user = 'tobi' %}hi{% endif %}{% endraw %}
    /// </summary>
    public class Raw : OurPresence.Liquid.Block
    {
        protected override void Parse(List<string> tokens)
        {
            NodeList = NodeList ?? new List<object>();
            NodeList.Clear();

            string token;
            while ((token = tokens.Shift()) != null)
            {
                var fullTokenMatch = FullToken.Match(token);
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
