// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using FluentAssertions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class FilterTests
    {
        #region Classes used in tests

        private static class MoneyFilter
        {
            public static string Money(object input)
            {
                return string.Format(" {0:d}$ ", input);
            }

            public static string MoneyWithUnderscore(object input)
            {
                return string.Format(" {0:d}$ ", input);
            }
        }

        private static class CanadianMoneyFilter
        {
            public static string Money(object input)
            {
                return string.Format(" {0:d}$ CAD ", input);
            }
        }

        private static class FiltersWithArgumentsInt
        {
            public static string Adjust(int input, int offset = 10)
            {
                return string.Format("[{0:d}]", input + offset);
            }

            public static string AddSub(int input, int plus, int minus = 20)
            {
                return string.Format("[{0:d}]", input + plus - minus);
            }
        }

        private static class FiltersWithArgumentsLong
        {
            public static string Adjust(long input, long offset = 10)
            {
                return string.Format("[{0:d}]", input + offset);
            }

            public static string AddSub(long input, long plus, long minus = 20)
            {
                return string.Format("[{0:d}]", input + plus - minus);
            }
        }

        private static class FiltersWithMulitpleMethodSignatures
        {
            public static string Concat(string one, string two)
            {
                return string.Concat(one, two);
            }

            public static string Concat(string one, string two, string three)
            {
                return string.Concat(one, two, three);
            }
        }

        private static class FiltersWithMultipleMethodSignaturesAndContextParam
        {
            public static string ConcatWithContext(Context context, string one, string two)
            {
                return string.Concat(one, two);
            }

            public static string ConcatWithContext(Context context, string one, string two, string three)
            {
                return string.Concat(one, two, three);
            }
        }

        private static class ContextFilters
        {
            public static string BankStatement(Context context, object input)
            {
                return string.Format(" " + context["name"] + " has {0:d}$ ", input);
            }
        }

        #endregion

        private Context _context= new Context(new Template(), CultureInfo.InvariantCulture);

        /*[Fact]
        public void TestNonExistentFilter()
        {
            _context["var"] = 1000;
            Assert.Throws<FilterNotFoundException>(() => new Variable("var | syzzy").Render(_context));
        }*/

        [Fact]
        public void TestLocalFilter()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(MoneyFilter));
            Assert.Equal(" 1000$ ", new Variable(_context.Template,"var | money").Render(_context));
        }

        [Fact]
        public void TestUnderscoreInFilterName()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(MoneyFilter));
            Assert.Equal(" 1000$ ", new Variable(_context.Template, "var | money_with_underscore").Render(_context));
        }

        [Fact]
        public void TestFilterWithNumericArgument()
        {
            _context["var"] = 1000L;
            _context.AddFilters(typeof(FiltersWithArgumentsInt));
            Assert.Equal("[1005]", new Variable(_context.Template, "var | adjust: 5").Render(_context));
        }

        [Fact]
        public void TestFilterWithNegativeArgument()
        {
            _context["var"] = 1000L;
            _context.AddFilters(typeof(FiltersWithArgumentsInt));
            Assert.Equal("[995]", new Variable(_context.Template, "var | adjust: -5").Render(_context));
        }

        [Fact]
        public void TestFilterWithDefaultArgument()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArgumentsInt));
            Assert.Equal("[1010]", new Variable(_context.Template, "var | adjust").Render(_context));
        }

        [Fact]
        public void TestFilterWithTwoArguments()
        {
            _context["var"] = 1000L;
            _context.AddFilters(typeof(FiltersWithArgumentsInt));
            Assert.Equal("[1150]", new Variable(_context.Template, "var | add_sub: 200, 50").Render(_context));
        }

        [Fact]
        public void TestFilterWithNumericArgumentLong()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArgumentsLong));
            Assert.Equal("[1005]", new Variable(_context.Template, "var | adjust: 5").Render(_context));
        }

        [Fact]
        public void TestFilterWithNegativeArgumentLong()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArgumentsLong));
            Assert.Equal("[995]", new Variable(_context.Template, "var | adjust: -5").Render(_context));
        }

        [Fact]
        public void TestFilterWithDefaultArgumentLong()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArgumentsLong));
            Assert.Equal("[1010]", new Variable(_context.Template, "var | adjust").Render(_context));
        }

        [Fact]
        public void TestFilterWithTwoArgumentsLong()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArgumentsLong));
            Assert.Equal("[1150]", new Variable(_context.Template, "var | add_sub: 200, 50").Render(_context));
        }

        [Fact]
        public void TestFilterWithMultipleMethodSignatures()
        {
            Template.RegisterFilter(typeof(FiltersWithMulitpleMethodSignatures));

            Assert.Equal("AB", Template.Parse("{{'A' | concat : 'B'}}").Render());
            Assert.Equal("ABC", Template.Parse("{{'A' | concat : 'B', 'C'}}").Render());
        }

        [Fact]
        public void TestFilterWithMultipleMethodSignaturesAndContextParam()
        {
            Template.RegisterFilter(typeof(FiltersWithMultipleMethodSignaturesAndContextParam));

            Assert.Equal("AB", Template.Parse("{{'A' | concat_with_context : 'B'}}").Render());
            Assert.Equal("ABC", Template.Parse("{{'A' | concat_with_context : 'B', 'C'}}").Render());
        }

        /*/// <summary>
        /// ATM the trailing value is silently ignored. Should raise an exception?
        /// </summary>
        [Fact]
        public void TestFilterWithTwoArgumentsNoComma()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(FiltersWithArguments));
            Assert.Equal("[1150]", string.Join(string.Empty, new Variable("var | add_sub: 200 50").Render(_context));
        }*/

        [Fact]
        public void TestSecondFilterOverwritesFirst()
        {
            _context["var"] = 1000;
            _context.AddFilters(typeof(MoneyFilter));
            _context.AddFilters(typeof(CanadianMoneyFilter));
            Assert.Equal(" 1000$ CAD ", new Variable(_context.Template, "var | money").Render(_context));
        }

        [Fact]
        public void TestSize()
        {
            _context["var"] = "abcd";
            _context.AddFilters(typeof(MoneyFilter));
            Assert.Equal(4, new Variable(_context.Template, "var | size").Render(_context));
        }

        [Fact]
        public void TestJoin()
        {
            _context["var"] = new[] { 1, 2, 3, 4 };
            Assert.Equal("1 2 3 4", new Variable(_context.Template, "var | join").Render(_context));
        }

        [Fact]
        public void TestSort()
        {
            _context["value"] = 3;
            _context["numbers"] = new[] { 2, 1, 4, 3 };
            _context["words"] = new[] { "expected", "as", "alphabetic" };
            _context["arrays"] = new[] { new[] { "flattened" }, new[] { "are" } };

            new Variable(_context.Template, "numbers | sort").Render(_context).Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });

            new Variable(_context.Template, "words | sort").Render(_context).Should().BeEquivalentTo(new[] { "alphabetic", "as", "expected" });
            new Variable(_context.Template, "value | sort").Render(_context).Should().BeEquivalentTo(new[] { 3 });
            new Variable(_context.Template, "arrays | sort").Render(_context).Should().BeEquivalentTo(new[] { "are", "flattened" });
        }

        [Fact]
        public void TestSplit()
        {
            _context["var"] = "a~b";
            Assert.Equal(new[] { "a", "b" }, new Variable(_context.Template, "var | split:'~'").Render(_context));
        }

        [Fact]
        public void TestStripHtml()
        {
            _context["var"] = "<b>bla blub</a>";
            Assert.Equal("bla blub", new Variable(_context.Template, "var | strip_html").Render(_context));
        }

        [Fact]
        public void Capitalize()
        {
            _context["var"] = "blub";
            Assert.Equal("Blub", new Variable(_context.Template, "var | capitalize").Render(_context));
        }

        [Fact]
        public void Slice()
        {
            _context["var"] = "blub";
            Assert.Equal("b", new Variable(_context.Template, "var | slice: 0, 1").Render(_context));
            Assert.Equal("bl", new Variable(_context.Template, "var | slice: 0, 2").Render(_context));
            Assert.Equal("l", new Variable(_context.Template, "var | slice: 1").Render(_context));
            Assert.Equal("", new Variable(_context.Template, "var | slice: 4, 1").Render(_context));
            Assert.Equal("ub", new Variable(_context.Template, "var | slice: -2, 2").Render(_context));
            Assert.Equal(null, new Variable(_context.Template, "var | slice: 5, 1").Render(_context));
        }

        [Fact]
        public void TestLocalGlobal()
        {
            Template.RegisterFilter(typeof(MoneyFilter));

            Assert.Equal(" 1000$ ", Template.Parse("{{1000 | money}}").Render());
            Assert.Equal(" 1000$ CAD ", Template.Parse("{{1000 | money}}").Render(new RenderParameters(CultureInfo.InvariantCulture) { Filters = new[] { typeof(CanadianMoneyFilter) } }));
            Assert.Equal(" 1000$ CAD ", Template.Parse("{{1000 | money}}").Render(new RenderParameters(CultureInfo.InvariantCulture) { Filters = new[] { typeof(CanadianMoneyFilter) } }));
        }

        [Fact]
        public void TestContextFilter()
        {
            _context["var"] = 1000;
            _context["name"] = "King Kong";
            _context.AddFilters(typeof(ContextFilters));
            Assert.Equal(" King Kong has 1000$ ", new Variable(_context.Template, "var | bank_statement").Render(_context));
        }
    }
}
