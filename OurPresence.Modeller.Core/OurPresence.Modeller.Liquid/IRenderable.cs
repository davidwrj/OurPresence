using System.IO;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Object that can render itslef
    /// </summary>
    internal interface IRenderable
    {
        void Render(Context context, TextWriter result);
    }
}
