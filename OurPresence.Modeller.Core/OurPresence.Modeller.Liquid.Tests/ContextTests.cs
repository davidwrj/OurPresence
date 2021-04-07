using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Exceptions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class ContextTests
    {
        #region Classes used in tests

        private static class TestFilters
        {
            public static string Hi(string output)
            {
                return output + " hi!";
            }
        }

        private static class TestContextFilters
        {
            public static string Hi(Context context, string output)
            {
                return output + " hi from " + context["name"] + "!";
            }
        }

        private static class GlobalFilters
        {
            public static string Notice(string output)
            {
                return "Global " + output;
            }
        }

        private static class LocalFilters
        {
            public static string Notice(string output)
            {
                return "Local " + output;
            }
        }

        private class HundredCents : ILiquidizable
        {
            public object ToLiquid()
            {
                return 100;
            }
        }

        private class CentsDrop : Drop
        {
            public object Amount
            {
                get { return new HundredCents(); }
            }

            public bool NonZero
            {
                get { return true; }
            }
        }

        private class ContextSensitiveDrop : Drop
        {
            public object Test()
            {
                return Context["test"];
            }
        }

        private class Category : Drop
        {
            public string Name { get; set; }

            public Category(string name)
            {
                Name = name;
            }

            public override object ToLiquid()
            {
                return new CategoryDrop(this);
            }
        }

        private class CategoryDrop : IContextAware
        {
            public Category Category { get; set; }
            public Context Context { get; set; }

            public CategoryDrop(Category category)
            {
                Category = category;
            }
        }

        private class CounterDrop : Drop
        {
            private int _count;

            public int Count()
            {
                return ++_count;
            }
        }

        private class ArrayLike : ILiquidizable
        {
            private readonly Dictionary<int, int> _counts = new Dictionary<int, int>();

            public object Fetch(int index)
            {
                return null;
            }

            public object this[int index]
            {
                get
                {
                    _counts[index] += 1;
                    return _counts[index];
                }
            }

            public object ToLiquid()
            {
                return this;
            }
        }

        private class IndexableLiquidizable : IIndexable, ILiquidizable
        {
            private const string theKey = "thekey";

            public object this[object key] => key as string == theKey ? new LiquidizableList() : null;

            public bool ContainsKey(object key)
            {
                return key as string == theKey;
            }

            public object ToLiquid()
            {
                return this;
            }
        }

        private class LiquidizableList : ILiquidizable
        {
            public object ToLiquid()
            {
                return new List<string>(new[] { "text1", "text2" });
            }
        }

        #endregion

        private readonly Context _context= new Context(CultureInfo.InvariantCulture);

        [Fact]
        public void TestVariables()
        {
            _context["string"] = "string";
            Assert.Equal("string", _context["string"]);

            _context["EscapedCharacter"] = "EscapedCharacter\"";
            Assert.Equal("EscapedCharacter\"", _context["EscapedCharacter"]);

            _context["num"] = 5;
            Assert.Equal(5, _context["num"]);

            _context["decimal"] = 5m;
            Assert.Equal(5m, _context["decimal"]);

            _context["float"] = 5.0f;
            Assert.Equal(5.0f, _context["float"]);

            _context["double"] = 5.0;
            Assert.Equal(5.0, _context["double"]);

            _context["time"] = TimeSpan.FromDays(1);
            Assert.Equal(TimeSpan.FromDays(1), _context["time"]);

            _context["date"] = DateTime.Today;
            Assert.Equal(DateTime.Today, _context["date"]);

            DateTime now = DateTime.Now;
            _context["datetime"] = now;
            Assert.Equal(now, _context["datetime"]);

            DateTimeOffset offset = new DateTimeOffset(2013, 9, 10, 0, 10, 32, new TimeSpan(1, 0, 0));
            _context["datetimeoffset"] = offset;
            Assert.Equal(offset, _context["datetimeoffset"]);

            Guid guid = Guid.NewGuid();
            _context["guid"] = guid;
            Assert.Equal(guid, _context["guid"]);

            _context["bool"] = true;
            Assert.Equal(true, _context["bool"]);

            _context["bool"] = false;
            Assert.Equal(false, _context["bool"]);

            _context["nil"] = null;
            Assert.Null(_context["nil"]);
            Assert.Null(_context["nil"]);
        }

        [Fact]
        public void TestVariablesNotExisting()
        {
            Assert.Null(_context["does_not_exist"]);
        }

        [Fact]
        public void TestVariableNotFoundErrors()
        {
            Template template = Template.Parse("{{ does_not_exist }}");
            string rendered = template.Render();
 
            Assert.Equal("", rendered);
            template.Errors.Should().HaveCount(1);
            Assert.Equal(string.Format(Liquid.ResourceManager.GetString("VariableNotFoundException"), "does_not_exist"), template.Errors[0].Message);
        }
 
        [Fact]
        public void TestVariableNotFoundFromAnonymousObject()
        {
            Template template = Template.Parse("{{ first.test }}{{ second.test }}");
            string rendered = template.Render(Hash.FromAnonymousObject(new { second = new { foo = "hi!" } }));
 
            Assert.Equal("", rendered);
            Assert.Equal(2, template.Errors.Count);
            Assert.Equal(string.Format(Liquid.ResourceManager.GetString("VariableNotFoundException"), "first.test"), template.Errors[0].Message);
            Assert.Equal(string.Format(Liquid.ResourceManager.GetString("VariableNotFoundException"), "second.test"), template.Errors[1].Message);
        }
 
        [Fact]
        public void TestVariableNotFoundException()
        {
            Action action = () => Template.Parse("{{ does_not_exist }}").Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            });
            action.Should().NotThrow();
        }

        [Fact]
        public void TestVariableNotFoundExceptionIgnoredForIfStatement()
        {
            Template template = Template.Parse("{% if does_not_exist %}abc{% endif %}");
            string rendered = template.Render();

            Assert.Equal("", rendered);
            template.Errors.Should().BeEmpty();
        }

        [Fact]
        public void TestVariableNotFoundExceptionIgnoredForUnlessStatement()
        {
            Template template = Template.Parse("{% unless does_not_exist %}abc{% endunless %}");
            string rendered = template.Render();

            Assert.Equal("abc", rendered);
            template.Errors.Should().BeEmpty();
        }

        [Fact]
        public void TestScoping()
        {

            Action action = () =>
            {
                _context.Push(null);
                _context.Pop();
            };
            action.Should().NotThrow();

            Assert.Throws<ContextException>(() => _context.Pop());

            Assert.Throws<ContextException>(() =>
            {
                _context.Push(null);
                _context.Pop();
                _context.Pop();
            });
        }

        [Fact]
        public void TestLengthQuery()
        {
            _context["numbers"] = new[] { 1, 2, 3, 4 };
            Assert.Equal(4, _context["numbers.size"]);

            _context["numbers"] = new Dictionary<int, int>
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 },
                { 4, 4 }
            };
            Assert.Equal(4, _context["numbers.size"]);

            _context["numbers"] = new Dictionary<object, int>
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 },
                { 4, 4 },
                { "size", 1000 }
            };
            Assert.Equal(1000, _context["numbers.size"]);
        }

        [Fact]
        public void TestHyphenatedVariable()
        {
            _context["oh-my"] = "godz";
            Assert.Equal("godz", _context["oh-my"]);
        }

        [Fact]
        public void TestAddFilter()
        {
            Context context = new Context(CultureInfo.InvariantCulture);
            context.AddFilters(new[] { typeof(TestFilters) });
            Assert.Equal("hi? hi!", context.Invoke("hi", new List<object> { "hi?" }));

            context = new Context(CultureInfo.InvariantCulture);
            Assert.Equal("hi?", context.Invoke("hi", new List<object> { "hi?" }));

            context.AddFilters(new[] { typeof(TestFilters) });
            Assert.Equal("hi? hi!", context.Invoke("hi", new List<object> { "hi?" }));
        }

        [Fact]
        public void TestAddContextFilter()
        {
            Context context = new Context(CultureInfo.InvariantCulture);
            context["name"] = "King Kong";

            context.AddFilters(new[] { typeof(TestContextFilters) });
            Assert.Equal("hi? hi from King Kong!", context.Invoke("hi", new List<object> { "hi?" }));

            context = new Context(CultureInfo.InvariantCulture);
            Assert.Equal("hi?", context.Invoke("hi", new List<object> { "hi?" }));
        }

        [Fact]
        public void TestOverrideGlobalFilter()
        {
            Template.RegisterFilter(typeof(GlobalFilters));
            Assert.Equal("Global test", Template.Parse("{{'test' | notice }}").Render());
            Assert.Equal("Local test", Template.Parse("{{'test' | notice }}").Render(new RenderParameters(CultureInfo.InvariantCulture) { Filters = new[] { typeof(LocalFilters) } }));
        }

        [Fact]
        public void TestOnlyIntendedFiltersMakeItThere()
        {
            Context context = new Context(CultureInfo.InvariantCulture);
            var methodsBefore = context.Strainer.Methods.Select(mi => mi.Name).ToList();
            context.AddFilters(new[] { typeof(TestFilters) });
            var methodsAfter = context.Strainer.Methods.Select(mi => mi.Name).ToList();

            methodsBefore.Concat(new[] { "Hi" }).OrderBy(s => s).Should().BeEquivalentTo(methodsAfter.OrderBy(s => s));
        }

        [Fact]
        public void TestAddItemInOuterScope()
        {
            _context["test"] = "test";
            _context.Push(new Hash());
            Assert.Equal("test", _context["test"]);
            _context.Pop();
            Assert.Equal("test", _context["test"]);
        }

        [Fact]
        public void TestAddItemInInnerScope()
        {
            _context.Push(new Hash());
            _context["test"] = "test";
            Assert.Equal("test", _context["test"]);
            _context.Pop();
            Assert.Null(_context["test"]);
        }

        [Fact]
        public void TestHierarchicalData()
        {
            _context["hash"] = new { name = "tobi" };
            Assert.Equal("tobi", _context["hash.name"]);
            Assert.Equal("tobi", _context["hash['name']"]);
        }

        [Fact]
        public void TestKeywords()
        {
            Assert.Equal(true, _context["true"]);
            Assert.Equal(false, _context["false"]);
        }

        [Fact]
        public void TestDigits()
        {
            Assert.Equal(100, _context["100"]);
            Assert.Equal(100.00, _context[string.Format("100{0}00", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)]);
        }

        [Fact]
        public void TestStrings()
        {
            Assert.Equal("hello!", _context["'hello!'"]);
            Assert.Equal("hello!", _context["'hello!'"]);
        }

        [Fact]
        public void TestMerge()
        {
            _context.Merge(new Hash { { "test", "test" } });
            Assert.Equal("test", _context["test"]);
            _context.Merge(new Hash { { "test", "newvalue" }, { "foo", "bar" } });
            Assert.Equal("newvalue", _context["test"]);
            Assert.Equal("bar", _context["foo"]);
        }

        [Fact]
        public void TestArrayNotation()
        {
            _context["test"] = new[] { 1, 2, 3, 4, 5 };

            Assert.Equal(1, _context["test[0]"]);
            Assert.Equal(2, _context["test[1]"]);
            Assert.Equal(3, _context["test[2]"]);
            Assert.Equal(4, _context["test[3]"]);
            Assert.Equal(5, _context["test[4]"]);
        }

        [Fact]
        public void TestRecursiveArrayNotation()
        {
            _context["test"] = new { test = new[] { 1, 2, 3, 4, 5 } };

            Assert.Equal(1, _context["test.test[0]"]);

            _context["test"] = new[] { new { test = "worked" } };

            Assert.Equal("worked", _context["test[0].test"]);
        }

        [Fact]
        public void TestHashToArrayTransition()
        {
            _context["colors"] = new
            {
                Blue = new[] { "003366", "336699", "6699CC", "99CCFF" },
                Green = new[] { "003300", "336633", "669966", "99CC99" },
                Yellow = new[] { "CC9900", "FFCC00", "FFFF99", "FFFFCC" },
                Red = new[] { "660000", "993333", "CC6666", "FF9999" }
            };

            Assert.Equal("003366", _context["colors.Blue[0]"]);
            Assert.Equal("FF9999", _context["colors.Red[3]"]);
        }

        [Fact]
        public void TestTryFirst()
        {
            _context["test"] = new[] { 1, 2, 3, 4, 5 };

            Assert.Equal(1, _context["test.first"]);
            Assert.Equal(5, _context["test.last"]);

            _context["test"] = new { test = new[] { 1, 2, 3, 4, 5 } };

            Assert.Equal(1, _context["test.test.first"]);
            Assert.Equal(5, _context["test.test.last"]);

            _context["test"] = new[] { 1 };

            Assert.Equal(1, _context["test.first"]);
            Assert.Equal(1, _context["test.last"]);
        }

        [Fact]
        public void TestAccessHashesWithHashNotation()
        {
            _context["products"] = new { count = 5, tags = new[] { "deepsnow", "freestyle" } };
            _context["product"] = new { variants = new[] { new { title = "draft151cm" }, new { title = "element151cm" } } };

            Assert.Equal(5, _context["products[\"count\"]"]);
            Assert.Equal("deepsnow", _context["products['tags'][0]"]);
            Assert.Equal("deepsnow", _context["products['tags'].first"]);
            Assert.Equal("draft151cm", _context["product['variants'][0][\"title\"]"]);
            Assert.Equal("element151cm", _context["product['variants'][1]['title']"]);
            Assert.Equal("draft151cm", _context["product['variants'][0]['title']"]);
            Assert.Equal("element151cm", _context["product['variants'].last['title']"]);
        }

        [Fact]
        public void TestAccessVariableWithHashNotation()
        {
            _context["foo"] = "baz";
            _context["bar"] = "foo";

            Assert.Equal("baz", _context["[\"foo\"]"]);
            Assert.Equal("baz", _context["[bar]"]);
        }

        [Fact]
        public void TestAccessHashesWithHashAccessVariables()
        {
            _context["var"] = "tags";
            _context["nested"] = new { var = "tags" };
            _context["products"] = new { count = 5, tags = new[] { "deepsnow", "freestyle" } };

            Assert.Equal("deepsnow", _context["products[var].first"]);
            Assert.Equal("freestyle", _context["products[nested.var].last"]);
        }

        [Fact]
        public void TestHashNotationOnlyForHashAccess()
        {
            _context["array"] = new[] { 1, 2, 3, 4, 5 };
            _context["hash"] = new { first = "Hello" };

            Assert.Equal(1, _context["array.first"]);
            Assert.Null(_context["array['first']"]);
            Assert.Equal("Hello", _context["hash['first']"]);
        }

        [Fact]
        public void TestFirstCanAppearInMiddleOfCallchain()
        {
            _context["product"] = new { variants = new[] { new { title = "draft151cm" }, new { title = "element151cm" } } };

            Assert.Equal("draft151cm", _context["product.variants[0].title"]);
            Assert.Equal("element151cm", _context["product.variants[1].title"]);
            Assert.Equal("draft151cm", _context["product.variants.first.title"]);
            Assert.Equal("element151cm", _context["product.variants.last.title"]);
        }

        [Fact]
        public void TestCents()
        {
            _context.Merge(Hash.FromAnonymousObject(new { cents = new HundredCents() }));
            Assert.Equal(100, _context["cents"]);
        }

        [Fact]
        public void TestNestedCents()
        {
            _context.Merge(Hash.FromAnonymousObject(new { cents = new { amount = new HundredCents() } }));
            Assert.Equal(100, _context["cents.amount"]);

            _context.Merge(Hash.FromAnonymousObject(new { cents = new { cents = new { amount = new HundredCents() } } }));
            Assert.Equal(100, _context["cents.cents.amount"]);
        }

        [Fact]
        public void TestCentsThroughDrop()
        {
            _context.Merge(Hash.FromAnonymousObject(new { cents = new CentsDrop() }));
            Assert.Equal(100, _context["cents.amount"]);
        }

        [Fact]
        public void TestNestedCentsThroughDrop()
        {
            _context.Merge(Hash.FromAnonymousObject(new { vars = new { cents = new CentsDrop() } }));
            Assert.Equal(100, _context["vars.cents.amount"]);
        }

        [Fact]
        public void TestDropMethodsWithQuestionMarks()
        {
            _context.Merge(Hash.FromAnonymousObject(new { cents = new CentsDrop() }));
            Assert.Equal(true, _context["cents.non_zero"]);
        }

        [Fact]
        public void TestContextFromWithinDrop()
        {
            _context.Merge(Hash.FromAnonymousObject(new { test = "123", vars = new ContextSensitiveDrop() }));
            Assert.Equal("123", _context["vars.test"]);
        }

        [Fact]
        public void TestNestedContextFromWithinDrop()
        {
            _context.Merge(Hash.FromAnonymousObject(new { test = "123", vars = new { local = new ContextSensitiveDrop() } }));
            Assert.Equal("123", _context["vars.local.test"]);
        }

        [Fact]
        public void TestRanges()
        {
            _context.Merge(Hash.FromAnonymousObject(new { test = 5 }));

            _context["(1..5)"].Should().BeEquivalentTo(Enumerable.Range(1, 5));
            _context["(1..test)"].Should().BeEquivalentTo(Enumerable.Range(1, 5));
            _context["(test..test)"].Should().BeEquivalentTo(Enumerable.Range(5, 1));
        }

        [Fact]
        public void TestCentsThroughDropNestedly()
        {
            _context.Merge(Hash.FromAnonymousObject(new { cents = new { cents = new CentsDrop() } }));
            Assert.Equal(100, _context["cents.cents.amount"]);

            _context.Merge(Hash.FromAnonymousObject(new { cents = new { cents = new { cents = new CentsDrop() } } }));
            Assert.Equal(100, _context["cents.cents.cents.amount"]);
        }

        [Fact]
        public void TestDropWithVariableCalledOnlyOnce()
        {
            _context["counter"] = new CounterDrop();

            Assert.Equal(1, _context["counter.count"]);
            Assert.Equal(2, _context["counter.count"]);
            Assert.Equal(3, _context["counter.count"]);
        }

        [Fact]
        public void TestDropWithKeyOnlyCalledOnce()
        {
            _context["counter"] = new CounterDrop();

            Assert.Equal(1, _context["counter['count']"]);
            Assert.Equal(2, _context["counter['count']"]);
            Assert.Equal(3, _context["counter['count']"]);
        }

        [Fact]
        public void TestSimpleVariablesRendering()
        {
            Helper.AssertTemplateResult(
                expected: "string",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = "string" }));

            Helper.AssertTemplateResult(
                expected: "EscapedCharacter\"",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = "EscapedCharacter\"" }));

            Helper.AssertTemplateResult(
                expected: "5",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = 5 }));

            Helper.AssertTemplateResult(
                expected: "5",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = 5m }));

            Helper.AssertTemplateResult(
                expected: "5",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = 5.0f }));

            Helper.AssertTemplateResult(
                expected: "5",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = 5.0 }));

            Helper.AssertTemplateResult(
                expected: "1.00:00:00",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = TimeSpan.FromDays(1) }));

            Helper.AssertTemplateResult(
                expected: "1/1/0001 12:00:00 AM",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = DateTime.MinValue }));

            Helper.AssertTemplateResult(
                expected: "9/10/2013 12:10:32 AM +01:00",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = new DateTimeOffset(2013, 9, 10, 0, 10, 32, new TimeSpan(1, 0, 0)) }));

            Helper.AssertTemplateResult(
                expected: "d0f28a51-9393-4658-af0b-8c4b4c5c31ff",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = new Guid("{D0F28A51-9393-4658-AF0B-8C4B4C5C31FF}") }));

            Helper.AssertTemplateResult(
                expected: "true",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = true }));

            Helper.AssertTemplateResult(
                expected: "false",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = false }));

            Helper.AssertTemplateResult(
                expected: "",
                template: "{{context}}",
                localVariables: Hash.FromAnonymousObject(new { context = null as string }));
        }

        [Fact]
        public void TestListRendering()
        {
            Assert.Equal(
                expected: "text1text2",
                actual: Template
                    .Parse("{{context}}")
                    .Render(Hash.FromAnonymousObject(new { context = new LiquidizableList() })));
        }

        [Fact]
        public void TestWrappedListRendering()
        {
            Assert.Equal(
                expected: string.Empty,
                actual: Template
                    .Parse("{{context}}")
                    .Render(Hash.FromAnonymousObject(new { context = new IndexableLiquidizable() })));

            Assert.Equal(
                expected: "text1text2",
                actual: Template
                    .Parse("{{context.thekey}}")
                    .Render(Hash.FromAnonymousObject(new { context = new IndexableLiquidizable() })));
        }

        [Fact]
        public void TestDictionaryRendering()
        {
            Assert.Equal(
                expected: "[lambda, Hello][alpha, bet]",
                actual: Template
                    .Parse("{{context}}")
                    .Render(Hash.FromAnonymousObject(new { context = new Dictionary<string, object> { ["lambda"] = "Hello", ["alpha"] = "bet" } })));
        }

        [Fact]
        public void TestDictionaryAsVariable()
        {
            _context["dynamic"] = Hash.FromDictionary(new Dictionary<string, object> { ["lambda"] = "Hello" });

            Assert.Equal("Hello", _context["dynamic.lambda"]);
        }

        [Fact]
        public void TestNestedDictionaryAsVariable()
        {
            _context["dynamic"] = Hash.FromDictionary(new Dictionary<string, object> { ["lambda"] = new Dictionary<string, object> { ["name"] = "Hello" } });

            Assert.Equal("Hello", _context["dynamic.lambda.name"]);
        }

        [Fact]
        public void TestDynamicAsVariable()
        {
            dynamic expandoObject = new ExpandoObject();
            expandoObject.lambda = "Hello";
            _context["dynamic"] = Hash.FromDictionary(expandoObject);

            Assert.Equal("Hello", _context["dynamic.lambda"]);
        }

        [Fact]
        public void TestNestedDynamicAsVariable()
        {
            dynamic root = new ExpandoObject();
            root.lambda = new ExpandoObject();
            root.lambda.name = "Hello";
            _context["dynamic"] = Hash.FromDictionary(root);

            Assert.Equal("Hello", _context["dynamic.lambda.name"]);
        }

        [Fact]
        public void TestProcAsVariable()
        {
            _context["dynamic"] = (Proc) delegate { return "Hello"; };

            Assert.Equal("Hello", _context["dynamic"]);
        }

        [Fact]
        public void TestLambdaAsVariable()
        {
            _context["dynamic"] = (Proc) (c => "Hello");

            Assert.Equal("Hello", _context["dynamic"]);
        }

        [Fact]
        public void TestNestedLambdaAsVariable()
        {
            _context["dynamic"] = Hash.FromAnonymousObject(new { lambda = (Proc) (c => "Hello") });

            Assert.Equal("Hello", _context["dynamic.lambda"]);
        }

        [Fact]
        public void TestArrayContainingLambdaAsVariable()
        {
            _context["dynamic"] = new object[] { 1, 2, (Proc) (c => "Hello"), 4, 5 };

            Assert.Equal("Hello", _context["dynamic[2]"]);
        }

        [Fact]
        public void TestLambdaIsCalledOnce()
        {
            int global = 0;
            _context["callcount"] = (Proc) (c =>
            {
                ++global;
                return global.ToString();
            });

            Assert.Equal("1", _context["callcount"]);
            Assert.Equal("1", _context["callcount"]);
            Assert.Equal("1", _context["callcount"]);
        }

        [Fact]
        public void TestNestedLambdaIsCalledOnce()
        {
            int global = 0;
            _context["callcount"] = Hash.FromAnonymousObject(new
            {
                lambda = (Proc) (c =>
                {
                    ++global;
                    return global.ToString();
                })
            });

            Assert.Equal("1", _context["callcount.lambda"]);
            Assert.Equal("1", _context["callcount.lambda"]);
            Assert.Equal("1", _context["callcount.lambda"]);
        }

        [Fact]
        public void TestLambdaInArrayIsCalledOnce()
        {
            int global = 0;
            _context["callcount"] = new object[]
            { 1, 2, (Proc) (c =>
            {
                ++global;
                return global.ToString();
            }), 4, 5
            };

            Assert.Equal("1", _context["callcount[2]"]);
            Assert.Equal("1", _context["callcount[2]"]);
            Assert.Equal("1", _context["callcount[2]"]);
        }

        [Fact]
        public void TestAccessToContextFromProc()
        {
            _context.Registers["magic"] = 345392;

            _context["magic"] = (Proc) (c => _context.Registers["magic"]);

            Assert.Equal(345392, _context["magic"]);
        }

        [Fact]
        public void TestToLiquidAndContextAtFirstLevel()
        {
            _context["category"] = new Category("foobar");
            _context["category"].Should().BeOfType<CategoryDrop>();

            Assert.Equal(_context, ((CategoryDrop) _context["category"]).Context);
        }
    }
}
