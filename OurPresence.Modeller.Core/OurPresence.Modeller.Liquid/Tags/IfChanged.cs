using System.IO;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class IfChanged : OurPresence.Modeller.Liquid.Block
    {
        public override void Render(Context context, TextWriter result)
        {
            context.Stack(() =>
            {
                string tempString;
                using (TextWriter temp = new StringWriter(result.FormatProvider))
                {
                    RenderAll(NodeList, context, temp);
                    tempString = temp.ToString();
                }

                if (tempString != (context.Registers["ifchanged"] as string))
                {
                    context.Registers["ifchanged"] = tempString;
                    result.Write(tempString);
                }
            });
        }
    }
}
