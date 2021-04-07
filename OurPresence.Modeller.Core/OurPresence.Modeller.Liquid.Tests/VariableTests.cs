using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class VariableTests
    {
        [Fact]
        public void TestVariable()
        {
            Variable var = new Variable("hello");
            Assert.Equal("hello", var.Name);
        }

        [Fact]
        public void TestFilters()
        {
            Variable var = new Variable("hello | textileze");
            Assert.Equal("hello", var.Name);
            Assert.Equal(1, var.Filters.Count);
            Assert.Equal("textileze", var.Filters[0].Name);
            Assert.Equal(0, var.Filters[0].Arguments.Length);

            var = new Variable("hello | textileze | paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);

            var = new Variable(" hello | strftime: '%Y'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("strftime", new[] { "'%Y'" }) }, var.Filters);

            var = new Variable(" 'typo' | link_to: 'Typo', true ");
            Assert.Equal("'typo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("link_to", new[] { "'Typo'", "true" }) }, var.Filters);

            var = new Variable(" 'typo' | link_to: 'Typo', false ");
            Assert.Equal("'typo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("link_to", new[] { "'Typo'", "false" }) }, var.Filters);

            var = new Variable(" 'foo' | repeat: 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3" }) }, var.Filters);

            var = new Variable(" 'foo' | repeat: 3, 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3", "3" }) }, var.Filters);

            var = new Variable(" 'foo' | repeat: 3, 3, 3 ");
            Assert.Equal("'foo'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("repeat", new[] { "3", "3", "3" }) }, var.Filters);

            var = new Variable(" hello | strftime: '%Y, okay?'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("strftime", new[] { "'%Y, okay?'" }) }, var.Filters);

            var = new Variable(" hello | things: \"%Y, okay?\", 'the other one'");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("things", new[] { "\"%Y, okay?\"", "'the other one'" }) }, var.Filters);
        }

        [Fact]
        public void TestFilterWithDateParameter()
        {
            Variable var = new Variable(" '2006-06-06' | date: \"%m/%d/%Y\"");
            Assert.Equal("'2006-06-06'", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("date", new[] { "\"%m/%d/%Y\"" }) }, var.Filters);
        }

        [Fact]
        public void TestFiltersWithoutWhitespace()
        {
            Variable var = new Variable("hello | textileze | paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);

            var = new Variable("hello|textileze|paragraph");
            Assert.Equal("hello", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("textileze", new string[] { }), new Variable.Filter("paragraph", new string[] { }) }, var.Filters);
        }

        [Fact]
        public void TestSymbol()
        {
            Variable var = new Variable("http://disney.com/logo.gif | image: 'med' ");
            Assert.Equal("http://disney.com/logo.gif", var.Name);
            AssertFiltersAreEqual(new[] { new Variable.Filter("image", new[] { "'med'" }) }, var.Filters);
        }

        [Fact]
        public void TestStringSingleQuoted()
        {
            Variable var = new Variable(" 'hello' ");
            Assert.Equal("'hello'", var.Name);
        }

        [Fact]
        public void TestStringDoubleQuoted()
        {
            Variable var = new Variable(" \"hello\" ");
            Assert.Equal("\"hello\"", var.Name);
        }

        [Fact]
        public void TestInteger()
        {
            Variable var = new Variable(" 1000 ");
            Assert.Equal("1000", var.Name);
        }

        [Fact]
        public void TestFloat()
        {
            Variable var = new Variable(" 1000.01 ");
            Assert.Equal("1000.01", var.Name);
        }

        [Fact]
        public void TestStringWithSpecialChars()
        {
            Variable var = new Variable(" 'hello! $!@.;\"ddasd\" ' ");
            Assert.Equal("'hello! $!@.;\"ddasd\" '", var.Name);
        }

        [Fact]
        public void TestStringDot()
        {
            Variable var = new Variable(" test.test ");
            Assert.Equal("test.test", var.Name);
        }

        private static void AssertFiltersAreEqual(Variable.Filter[] expected, System.Collections.Generic.List<Variable.Filter> actual)
        {
            Assert.Equal(expected.Length, actual.Count);
            for (int i = 0; i < expected.Length; ++i)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Arguments.Length, actual[i].Arguments.Length);
                for (int j = 0; j < expected[i].Arguments.Length; ++j)
                    Assert.Equal(expected[i].Arguments[j], actual[i].Arguments[j]);
            }
        }
    }
}
