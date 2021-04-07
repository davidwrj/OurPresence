using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Liquid.Exceptions;
using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags
{
    /// <summary>
    /// Cycle is usually used within a loop to alternate between values, like colors or DOM classes.
    ///
    ///   {% for item in items %}
    ///    &lt;div class="{% cycle 'red', 'green', 'blue' %}"&gt; {{ item }} &lt;/div&gt;
    ///   {% end %}
    ///
    ///    &lt;div class="red"&gt; Item one &lt;/div&gt;
    ///    &lt;div class="green"&gt; Item two &lt;/div&gt;
    ///    &lt;div class="blue"&gt; Item three &lt;/div&gt;
    ///    &lt;div class="red"&gt; Item four &lt;/div&gt;
    ///    &lt;div class="green"&gt; Item five&lt;/div&gt;
    /// </summary>
    public class Cycle : Tag
    {
        private static readonly Regex s_simpleSyntax = R.B(R.Q(@"^{0}+"), Liquid.QuotedFragment);
        private static readonly Regex s_namedSyntax = R.B(R.Q(@"^({0})\s*\:\s*(.*)"), Liquid.QuotedFragment);
        private static readonly Regex s_quotedFragmentRegex = R.B(R.Q(@"\s*({0})\s*"), Liquid.QuotedFragment);

        private string[] _variables;
        private string _name;

        /// <summary>
        /// Initializes the cycle tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        /// <param name="tokens"></param>
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var match = s_namedSyntax.Match(markup);
            if (match.Success)
            {
                _variables = VariablesFromString(match.Groups[2].Value);
                _name = match.Groups[1].Value;
            }
            else
            {
                match = s_simpleSyntax.Match(markup);
                if (match.Success)
                {
                    _variables = VariablesFromString(markup);
                    _name = "'" + string.Join(string.Empty, _variables) + "'";
                }
                else
                {
                    throw new SyntaxException("Syntax Error in 'cycle' tag - Valid syntax: cycle [name :] var [, var2, var3 ...]");
                }
            }

            base.Initialize(tagName, markup, tokens);
        }

        private static string[] VariablesFromString(string markup)
        {
            return markup.Split(',').Select(var =>
            {
                var match = s_quotedFragmentRegex.Match(var);
                return (match.Success && !string.IsNullOrEmpty(match.Groups[1].Value))
                    ? match.Groups[1].Value
                    : null;
            }).ToArray();
        }

        /// <summary>
        /// Renders the cycle tag
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            context.Registers["cycle"] = context.Registers["cycle"] ?? new Hash(0);

            context.Stack(() =>
            {
                var key = context[_name].ToString();
                var iteration = (int) (((Hash) context.Registers["cycle"])[key] ?? 0);
                result.Write(context[_variables[iteration]].ToString());
                ++iteration;
                if (iteration >= _variables.Length)
                    iteration = 0;
                ((Hash) context.Registers["cycle"])[key] = iteration;
            });
        }
    }
}
