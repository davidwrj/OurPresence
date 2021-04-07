using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid.Tags
{
    public class BlockDrop : Drop
    {
        private readonly Block _block;
        private readonly TextWriter _result;

        public BlockDrop(Template template, Block block, TextWriter result)
            :base(template)
        {
            _block = block;
            _result = result;
        }

        public void Super()
        {
            _block.CallSuper(Context, _result);
        }
    }

    // Keeps track of the render-time state of all Blocks for a given Context
    internal class BlockRenderState
    {
        public BlockRenderState()
        { }

        public Dictionary<Block, Block> Parents { get; } = new Dictionary<Block, Block>();

        public Dictionary<Block, NodeList> NodeLists { get; } = new Dictionary<Block, NodeList>();

        public NodeList GetNodeList(Block block)
        {
            if (!NodeLists.TryGetValue(block, out NodeList nodeList))
                nodeList = block.NodeList;
            return nodeList;
        }

        // Searches up the scopes for the inner-most BlockRenderState (though there should be only one)
        public static BlockRenderState Find(Context context)
        {
            foreach (Hash scope in context.Scopes)
            {
                if (scope.TryGetValue("blockstate", out object blockState))
                {
                    return blockState as BlockRenderState;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// The Block tag is used in conjunction with the Extends tag to provide template inheritance.
    /// For an example please refer to the Extends tag.
    /// </summary>
    public class Block : Modeller.Liquid.Block
    {
        private readonly Regex _syntax;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tagName"></param>
        /// <param name="markup"></param>
        public Block(Template template, string tagName, string markup)
            :base(template, tagName, markup)
        {
            _syntax = R.C(template,@"(\w+)");
        }

        internal string BlockName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public override void Initialize(IEnumerable<string> tokens)
        {
            var syntaxMatch = _syntax.Match(Markup);
            if (syntaxMatch.Success)
            {
                BlockName = syntaxMatch.Groups[1].Value;
            }
            else
            {
                throw new SyntaxException(Liquid.ResourceManager.GetString("BlockTagSyntaxException"));
            }

            if (tokens is not null)
            {
                base.Initialize(tokens);
            }
        }

        internal override void AssertTagRulesViolation(NodeList rootNodeList)
        {
            rootNodeList.GetItems().ForEach(n =>
                {
                    Block b1 = n as Block;

                    if (b1 != null)
                    {
                        List<object> found = rootNodeList.FindAll(o =>
                            {
                                Block b2 = o as Block;
                                return b2 != null && b1.BlockName == b2.BlockName;
                            });

                        if (found != null && found.Count > 1)
                        {
                            throw new SyntaxException(Liquid.ResourceManager.GetString("BlockTagAlreadyDefinedException"), b1.BlockName);
                        }
                    }
                });
        }

        public override void Render(Context context, TextWriter result)
        {
            BlockRenderState blockState = BlockRenderState.Find(context);
            context.Stack(() =>
                {
                    context["block"] = new BlockDrop(context.Template, this, result);
                    RenderAll(GetNodeList(blockState), context, result);
                });
        }

        // Gets the render-time node list from the node state
        internal NodeList GetNodeList(BlockRenderState blockState)
        {
            return blockState == null ? NodeList : blockState.GetNodeList(this);
        }

        public void AddParent(Dictionary<Block, Block> parents, NodeList nodeList)
        {
            if (parents.TryGetValue(this, out Block parent))
            {
                parent.AddParent(parents, nodeList);
            }
            else
            {
                parent = new Block(Template, TagName, Markup);
                parent.Initialize(null);
                parents[this] = parent;
            }
        }

        public void CallSuper(Context context, TextWriter result)
        {
            BlockRenderState blockState = BlockRenderState.Find(context);
            if (blockState != null
                && blockState.Parents.TryGetValue(this, out Block parent)
                && parent != null)
            {
                parent.Render(context, result);
            }
        }
    }
}
