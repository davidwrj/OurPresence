using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OurPresence.Liquid.Exceptions;
using OurPresence.Liquid.FileSystems;
using OurPresence.Liquid.Util;

namespace OurPresence.Liquid.Tags
{
    /// <summary>
    /// The Extends tag is used in conjunction with the Block tag to provide template inheritance.
    /// For further syntax and usage please refer to
    /// <see cref="http://docs.djangoproject.com/en/dev/topics/templates/#template-inheritance"/>
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
    public class Extends : OurPresence.Liquid.Block
    {

        private static readonly Regex s_syntax = R.B(@"^({0})", Liquid.QuotedFragment);

        private string _templateName;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = s_syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                _templateName = syntaxMatch.Groups[1].Value;
            }
            else
                throw new SyntaxException("Syntax Error in 'extends' tag - Valid syntax: extends [template]");

            base.Initialize(tagName, markup, tokens);
        }

        internal override void AssertTagRulesViolation(List<object> rootNodeList)
        {
            if (!(rootNodeList[0] is Extends))
            {
                throw new SyntaxException("Liquid Error - 'extends' must be the first tag in an extending template");
            }

            NodeList.ForEach(n =>
            {
                if (!((n is string && ((string) n).IsNullOrWhiteSpace()) || n is Block || n is Comment || n is Extends))
                    throw new SyntaxException("Liquid Error - Only 'comment' and 'block' tags are allowed in an extending template");
            });

            if (NodeList.Count(o => o is Extends) > 0)
            {
                throw new SyntaxException("Liquid Error - 'extends' tag can be used only once");
            }
        }

        protected override void AssertMissingDelimitation()
        {
        }

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
                template = Template.Parse(source);
            }

            var parentBlocks = FindBlocks(template.Root, null);
            var orphanedBlocks = ((List<Block>)context.Scopes[0]["extends"]) ?? new List<Block>();
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
                            blockState.Parents[pb] = parent;
                        pb.AddParent(blockState.Parents, pb.GetNodeList(blockState));
                        blockState.NodeLists[pb] = block.GetNodeList(blockState);
                    }
                    else if(IsExtending(template))
                    {
                        ((List<Block>)context.Scopes[0]["extends"]).Add(block);
                    }
                }
                template.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
            });
        }

        public bool IsExtending(Template template)
        {
            return template.Root.NodeList.Any(node => node is Extends);
        }

        private List<Block> FindBlocks(object node, List<Block> blocks)
        {
            if(blocks == null) blocks = new List<Block>();

            if (node.RespondTo("NodeList"))
            {
                var nodeList = (List<object>) node.Send("NodeList");

                if (nodeList != null)
                {
                    nodeList.ForEach(n =>
                    {
                        var block = n as Block;

                        if (block != null)
                        {
                            if (blocks.All(bl => bl.BlockName != block.BlockName)) blocks.Add(block);
                        }

                        FindBlocks(n, blocks);
                    });
                }
            }

            return blocks;
        }
    }
}
