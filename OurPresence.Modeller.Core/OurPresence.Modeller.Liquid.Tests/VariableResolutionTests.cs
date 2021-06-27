// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            var template = Template.Parse("{{test}}");
            Assert.Equal("worked", template.Render(Hash.FromAnonymousObject(new { test = "worked" })));
            Assert.Equal("worked wonderfully", template.Render(Hash.FromAnonymousObject(new { test = "worked wonderfully" })));
        }

        [Fact]
        public void TestSimpleWithWhitespaces()
        {
            var template = Template.Parse("  {{ test }}  ");
            Assert.Equal("  worked  ", template.Render(Hash.FromAnonymousObject(new { test = "worked" })));
            Assert.Equal("  worked wonderfully  ", template.Render(Hash.FromAnonymousObject(new { test = "worked wonderfully" })));
        }

        [Fact]
        public void TestIgnoreUnknown()
        {
            var template = Template.Parse("{{ test }}");
            var result = template.Render();
            Assert.Equal("", result);
        }

        [Fact]
        public void TestHashScoping()
        {
            var template = Template.Parse("{{ test.test }}");
            Assert.Equal("worked", template.Render(Hash.FromAnonymousObject(new { test = new { test = "worked" } })));
        }

        [Fact]
        public void TestPresetAssigns()
        {
            var template = Template.Parse("{{ test }}");
            template.Assigns["test"] = "worked";
            var result = template.Render();
            Assert.Equal("worked", result);
        }

        [Fact]
        public void TestReuseParsedTemplate()
        {
            var template = Template.Parse("{{ greeting }} {{ name }}");
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
            var template = Template.Parse("{{ test }}{% assign test = 'bar' %}{{ test }}");
            template.Assigns["test"] = "baz";

            Assert.Equal("bazbar", template.Render());
            Assert.Equal("bazbar", template.Render());
            Assert.Equal("foobar", template.Render(Hash.FromAnonymousObject(new { test = "foo" })));
            Assert.Equal("bazbar", template.Render());
        }

        [Fact]
        public void TestHashWithDefaultProc()
        {
            var template = Template.Parse("Hello {{ test }}");
            var assigns = new Hash((h, k) => { throw new Exception("Unknown variable '" + k + "'"); });
            assigns["test"] = "Tobi";
            Assert.Equal("Hello Tobi", template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
            assigns.Remove("test");
            var ex = Assert.Throws<Exception>(() => template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
            Assert.Equal("Unknown variable 'test'", ex.Message);
        }
    }
}
