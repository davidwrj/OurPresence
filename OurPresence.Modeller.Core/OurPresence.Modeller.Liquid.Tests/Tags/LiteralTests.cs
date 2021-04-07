using Xunit;
using OurPresence.Modeller.Liquid.Tags;

namespace OurPresence.Modeller.Liquid.Tests.Tags
{
    public class LiteralTests
    {
        [Fact]
        public void TestEmptyLiteral()
        {
            Template t = Template.Parse("{% literal %}{% endliteral %}");
            Assert.Equal(string.Empty, t.Render());
            t = Template.Parse("{{{}}}");
            Assert.Equal(string.Empty, t.Render());
        }

        [Fact]
        public void TestSimpleLiteralValue()
        {
            Template t = Template.Parse("{% literal %}howdy{% endliteral %}");
            Assert.Equal("howdy", t.Render());
        }

        [Fact]
        public void TestLiteralsIgnoreLiquidMarkup()
        {
            Template t = Template.Parse("{% literal %}{% if 'gnomeslab' contains 'liquid' %}yes{ % endif %}{% endliteral %}");
            Assert.Equal("{% if 'gnomeslab' contains 'liquid' %}yes{ % endif %}", t.Render());
        }

        [Fact]
        public void TestShorthandSyntax()
        {
            Template t = Template.Parse("{{{{% if 'gnomeslab' contains 'liquid' %}yes{ % endif %}}}}");
            Assert.Equal("{% if 'gnomeslab' contains 'liquid' %}yes{ % endif %}", t.Render());
        }

        [Fact]
        public void TestLiteralsDontRemoveComments()
        {
            Template t = Template.Parse("{{{ {# comment #} }}}");
            Assert.Equal("{# comment #}", t.Render());
        }

        [Fact]
        public void TestFromShorthand()
        {
            Assert.Equal("{% literal %}gnomeslab{% endliteral %}", Literal.FromShortHand("{{{gnomeslab}}}"));
        }

        [Fact]
        public void TestFromShorthandIgnoresImproperSyntax()
        {
            Assert.Equal("{% if 'hi' == 'hi' %}hi{% endif %}", Literal.FromShortHand("{% if 'hi' == 'hi' %}hi{% endif %}"));
        }
    }
}
