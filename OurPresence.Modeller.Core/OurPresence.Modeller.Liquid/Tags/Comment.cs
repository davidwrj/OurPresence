using System.IO;
using System.Text.RegularExpressions;

using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class Comment : OurPresence.Modeller.Liquid.Block
    {
        private static readonly Regex ShortHandRegex = R.C(Liquid.CommentShorthand);

        public static string FromShortHand(string @string)
        {
            if (@string == null)
                return @string;

            Match match = ShortHandRegex.Match(@string);
            return match.Success ? string.Format(@"{{% comment %}}{0}{{% endcomment %}}", match.Groups[1].Value) : @string;
        }

        public override void Render(Context context, TextWriter result)
        {
        }
    }
}
