using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Util;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class RegexpTests
    {
        [Fact]
        public void TestAllRegexesAreCompiled()
        {
            var assembly = typeof (Template).GetTypeInfo().Assembly;
            foreach (Type parent in assembly.GetTypes())
            {
                foreach (var t in parent.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (t.FieldType == typeof(Regex))
                    {
                        if (t.IsStatic)
                        {
                            Assert.NotEqual(RegexOptions.None,RegexOptions.Compiled & ((Regex) t.GetValue(null)).Options);
                        }
                        else
                        {
                            Assert.NotEqual(RegexOptions.None, RegexOptions.Compiled & ((Regex)t.GetValue(parent)).Options);
                        }

                        //Trace.TraceInformation(parent.Name + ": " + t.Name);
                    }
                }
            }
        }

        [Fact]
        public void TestEmpty()
        {
            Run(string.Empty, Liquid.QuotedFragment).Should().BeEmpty();
        }

        [Fact]
        public void TestQuote()
        {
            Run("\"arg 1\"", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "\"arg 1\"" });
        }

        [Fact]
        public void TestWords()
        {
            Run("arg1 arg2", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "arg1", "arg2" });
        }

        [Fact]
        public void TestTags()
        {
            Run("<tr> </tr>", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "<tr>", "</tr>" });
            Run("<tr></tr>", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "<tr></tr>" });
            Run("<style class=\"hello\">' </style>", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "<style", "class=\"hello\">", "</style>" });
        }

        [Fact]
        public void TestQuotedWords()
        {
            Run("arg1 arg2 \"arg 3\"", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "arg1", "arg2", "\"arg 3\"" });
        }

        [Fact]
        public void TestQuotedWords2()
        {
            Run("arg1 arg2 'arg 3'", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "arg1", "arg2", "'arg 3'" });
        }

        [Fact]
        public void TestQuotedWordsInTheMiddle()
        {
            Run("arg1 arg2 \"arg 3\" arg4", Liquid.QuotedFragment).Should().BeEquivalentTo(new[] { "arg1", "arg2", "\"arg 3\"", "arg4" });
        }

        [Fact]
        public void TestVariableParser()
        {
            Run("var", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var" });
            Run("var.method", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var", "method" });
            Run("var[method]", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var", "[method]" });
            Run("var[method][0]", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var", "[method]", "[0]" });
            Run("var[\"method\"][0]", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var", "[\"method\"]", "[0]" });
            Run("var[method][0].method", Liquid.VariableParser).Should().BeEquivalentTo(new[] { "var", "[method]", "[0]", "method" });
        }

        private static List<string> Run(string input, string pattern)
        {
            return R.Scan(input, new Regex(pattern));
        }
    }
}
