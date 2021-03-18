using FluentAssertions;
using OurPresence.Modeller.Liquid.Exceptions;
using System;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class ParsingQuirksTests
    {
        [Fact]
        public void TestErrorWithCss()
        {
            const string text = " div { font-weight: bold; } ";
            Template template = Template.Parse(text);
            Assert.Equal(text, template.Render());
            Assert.Equal(1, template.Root.NodeList.Count);
            template.Root.NodeList[0].Should().BeOfType<string>();
        }

        [Fact]
        public void TestRaiseOnSingleCloseBrace()
        {
            Assert.Throws<SyntaxException>(() => Template.Parse("text {{method} oh nos!"));
        }

        [Fact]
        public void TestRaiseOnLabelAndNoCloseBrace()
        {
            Assert.Throws<SyntaxException>(() => Template.Parse("TEST {{ "));
        }

        [Fact]
        public void TestRaiseOnLabelAndNoCloseBracePercent()
        {
            Assert.Throws<SyntaxException>(() => Template.Parse("TEST {% "));
        }

        [Fact]
        public void TestErrorOnEmptyFilter()
        {
            Action action = () =>
            {
                Template.Parse("{{test |a|b|}}");
                Template.Parse("{{test}}");
                Template.Parse("{{|test|}}");
            };

            action.Should().NotThrow();
        }

        [Fact]
        public void TestMeaninglessParens()
        {
            Hash assigns = Hash.FromAnonymousObject(new { b = "bar", c = "baz" });
            Helper.AssertTemplateResult(" YES ", "{% if a == 'foo' or (b == 'bar' and c == 'baz') or false %} YES {% endif %}", assigns);
        }

        [Fact]
        public void TestUnexpectedCharactersSilentlyEatLogic()
        {
            Helper.AssertTemplateResult(" YES ", "{% if true && false %} YES {% endif %}");
            Helper.AssertTemplateResult("", "{% if false || true %} YES {% endif %}");
        }
    }
}
