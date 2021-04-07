using System.IO;
using OurPresence.Liquid.Exceptions;

namespace OurPresence.Liquid.Tags
{
    public class Break : Tag
    {
        public override void Render(Context context, TextWriter result)
        {
            throw new BreakInterrupt();
        }
    }
}
