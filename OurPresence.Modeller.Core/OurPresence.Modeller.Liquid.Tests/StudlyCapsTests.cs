// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class StudlyCapsTests
    {
        [Fact]
        public void TestSimpleVariablesStudlyCaps()
        {
            var template = "{{ Greeting }} {{ Name }}";
            Helper.AssertTemplateResult(
                expected: "Hello Tobi",
                template: template,
                anonymousObject: new { greeting = "Hello", name = "Tobi" });

            Helper.AssertTemplateResult(
                expected: "Hello Tobi",
                template: template,
                anonymousObject: new { Greeting = "Hello", Name = "Tobi" });
            Helper.AssertTemplateResult(
                expected: " ",
                template: template,
                anonymousObject: new { greeting = "Hello", name = "Tobi" });
        }

        [Fact]
        public void TestFiltersStudlyCapsAreNotAllowed()
        {
            Helper.AssertTemplateResult(
                expected:"HI TOBI",
                template: "{{ 'hi tobi' | upcase }}");

            Helper.AssertTemplateResult(
                expected: "HI TOBI",
                template: "{{ 'hi tobi' | Upcase }}");
        }

        [Fact]
        public void TestAssignsStudlyCaps()
        {
            Helper.AssertTemplateResult(
                expected: ".foo.",
                template: "{% assign Foo = values %}.{{ Foo[0] }}.",
                anonymousObject: new { values = new[] { "foo", "bar", "baz" } });
            Helper.AssertTemplateResult(
                expected: ".bar.",
                template: "{% assign fOo = values %}.{{ fOo[1] }}.",
                anonymousObject: new { values = new[] { "foo", "bar", "baz" } });
        }
    }
}
