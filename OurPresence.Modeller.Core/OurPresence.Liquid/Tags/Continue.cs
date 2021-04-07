using System.IO;
using OurPresence.Liquid.Exceptions;

namespace OurPresence.Liquid.Tags
{
    public class Continue : Tag
    {
        public override void Render(Context context, TextWriter result)
        {
            throw new ContinueInterrupt();
        }
    }
}
