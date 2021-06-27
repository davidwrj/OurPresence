// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    /// The Extends tag is used in conjunction with the Block tag to provide template inheritance.
    /// </summary>
    /// <example>
    /// To see how Extends and Block can be used together, start by considering this example:
    ///
    /// <html>
    /// <head>
    ///   <title>{% block title %}My Website{% endblock %}</title>
    /// </head>
    ///
    /// <body>
    ///   <div id="sidebar">
    ///     {% block sidebar %}
    ///     <ul>
    ///       <li><a href="/">Home</a></li>
    ///       <li><a href="/blog/">Blog</a></li>
    ///     </ul>
    ///     {% endblock %}
    ///   </div>
    ///
    ///   <div id="content">
    ///     {% block content %}{% endblock %}
    ///   </div>
    /// </body>
    /// </html>
    ///
    /// We'll assume this is saved in a file called base.html. In ASP.NET MVC terminology, this file would
    /// be the master page or layout, and each of the "blocks" would be a section. Child templates
    /// (in ASP.NET MVC terminology, views) fill or override these blocks with content. If a child template
    /// does not define a particular block, then the content from the parent template is used as a fallback.
    ///
    /// A child template might look like this:
    ///
    /// {% extends "base.html" %}
    /// {% block title %}My AMAZING Website{% endblock %}
    ///
    /// {% block content %}
    /// {% for entry in blog_entries %}
    ///   <h2>{{ entry.title }}</h2>
    ///   <p>{{ entry.body }}</p>
    /// {% endfor %}
    /// {% endblock %}
    ///
    /// The current IFileSystem will be used to locate "base.html".
    /// </example>
    public class Extends : Modeller.Liquid.Block
    {
        private static readonly Regex s_syntax = R.B(@"^({0})", Liquid.QuotedFragment);
        private string _templateName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Extends(Template template, string tagName, string markup)
            : base(template, tagName, markup)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public override void Initialize(IEnumerable<string> tokens)
        {
            var syntaxMatch = s_syntax.Match(Markup);

            _templateName = syntaxMatch.Success
                ? syntaxMatch.Groups[1].Value
                : throw new SyntaxException(Liquid.ResourceManager.GetString("ExtendsTagSyntaxException"));

            base.Initialize(tokens);
        }

        internal override void AssertTagRulesViolation(NodeList rootNodeList)
        {
            if (!(rootNodeList.GetItems().First() is Extends))
            {
                throw new SyntaxException(Liquid.ResourceManager.GetString("ExtendsTagMustBeFirstTagException"));
            }

            NodeList.GetItems().ForEach(n =>
            {
                if (!(n is string && ((string)n).IsNullOrWhiteSpace() || n is Block || n is Comment || n is Extends))
                {
                    throw new SyntaxException(Liquid.ResourceManager.GetString("ExtendsTagUnallowedTagsException"));
                }
            });

            if (NodeList.Count(o => o is Extends) > 0)
            {
                throw new SyntaxException(Liquid.ResourceManager.GetString("ExtendsTagCanBeUsedOneException"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public override void Render(Context context, TextWriter result)
        {
            // Get the template or template content and then either copy it (since it will be modified) or parse it
            var fileSystem = context.Registers["file_system"] as IFileSystem ?? Template.FileSystem;
            var templateFileSystem = fileSystem as ITemplateFileSystem;
            Template template = null;
            if (templateFileSystem != null)
            {
                template = templateFileSystem.GetTemplate(context, _templateName);
            }
            if (template == null)
            {
                var source = fileSystem.ReadTemplateFile(context, _templateName);
                template = Template.Parse(source, template.FileSystem);
            }

            var parentBlocks = FindBlocks(template.Root, null);
            var orphanedBlocks = ((List<Block>)context.Scopes.First()["extends"]) ?? new List<Block>();
            var blockState = BlockRenderState.Find(context) ?? new BlockRenderState();

            context.Stack(() =>
            {
                context["blockstate"] = blockState;         // Set or copy the block state down to this scope
                context["extends"] = new List<Block>();     // Holds Blocks that were not found in the parent
                foreach (var block in NodeList.OfType<Block>().Concat(orphanedBlocks))
                {
                    var pb = parentBlocks.Find(b => b.BlockName == block.BlockName);

                    if (pb != null)
                    {
                        if (blockState.Parents.TryGetValue(block, out var parent))
                        {
                            blockState.Parents[pb] = parent;
                        }

                        pb.AddParent(blockState.Parents, pb.GetNodeList(blockState));
                        blockState.NodeLists[pb] = block.GetNodeList(blockState);
                    }
                    else if (IsExtending(template))
                    {
                        ((List<Block>)context.Scopes.First()["extends"]).Add(block);
                    }
                }
                template.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public bool IsExtending(Template template)
        {
            return template.Root.NodeList.Any(node => node is Extends);
        }

        private List<Block> FindBlocks(object node, List<Block> blocks)
        {
            if (blocks == null)
            {
                blocks = new List<Block>();
            }

            if (node.RespondTo("NodeList"))
            {
                var nodeList = (List<object>)node.Send("NodeList");

                if (nodeList != null)
                {
                    nodeList.ForEach(n =>
                    {
                        var block = n as Block;

                        if (block != null && blocks.All(bl => bl.BlockName != block.BlockName))
                        {
                            blocks.Add(block);
                        }

                        FindBlocks(n, blocks);
                    });
                }
            }

            return blocks;
        }
    }
}
