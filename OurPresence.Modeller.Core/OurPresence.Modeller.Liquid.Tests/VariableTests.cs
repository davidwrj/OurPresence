// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class VariableTests
    {
        [Fact]
        public void TestVariable()
        {
            var template = new Template();
            var var = new Variable(template, "hello");
            Assert.Equal("hello", var.Name);
        }

        [Fact]
        public void TestFilters()
        {
            var template = new Template();
            var var = new Variable(template, "hello | textileze");
            Assert.Equal("hello", var.Name);
            Assert.Single(var.Filters);
            Assert.Equal("textileze", var.Filters[0].Name);
            Assert.Empty(var.Filters[0].Arguments);

            var = new Variable(template, "hello | textileze | paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);

            var = new Variable(template, " hello | strftime: '%Y'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("strftime", new[] { "'%Y'" }) }, var.Filters);

            var = new Variable(template, " 'typo' | link_to: 'Typo', true ");
            Assert.Equal("'typo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("link_to", new[] { "'Typo'", "true" }) }, var.Filters);

            var = new Variable(template, " 'typo' | link_to: 'Typo', false ");
            Assert.Equal("'typo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("link_to", new[] { "'Typo'", "false" }) }, var.Filters);

            var = new Variable(template, " 'foo' | repeat: 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3" }) }, var.Filters);

            var = new Variable(template, " 'foo' | repeat: 3, 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3", "3" }) }, var.Filters);

            var = new Variable(template, " 'foo' | repeat: 3, 3, 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3", "3", "3" }) }, var.Filters);

            var = new Variable(template, " hello | strftime: '%Y, okay?'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("strftime", new[] { "'%Y, okay?'" }) }, var.Filters);

            var = new Variable(template, " hello | things: \"%Y, okay?\", 'the other one'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("things", new[] { "\"%Y, okay?\"", "'the other one'" }) }, var.Filters);
        }

        [Fact]
        public void TestFilterWithDateParameter()
        {
            var template = new Template();
            var var = new Variable(template, " '2006-06-06' | date: \"%m/%d/%Y\"");
            Assert.Equal("'2006-06-06'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("date", new[] { "\"%m/%d/%Y\"" }) }, var.Filters);
        }

        [Fact]
        public void TestFiltersWithoutWhitespace()
        {
            var template = new Template();
            var var = new Variable(template, "hello | textileze | paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);

            var = new Variable(template,"hello|textileze|paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);
        }

        [Fact]
        public void TestSymbol()
        {
            var template = new Template();
            var var = new Variable(template, "http://disney.com/logo.gif | image: 'med' ");
            Assert.Equal("http://disney.com/logo.gif", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("image", new[] { "'med'" }) }, var.Filters);
        }

        [Fact]
        public void TestStringSingleQuoted()
        {
            var template = new Template();
            var var = new Variable(template, " 'hello' ");
            Assert.Equal("'hello'", var.Name);
        }

        [Fact]
        public void TestStringDoubleQuoted()
        {
            var template = new Template();
            var var = new Variable(template, " \"hello\" ");
            Assert.Equal("\"hello\"", var.Name);
        }

        [Fact]
        public void TestInteger()
        {
            var template = new Template();
            var var = new Variable(template, " 1000 ");
            Assert.Equal("1000", var.Name);
        }

        [Fact]
        public void TestFloat()
        {
            var template = new Template();
            var var = new Variable(template, " 1000.01 ");
            Assert.Equal("1000.01", var.Name);
        }

        [Fact]
        public void TestStringWithSpecialChars()
        {
            var template = new Template();
            var var = new Variable(template, " 'hello! $!@.;\"ddasd\" ' ");
            Assert.Equal("'hello! $!@.;\"ddasd\" '", var.Name);
        }

        [Fact]
        public void TestStringDot()
        {
            var template = new Template();
            var var = new Variable(template, " test.test ");
            Assert.Equal("test.test", var.Name);
        }

        private static void AssertFiltersAreEqual(Variable.Filter[] expected, System.Collections.Generic.List<Variable.Filter> actual)
        {
            Assert.Equal(expected.Length, actual.Count);
            for (var i = 0; i < expected.Length; ++i)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Arguments.Length, actual[i].Arguments.Length);
                for (var j = 0; j < expected[i].Arguments.Length; ++j)
                    Assert.Equal(expected[i].Arguments[j], actual[i].Arguments[j]);
            }
        }
    }
}
