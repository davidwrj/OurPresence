using System.IO;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class Continue : Tag
    {
        public override void Render(Context context, TextWriter result)
        {
            throw new ContinueInterrupt();
        }
    }
}
