using System.IO;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class Break : Tag
    {
        public override void Render(Context context, TextWriter result)
        {
            throw new BreakInterrupt();
        }
    }
}
