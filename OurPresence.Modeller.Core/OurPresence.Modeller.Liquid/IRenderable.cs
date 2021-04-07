using System.IO;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Object that can render itslef
    /// </summary>
    internal interface IRenderable
    {
        Template Template { get; }

        void Render(Context context, TextWriter result);
    }
}
