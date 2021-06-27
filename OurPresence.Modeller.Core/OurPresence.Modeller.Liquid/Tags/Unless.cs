// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// Unless is a conditional just like 'if' but works on the inverse logic.
    ///
    ///  {% unless x &lt; 0 %} x is greater than zero {% end %}
    /// </summary>
    public class Unless : If
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Unless(Template template, string tagName, string markup) 
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            context.Stack(() =>
            {
                // First condition is interpreted backwards (if not)
                var block = Blocks.First();
                if (!block.Evaluate(Template, context, result.FormatProvider))
                {
                    RenderAll(new NodeList( block.Attachment), context, result);
                    return;
                }

                // After the first condition unless works just like if
                foreach (var forEachBlock in Blocks.Skip(1))
                {
                    if (forEachBlock.Evaluate(Template, context, result.FormatProvider))
                    {
                        RenderAll(new NodeList(forEachBlock.Attachment), context, result);
                        return;
                    }
                }
            });
        }
    }
}
