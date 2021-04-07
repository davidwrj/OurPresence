using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// Capture stores the result of a block into a variable without rendering it inplace.
    ///
    /// {% capture heading %}
    /// Monkeys!
    /// {% endcapture %}
    /// ...
    /// <h1>{{ heading }}</h1>
    ///
    /// Capture is useful for saving content for use later in your template, such as
    /// in a sidebar or footer.
    /// </summary>
    public class Capture : Modeller.Liquid.Block
    {
        private static readonly Regex Syntax = R.C(@"(\w+)");
        private string _to;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Capture(Template template, string tagName, string markup) : base(template, tagName, markup)
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public override void Initialize(IEnumerable<string> tokens)
        {
            Match syntaxMatch = Syntax.Match(Markup);
            if (syntaxMatch.Success)
                _to = syntaxMatch.Groups[1].Value;
            else
                throw new SyntaxException(Liquid.ResourceManager.GetString("CaptureTagSyntaxException"));

            base.Initialize(tokens);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            using (TextWriter temp = new StringWriter(result.FormatProvider))
            {
                base.Render(context, temp);
                context.Scopes.Last()[_to] = temp.ToString();
            }
        }
    }
}
