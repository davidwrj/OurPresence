using System.IO;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class Continue : Tag
    {
        public Continue(Template template, string tagName, string markup) : base(template, tagName, markup)
        { }

        public override void Render(Context context, TextWriter result)
        {
            throw new ContinueInterrupt();
        }
    }
}
