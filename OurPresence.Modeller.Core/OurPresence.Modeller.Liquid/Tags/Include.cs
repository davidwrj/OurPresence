// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.FileSystems;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    /// <summary>
    /// 
    /// </summary>
    public class Include : Modeller.Liquid.Block
    {
        private readonly Regex _syntax = R.B(@"({0}+)(\s+(?:with|for)\s+({0}+))?", Liquid.QuotedFragment);
        private string _templateName, _variableName;
        private Dictionary<string, string> _attributes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Include(Template template, string tagName, string markup) 
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public override void Initialize(IEnumerable<string> tokens)
        {
            var syntaxMatch = _syntax.Match(Markup);
            if (syntaxMatch.Success)
            {
                _templateName = syntaxMatch.Groups[1].Value;
                _variableName = syntaxMatch.Groups[3].Value;
                if (_variableName == string.Empty)
                {
                    _variableName = null;
                }

                _attributes = new Dictionary<string, string>();
                R.Scan(Markup, Liquid.TagAttributes, (key, value) => _attributes[key] = value);
            }
            else
            {
                throw new SyntaxException(Liquid.ResourceManager.GetString("IncludeTagSyntaxException"));
            }

            base.Initialize(tokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            var fileSystem = context.Registers["file_system"] as IFileSystem ?? Template.FileSystem;
            Template partial = null;
            if (fileSystem is ITemplateFileSystem templateFileSystem)
            {
                partial = templateFileSystem.GetTemplate(context, _templateName);
            }
            if (partial == null)
            {
                var source = fileSystem.ReadTemplateFile(context, _templateName);
                partial = Template.Parse(source, Template.FileSystem);
            }

            var shortenedTemplateName = _templateName[1..^1];
            var variable = context[_variableName ?? shortenedTemplateName, _variableName != null];

            context.Stack(() =>
            {
                foreach (var keyValue in _attributes)
                {
                    context[keyValue.Key] = context[keyValue.Value];
                }

                if (variable is IEnumerable enumerable)
                {
                    enumerable.Cast<object>().ToList().ForEach(v =>
                    {
                        context[shortenedTemplateName] = v;
                        partial.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
                    });
                    return;
                }

                context[shortenedTemplateName] = variable;
                partial.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
            });
        }
    }
}
