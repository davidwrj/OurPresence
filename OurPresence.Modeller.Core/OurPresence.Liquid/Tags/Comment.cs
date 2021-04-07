using System.IO;
using System.Text.RegularExpressions;

using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags
{
    public class Comment : OurPresence.Liquid.Block
    {
        private static readonly Regex s_shortHandRegex = R.C(Liquid.CommentShorthand);

        public static string FromShortHand(string @string)
        {
            if (@string == null)
                return @string;

            var match = s_shortHandRegex.Match(@string);
            return match.Success ? string.Format(@"{{% comment %}}{0}{{% endcomment %}}", match.Groups[1].Value) : @string;
        }

        public override void Render(Context context, TextWriter result)
        {
        }
    }
}
