// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Raw(Template template, string tagName, string markup) 
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        protected override void Parse(IEnumerable<string> tokens)
        {
            NodeList.Clear();

            string token;
            var t = tokens as List<string>;
            while ((token = t.Shift()) != null)
            {
                var fullTokenMatch = FullToken.Match(token);
                if (fullTokenMatch.Success && BlockDelimiter == fullTokenMatch.Groups[1].Value)
                {
                    EndTag();
                    return;
                }
                else
                {
                    NodeList.Add(token);
                }
            }

            AssertMissingDelimitation();
        }
    }
}
