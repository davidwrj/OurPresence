// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Resources;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Utiliy containing regexes for Liquid syntax and registering default tags and blocks
    /// </summary>
    public static class Liquid
    {
        internal static readonly ResourceManager ResourceManager = new ResourceManager(typeof(Properties.Resources));

        /// <summary>
        /// 
        /// </summary>
        public static readonly string FilterSeparator = R.Q(@"\|");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string ArgumentSeparator = R.Q(@",");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string FilterArgumentSeparator = R.Q(@":");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableAttributeSeparator = R.Q(@".");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string TagStart = R.Q(@"\{\%");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string TagEnd = R.Q(@"\%\}");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableSignature = R.Q(@"\(?[\w\-\.\[\]]\)?");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableSegment = R.Q(@"[\w\-]");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableStart = R.Q(@"\{\{");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableEnd = R.Q(@"\}\}");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableIncompleteEnd = R.Q(@"\}\}?");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string QuotedString = R.Q(@"""[^""]*""|'[^']*'");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string QuotedFragment = string.Format(R.Q(@"{0}|(?:[^\s,\|'""]|{0})+"), QuotedString);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string QuotedAssignFragment = string.Format(R.Q(@"{0}|(?:[^\s\|'""]|{0})+"), QuotedString);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string StrictQuotedFragment = R.Q(@"""[^""]+""|'[^']+'|[^\s\|\:\,]+");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string FirstFilterArgument = string.Format(R.Q(@"{0}(?:{1})"), FilterArgumentSeparator, StrictQuotedFragment);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string OtherFilterArgument = string.Format(R.Q(@"{0}(?:{1})"), ArgumentSeparator, StrictQuotedFragment);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string SpacelessFilter = string.Format(R.Q(@"^(?:'[^']+'|""[^""]+""|[^'""])*{0}(?:{1})(?:{2}(?:{3})*)?"), FilterSeparator, StrictQuotedFragment, FirstFilterArgument, OtherFilterArgument);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Expression = string.Format(R.Q(@"(?:{0}(?:{1})*)"), QuotedFragment, SpacelessFilter);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string TagAttributes = string.Format(R.Q(@"(\w+)\s*\:\s*({0})"), QuotedFragment);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string AnyStartingTag = R.Q(@"\{\{|\{\%");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string PartialTemplateParser = string.Format(R.Q(@"{0}.*?{1}|{2}.*?{3}"), TagStart, TagEnd, VariableStart, VariableIncompleteEnd);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string TemplateParser = string.Format(R.Q(@"({0}|{1})"), PartialTemplateParser, AnyStartingTag);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VariableParser = string.Format(R.Q(@"\[[^\]]+\]|{0}+\??"), VariableSegment);
        /// <summary>
        /// 
        /// </summary>
        public static readonly string LiteralShorthand = R.Q(@"^(?:\{\{\{\s?)(.*?)(?:\s*\}\}\})$");
        /// <summary>
        /// 
        /// </summary>
        public static readonly string CommentShorthand = R.Q(@"^(?:\{\s?\#\s?)(.*?)(?:\s*\#\s?\})$");
        /// <summary>
        /// 
        /// </summary>
        public static readonly bool UseRubyDateFormat = false;

        /// <summary>
        /// 
        /// </summary>
        static Liquid()
        {
            //Template.RegisterTag<Tags.Assign>("assign");
            //Template.RegisterTag<Tags.Block>("block");
            //Template.RegisterTag<Tags.Capture>("capture");
            //Template.RegisterTag<Tags.Case>("case");
            //Template.RegisterTag<Tags.Comment>("comment");
            //Template.RegisterTag<Tags.Cycle>("cycle");
            //Template.RegisterTag<Tags.Extends>("extends");
            //Template.RegisterTag<Tags.For>("for");
            //Template.RegisterTag<Tags.Break>("break");
            //Template.RegisterTag<Tags.Continue>("continue");
            //Template.RegisterTag<Tags.If>("if");
            //Template.RegisterTag<Tags.IfChanged>("ifchanged");
            //Template.RegisterTag<Tags.Include>("include");
            //Template.RegisterTag<Tags.Literal>("literal");
            //Template.RegisterTag<Tags.Unless>("unless");
            //Template.RegisterTag<Tags.Raw>("raw");

            //Template.RegisterTag<Tags.Html.TableRow>("tablerow");

            Template.RegisterFilter(typeof(StandardFilters));
        }
    }
}
