// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// 
    /// </summary>
    public class Comment : Modeller.Liquid.Block
    {
        private static readonly Regex s_shortHandRegex = R.C(Liquid.CommentShorthand);

        /// <summary>
        /// 
        /// </summary>
        public Comment(Template template, string tagName, string markup)
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string FromShortHand(string @string)
        {
            if (@string == null)
            {
                return @string;
            }

            var match = s_shortHandRegex.Match(@string);
            return match.Success ? string.Format(@"{{% comment %}}{0}{{% endcomment %}}", match.Groups[1].Value) : @string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        { }
    }
}
