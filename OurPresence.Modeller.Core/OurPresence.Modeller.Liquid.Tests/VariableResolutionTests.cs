using FluentAssertions;
using System;
using System.Globalization;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class VariableResolutionTests
    {
        [Fact]
        public void TestSimpleVariable()
        {
            Template template = Template.Parse("{{test}}");
            Assert.Equal("worked", template.Render(Hash.FromAnonymousObject(new { test = "worked" })));
            Assert.Equal("worked wonderfully", template.Render(Hash.FromAnonymousObject(new { test = "worked wonderfully" })));
        }

        [Fact]
        public void TestSimpleWithWhitespaces()
        {
            Template template = Template.Parse("  {{ test }}  ");
            Assert.Equal("  worked  ", template.Render(Hash.FromAnonymousObject(new { test = "worked" })));
            Assert.Equal("  worked wonderfully  ", template.Render(Hash.FromAnonymousObject(new { test = "worked wonderfully" })));
        }

        [Fact]
        public void TestIgnoreUnknown()
        {
            Template template = Template.Parse("{{ test }}");
            Assert.Equal("", template.Render());
        }

        [Fact]
        public void TestHashScoping()
        {
            Template template = Template.Parse("{{ test.test }}");
            Assert.Equal("worked", template.Render(Hash.FromAnonymousObject(new { test = new { test = "worked" } })));
        }

        [Fact]
        public void TestPresetAssigns()
        {
            Template template = Template.Parse("{{ test }}");
            template.Assigns["test"] = "worked";
            Assert.Equal("worked", template.Render());
        }

        [Fact]
        public void TestReuseParsedTemplate()
        {
            Template template = Template.Parse("{{ greeting }} {{ name }}");
            template.Assigns["greeting"] = "Goodbye";
            Assert.Equal("Hello Tobi", template.Render(Hash.FromAnonymousObject(new { greeting = "Hello", name = "Tobi" })));
            Assert.Equal("Hello ", template.Render(Hash.FromAnonymousObject(new { greeting = "Hello", unknown = "Tobi" })));
            Assert.Equal("Hello Brian", template.Render(Hash.FromAnonymousObject(new { greeting = "Hello", name = "Brian" })));
            Assert.Equal("Goodbye Brian", template.Render(Hash.FromAnonymousObject(new { name = "Brian" })));
            template.Assigns.Should().BeEquivalentTo(Hash.FromAnonymousObject(new { greeting = "Goodbye" }));
        }

        [Fact]
        public void TestAssignsNotPollutedFromTemplate()
        {
            Template template = Template.Parse("{{ test }}{% assign test = 'bar' %}{{ test }}");
            template.Assigns["test"] = "baz";
            Assert.Equal("bazbar", template.Render());
            Assert.Equal("bazbar", template.Render());
            Assert.Equal("foobar", template.Render(Hash.FromAnonymousObject(new { test = "foo" })));
            Assert.Equal("bazbar", template.Render());
        }

        [Fact]
        public void TestHashWithDefaultProc()
        {
            Template template = Template.Parse("Hello {{ test }}");
            Hash assigns = new Hash((h, k) => { throw new Exception("Unknown variable '" + k + "'"); });
            assigns["test"] = "Tobi";
            Assert.Equal("Hello Tobi", template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
            assigns.Remove("test");
            Exception ex = Assert.Throws<Exception>(() => template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
            Assert.Equal("Unknown variable 'test'", ex.Message);
        }
    }
}
