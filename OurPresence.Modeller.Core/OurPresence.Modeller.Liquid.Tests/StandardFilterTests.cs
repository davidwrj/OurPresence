// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using FluentAssertions;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class StandardFilterTests
    {
        private readonly Context _contextV21;

        public StandardFilterTests()
        {
            _contextV21 = new Context(new Template(), CultureInfo.InvariantCulture)
            {
            };
        }

        [Fact]
        public void TestSize()
        {
            Assert.Equal(3, StandardFilters.Size(new[] { 1, 2, 3 }));
            Assert.Equal(0, StandardFilters.Size(new object[] { }));
            Assert.Equal(0, StandardFilters.Size(null));
        }

        [Fact]
        public void TestDowncase()
        {
            Assert.Equal("testing", StandardFilters.Downcase("Testing"));
            Assert.Null(StandardFilters.Downcase(null));
        }

        [Fact]
        public void TestUpcase()
        {
            Assert.Equal("TESTING", StandardFilters.Upcase("Testing"));
            Assert.Null(StandardFilters.Upcase(null));
        }

        [Fact]
        public void TestTruncate()
        {
            Assert.Null(StandardFilters.Truncate(null));
            Assert.Equal(expected: "", actual: StandardFilters.Truncate(""));
            Assert.Equal(expected: "1234...", actual: StandardFilters.Truncate("1234567890", 7));
            Assert.Equal(expected: "1234567890", actual: StandardFilters.Truncate("1234567890", 20));
            Assert.Equal(expected: "...", actual: StandardFilters.Truncate("1234567890", 0));
            Assert.Equal(expected: "1234567890", actual: StandardFilters.Truncate("1234567890"));
            Helper.AssertTemplateResult(expected: "H...", template: "{{ 'Hello' | truncate:4 }}");

            Helper.AssertTemplateResult(expected: "Ground control to...", template: "{{ \"Ground control to Major Tom.\" | truncate: 20}}");
            Helper.AssertTemplateResult(expected: "Ground control, and so on", template: "{{ \"Ground control to Major Tom.\" | truncate: 25, \", and so on\"}}");
            Helper.AssertTemplateResult(expected: "Ground control to Ma", template: "{{ \"Ground control to Major Tom.\" | truncate: 20, \"\"}}");
            Helper.AssertTemplateResult(expected: "...", template: "{{ \"Ground control to Major Tom.\" | truncate: 0}}");
            Helper.AssertTemplateResult(expected: "Liquid error: Value was either too large or too small for an Int32.", template: $"{{{{ \"Ground control to Major Tom.\" | truncate: {(long)int.MaxValue + 1}}}}}");
            Helper.AssertTemplateResult(expected: "...", template: "{{ \"Ground control to Major Tom.\" | truncate: -1}}");
        }

        [Fact]
        public void TestEscape()
        {
            Assert.Null(StandardFilters.Escape(null));
            Assert.Equal("", StandardFilters.Escape(""));
            Assert.Equal("&lt;strong&gt;", StandardFilters.Escape("<strong>"));
            Assert.Equal("&lt;strong&gt;", StandardFilters.H("<strong>"));
        }

        [Fact]
        public void TestTruncateWords()
        {
            Assert.Null(StandardFilters.TruncateWords(null));
            Assert.Equal("", StandardFilters.TruncateWords(""));
            Assert.Equal("one two three", StandardFilters.TruncateWords("one two three", 4));
            Assert.Equal("one two...", StandardFilters.TruncateWords("one two three", 2));
            Assert.Equal("one two three", StandardFilters.TruncateWords("one two three"));
            Assert.Equal("Two small (13&#8221; x 5.5&#8221; x 10&#8221; high) baskets fit inside one large basket (13&#8221;...", StandardFilters.TruncateWords("Two small (13&#8221; x 5.5&#8221; x 10&#8221; high) baskets fit inside one large basket (13&#8221; x 16&#8221; x 10.5&#8221; high) with cover.", 15));

            Helper.AssertTemplateResult(expected: "Ground control to...", template: "{{ \"Ground control to Major Tom.\" | truncate_words: 3}}");
            Helper.AssertTemplateResult(expected: "Ground control to--", template: "{{ \"Ground control to Major Tom.\" | truncate_words: 3, \"--\"}}");
            Helper.AssertTemplateResult(expected: "Ground control to", template: "{{ \"Ground control to Major Tom.\" | truncate_words: 3, \"\"}}");
            Helper.AssertTemplateResult(expected: "...", template: "{{ \"Ground control to Major Tom.\" | truncate_words: 0}}");
            Helper.AssertTemplateResult(expected: "...", template: "{{ \"Ground control to Major Tom.\" | truncate_words: -1}}");
            Helper.AssertTemplateResult(expected: "Liquid error: Value was either too large or too small for an Int32.", template: $"{{{{ \"Ground control to Major Tom.\" | truncate_words: {(long)int.MaxValue + 1}}}}}");
        }

        [Fact]
        public void TestSplit()
        {
            Assert.Equal(new[] { "This", "is", "a", "sentence" }, StandardFilters.Split("This is a sentence", " "));
            Assert.Equal(new string[] { null }, StandardFilters.Split(null, null));
        }

        [Fact]
        public void TestStripHtml()
        {
            Assert.Equal("test", StandardFilters.StripHtml(_contextV21.Template, "<div>test</div>"));
            Assert.Equal("test", StandardFilters.StripHtml(_contextV21.Template, "<div id='test'>test</div>"));
            Assert.Null(StandardFilters.StripHtml(_contextV21.Template, null));
        }

        [Fact]
        public void TestStrip()
        {
            Assert.Equal("test", StandardFilters.Strip("  test  "));
            Assert.Equal("test", StandardFilters.Strip("   test"));
            Assert.Equal("test", StandardFilters.Strip("test   "));
            Assert.Equal("test", StandardFilters.Strip("test"));
            Assert.Null(StandardFilters.Strip(null));
        }

        [Fact]
        public void TestLStrip()
        {
            Assert.Equal("test  ", StandardFilters.Lstrip("  test  "));
            Assert.Equal("test", StandardFilters.Lstrip("   test"));
            Assert.Equal("test   ", StandardFilters.Lstrip("test   "));
            Assert.Equal("test", StandardFilters.Lstrip("test"));
            Assert.Null(StandardFilters.Lstrip(null));
        }

        [Fact]
        public void TestRStrip()
        {
            Assert.Equal("  test", StandardFilters.Rstrip("  test  "));
            Assert.Equal("   test", StandardFilters.Rstrip("   test"));
            Assert.Equal("test", StandardFilters.Rstrip("test   "));
            Assert.Equal("test", StandardFilters.Rstrip("test"));
            Assert.Null(StandardFilters.Rstrip(null));
        }

        [Fact]
        public void TestSlice()
        {
            Assert.Null(StandardFilters.Slice(null, 1));
            Assert.Null(StandardFilters.Slice("", 10));
            Assert.Equal("abc", StandardFilters.Slice("abcdefg", 0, 3));
            Assert.Equal("bcd", StandardFilters.Slice("abcdefg", 1, 3));
            Assert.Equal("efg", StandardFilters.Slice("abcdefg", -3, 3));
            Assert.Equal("efg", StandardFilters.Slice("abcdefg", -3, 30));
            Assert.Equal("efg", StandardFilters.Slice("abcdefg", 4, 30));
            Assert.Equal("a", StandardFilters.Slice("abc", -4, 2));
            Assert.Equal("", StandardFilters.Slice("abcdefg", -10, 1));
        }

        [Fact]
        public void TestJoin()
        {
            Assert.Null(StandardFilters.Join(null));
            Assert.Equal("", StandardFilters.Join(""));
            Assert.Equal("1 2 3 4", StandardFilters.Join(new[] { 1, 2, 3, 4 }));
            Assert.Equal("1 - 2 - 3 - 4", StandardFilters.Join(new[] { 1, 2, 3, 4 }, " - "));

            // Sample from specification at https://shopify.github.io/liquid/filters/join/
            Helper.AssertTemplateResult(
                expected: "\r\nJohn and Paul and George and Ringo",
                template: @"{% assign beatles = ""John, Paul, George, Ringo"" | split: "", "" %}
{{ beatles | join: "" and "" }}");
        }

        [Fact]
        public void TestSort()
        {
            Assert.Null(StandardFilters.Sort(null));
            StandardFilters.Sort(new string[] { }).Should().BeEquivalentTo(new string[] { });
            StandardFilters.Sort(new[] { 4, 3, 2, 1 }).Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
            StandardFilters.Sort(new[] { new { a = 4 }, new { a = 3 }, new { a = 1 }, new { a = 2 } }, "a").Should().BeEquivalentTo(new[] { new { a = 1 }, new { a = 2 }, new { a = 3 }, new { a = 4 } });
        }

        [Fact]
        public void TestSort_OnHashList_WithProperty_DoesNotFlattenList()
        {
            var list = new List<Hash>();
            var hash1 = CreateHash("1", "Text1");
            var hash2 = CreateHash("2", "Text2");
            var hash3 = CreateHash("3", "Text3");
            list.Add(hash3);
            list.Add(hash1);
            list.Add(hash2);

            var result = StandardFilters.Sort(list, "sortby").Cast<Hash>().ToArray();
            Assert.Equal(3, result.Count());
            Assert.Equal(hash1["content"], result[0]["content"]);
            Assert.Equal(hash2["content"], result[1]["content"]);
            Assert.Equal(hash3["content"], result[2]["content"]);
        }

        [Fact]
        public void TestSort_OnDictionaryWithPropertyOnlyInSomeElement_ReturnsSortedDictionary()
        {
            var list = new List<Hash>();
            var hash1 = CreateHash("1", "Text1");
            var hash2 = CreateHash("2", "Text2");
            var hashWithNoSortByProperty = new Hash();
            hashWithNoSortByProperty.Add("content", "Text 3");
            list.Add(hash2);
            list.Add(hashWithNoSortByProperty);
            list.Add(hash1);

            var result = StandardFilters.Sort(list, "sortby").Cast<Hash>().ToArray();
            Assert.Equal(3, result.Count());
            Assert.Equal(hashWithNoSortByProperty["content"], result[0]["content"]);
            Assert.Equal(hash1["content"], result[1]["content"]);
            Assert.Equal(hash2["content"], result[2]["content"]);
        }

        private static Hash CreateHash(string sortby, string content) =>
            new Hash
            {
                { "sortby", sortby },
                { "content", content }
            };

        [Fact]
        public void TestMap()
        {
            var tpl = _contextV21.Template;

            StandardFilters.Map(tpl, new string[] { }, "a").Should().BeEquivalentTo(new string[] { });
            StandardFilters.Map(tpl, new[] { new { a = 1 }, new { a = 2 }, new { a = 3 }, new { a = 4 } }, "a").Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
            Helper.AssertTemplateResult("abc", "{{ ary | map:'foo' | map:'bar' }}",
                Hash.FromAnonymousObject(
                    new
                    {
                        ary =
                            new[]
                    {
                        Hash.FromAnonymousObject(new { foo = Hash.FromAnonymousObject(new { bar = "a" }) }), Hash.FromAnonymousObject(new { foo = Hash.FromAnonymousObject(new { bar = "b" }) }),
                        Hash.FromAnonymousObject(new { foo = Hash.FromAnonymousObject(new { bar = "c" }) })
                    }
                    }));
            StandardFilters.Map(tpl, new[] { new { a = 1 }, new { a = 2 }, new { a = 3 }, new { a = 4 } }, "b").Should().BeEquivalentTo(new[] { new { a = 1 }, new { a = 2 }, new { a = 3 }, new { a = 4 } });

            Assert.Null(StandardFilters.Map(tpl, null, "a"));
            StandardFilters.Map(tpl, new object[] { null }, "a").Should().BeEquivalentTo(new object[] { null });

            var hash = Hash.FromAnonymousObject(new
            {
                ary = new[] {
                    new Helper.DataObject { PropAllowed = "a", PropDisallowed = "x" },
                    new Helper.DataObject { PropAllowed = "b", PropDisallowed = "y" },
                    new Helper.DataObject { PropAllowed = "c", PropDisallowed = "z" },
                }
            });

            Helper.AssertTemplateResult("abc", "{{ ary | map:'prop_allowed' | join:'' }}", hash);
            Helper.AssertTemplateResult("", "{{ ary | map:'prop_disallowed' | join:'' }}", hash);

            hash = Hash.FromAnonymousObject(new
            {
                ary = new[] {
                    new Helper.DataObjectDrop(tpl) { Prop = "a" },
                    new Helper.DataObjectDrop(tpl) { Prop = "b" },
                    new Helper.DataObjectDrop(tpl) { Prop = "c" },
                }
            });

            Helper.AssertTemplateResult("abc", "{{ ary | map:'prop' | join:'' }}", hash);
            Helper.AssertTemplateResult("", "{{ ary | map:'no_prop' | join:'' }}", hash);
        }

        /// <summary>
        /// Tests map filter per Shopify specification sample
        /// </summary>
        /// <remarks><see href="https://shopify.github.io/liquid/filters/map/"/></remarks>
        [Fact]
        public void TestMapSpecificationSample()
        {
            var hash = Hash.FromAnonymousObject(new
            {
                site = new
                {
                    pages = new[] {
                        new { category = "business" },
                        new { category = "celebrities" },
                        new { category = "lifestyle" },
                        new { category = "sports" },
                        new { category = "technology" }
                    }
                }
            });

            Helper.AssertTemplateResult(
                expected: "\r\n- business\r\n\r\n- celebrities\r\n\r\n- lifestyle\r\n\r\n- sports\r\n\r\n- technology\r\n",
                template: @"{% assign all_categories = site.pages | map: ""category"" %}{% for item in all_categories %}
- {{ item }}
{% endfor %}",
                localVariables: hash);
        }

        /// <summary>
        /// Tests map filter per Shopify specification sample
        /// </summary>
        /// <remarks>In this variant of the test we add another property to the items in the collection to ensure the map filter does its job of removing other properties</remarks>
        [Fact]
        public void TestMapSpecificationSampleVariant()
        {
            var hash = Hash.FromAnonymousObject(new
            {
                site = new
                {
                    pages = new[] {
                        new { category = "business", author = "Joe" },
                        new { category = "celebrities", author = "Jon" },
                        new { category = "lifestyle", author = "John" },
                        new { category = "sports", author = "Joan" },
                        new { category = "technology", author = "Jean" }
                    }
                }
            });

            Helper.AssertTemplateResult(
                expected: "\r\n- business\r\n\r\n- celebrities\r\n\r\n- lifestyle\r\n\r\n- sports\r\n\r\n- technology\r\n",
                template: @"{% assign all_categories = site.pages | map: ""category"" %}{% for item in all_categories %}
- {{ item }}
{% endfor %}", localVariables: hash);
        }

        [Fact]
        public void TestMapShipmentPackage()
        {
            var hash = Hash.FromAnonymousObject(new
            {
                content = new
                {
                    carrierSettings = new[] {
                        new
                        {
                            numberOfPiecesPerPackage = 10,
                            test = "test"
                        },
                        new
                        {
                            numberOfPiecesPerPackage = 12,
                            test = "test1"
                        }
                    }
                }
            });

            Helper.AssertTemplateResult(
                expected: "{\r\n\r\n\"tests\" : [\r\n            {\r\n                \"numberOfPiecesPerPackage\" : \"10\"\r\n      },\r\n      \r\n            {\r\n                \"numberOfPiecesPerPackage\" : \"12\"\r\n      },\r\n      ]\r\n}",
                template: @"{
{% assign test1 = content.carrierSettings | map: ""numberOfPiecesPerPackage"" %}
""tests"" : [{% for test in test1 %}
            {
                ""numberOfPiecesPerPackage"" : ""{{ test }}""
      },
      {% endfor %}]
}",
                localVariables: hash);

            Helper.AssertTemplateResult(
                expected: "{\r\n\r\n\"tests\" : 1012\r\n}",
                template: @"{
{% assign test1 = content.carrierSettings | map: ""numberOfPiecesPerPackage"" %}
""tests"" : {{test1}}
}",
                localVariables: hash);
        }

        private class Package : IIndexable, ILiquidizable
        {
#pragma warning disable IDE1006 // Naming Styles
            private readonly int numberOfPiecesPerPackage;
            private readonly string test;
#pragma warning restore IDE1006 // Naming Styles

            public Package(int numberOfPiecesPerPackage, string test)
            {
                this.numberOfPiecesPerPackage = numberOfPiecesPerPackage;
                this.test = test;
            }

            public object this[object key] => key as string == "numberOfPiecesPerPackage"
                ? this.numberOfPiecesPerPackage as object
                : key as string == "test"
                    ? test
                    : null;

            public bool ContainsKey(object key)
            {
                return new List<string> { nameof(numberOfPiecesPerPackage), nameof(test) }
                    .Contains(key);
            }

            public object ToLiquid()
            {
                return this;
            }
        };

        [Fact]
        public void TestMapIndexable()
        {
            var hash = Hash.FromAnonymousObject(new
            {
                content = new
                {
                    carrierSettings = new[]
                    {
                        new Package(numberOfPiecesPerPackage:10, test:"test"),
                        new Package(numberOfPiecesPerPackage:12, test:"test1"),
                    }
                }
            });

            Helper.AssertTemplateResult(
                expected: "{\r\n\r\n\"tests\" : [\r\n            {\r\n                \"numberOfPiecesPerPackage\" : \"10\"\r\n      },\r\n      \r\n            {\r\n                \"numberOfPiecesPerPackage\" : \"12\"\r\n      },\r\n      ]\r\n}",
                template: @"{
{% assign test1 = content.carrierSettings | map: ""numberOfPiecesPerPackage"" %}
""tests"" : [{% for test in test1 %}
            {
                ""numberOfPiecesPerPackage"" : ""{{ test }}""
      },
      {% endfor %}]
}",
                localVariables: hash);

            Helper.AssertTemplateResult(
                expected: "{\r\n\r\n\"tests\" : 1012\r\n}",
                template: @"{
{% assign test1 = content.carrierSettings | map: ""numberOfPiecesPerPackage"" %}
""tests"" : {{test1}}
}",
                localVariables: hash);
        }

        [Fact]
        public void TestMapJoin()
        {
            var hash = Hash.FromAnonymousObject(new
            {
                content = new
                {
                    carrierSettings = new[] {
                        new
                        {
                            numberOfPiecesPerPackage = 10,
                            test = "test"
                        },
                        new
                        {
                            numberOfPiecesPerPackage = 12,
                            test = "test1"
                        }
                    }
                }
            });

            Helper.AssertTemplateResult(
                expected: "\r\n{ \"test\": \"10, 12\"}",
                template: @"{% assign test = content.carrierSettings | map: ""numberOfPiecesPerPackage"" | join: "", ""%}
{ ""test"": ""{{test}}""}",
                localVariables: hash);
        }

        [InlineData("6.72", "$6.72")]
        [InlineData("6000", "$6,000.00")]
        [InlineData("6000000", "$6,000,000.00")]
        [InlineData("6000.4", "$6,000.40")]
        [InlineData("6000000.4", "$6,000,000.40")]
        [InlineData("6.8458", "$6.85")]
        public void TestAmericanCurrencyFromString(string input, string expected)
        {
#if CORE
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
#else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
#endif
            Assert.Equal(expected, StandardFilters.Currency(input));
        }

        [InlineData("6.72", "6,72 €")]
        [InlineData("6000", "6.000,00 €")]
        [InlineData("6000000", "6.000.000,00 €")]
        [InlineData("6000.4", "6.000,40 €")]
        [InlineData("6000000.4", "6.000.000,40 €")]
        [InlineData("6.8458", "6,85 €")]
        public void TestEuroCurrencyFromString(string input, string expected)
        {
#if CORE
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
#else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
#endif
            Assert.Equal(expected, StandardFilters.Currency(input, "de-DE"));
        }

        [Fact]
        public void TestMalformedCurrency()
        {
            Assert.Equal("teststring", StandardFilters.Currency("teststring", "de-DE"));
        }

        [Fact]
        public void TestCurrencyWithinTemplateRender()
        {
#if CORE
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
#else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
#endif

            Template dollarTemplate = Template.Parse(@"{{ amount | currency }}");
            Template euroTemplate = Template.Parse(@"{{ amount | currency: ""de-DE"" }}");

            Assert.Equal("$7,000.00", dollarTemplate.Render(Hash.FromAnonymousObject(new { amount = "7000" })));
            Assert.Equal("7.000,00 €", euroTemplate.Render(Hash.FromAnonymousObject(new { amount = 7000 })));
        }

        [Fact]
        public void TestCurrencyFromDoubleInput()
        {
            Assert.Equal("$6.85", StandardFilters.Currency(6.8458, "en-US"));
            Assert.Equal("$6.72", StandardFilters.Currency(6.72, "en-CA"));
            Assert.Equal("6.000.000,00 €", StandardFilters.Currency(6000000, "de-DE"));
            Assert.Equal("6.000.000,78 €", StandardFilters.Currency(6000000.78, "de-DE"));
        }

        public void TestDate(Context context)
        {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

            Assert.Equal(dateTimeFormat.GetMonthName(5), StandardFilters.Date(context: context, input: DateTime.Parse("2006-05-05 10:00:00"), format: "MMMM"));
            Assert.Equal(dateTimeFormat.GetMonthName(6), StandardFilters.Date(context: context, input: DateTime.Parse("2006-06-05 10:00:00"), format: "MMMM"));
            Assert.Equal(dateTimeFormat.GetMonthName(7), StandardFilters.Date(context: context, input: DateTime.Parse("2006-07-05 10:00:00"), format: "MMMM"));

            Assert.Equal(dateTimeFormat.GetMonthName(5), StandardFilters.Date(context: context, input: "2006-05-05 10:00:00", format: "MMMM"));
            Assert.Equal(dateTimeFormat.GetMonthName(6), StandardFilters.Date(context: context, input: "2006-06-05 10:00:00", format: "MMMM"));
            Assert.Equal(dateTimeFormat.GetMonthName(7), StandardFilters.Date(context: context, input: "2006-07-05 10:00:00", format: "MMMM"));

            Assert.Equal("08/01/2006 10:00:00", StandardFilters.Date(context: context, input: "08/01/2006 10:00:00", format: string.Empty));
            Assert.Equal("08/02/2006 10:00:00", StandardFilters.Date(context: context, input: "08/02/2006 10:00:00", format: null));
            Assert.Equal(new DateTime(2006, 8, 3, 10, 0, 0).ToString(), StandardFilters.Date(context: context, input: new DateTime(2006, 8, 3, 10, 0, 0), format: string.Empty));
            Assert.Equal(new DateTime(2006, 8, 4, 10, 0, 0).ToString(), StandardFilters.Date(context: context, input: new DateTime(2006, 8, 4, 10, 0, 0), format: null));

            Assert.Equal(new DateTime(2006, 7, 5).ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "2006-07-05 10:00:00", format: "MM/dd/yyyy"));

            Assert.Equal(new DateTime(2004, 7, 16).ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "Fri Jul 16 2004 01:00:00", format: "MM/dd/yyyy"));

            Assert.Null(StandardFilters.Date(context: context, input: null, format: "MMMM"));

            Assert.Equal("hi", StandardFilters.Date(context: context, input: "hi", format: "MMMM"));

            Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "now", format: "MM/dd/yyyy"));
            Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "today", format: "MM/dd/yyyy"));
            Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "Now", format: "MM/dd/yyyy"));
            Assert.Equal(DateTime.Now.ToString("MM/dd/yyyy"), StandardFilters.Date(context: context, input: "Today", format: "MM/dd/yyyy"));

            Assert.Equal("345000", StandardFilters.Date(context: context, input: DateTime.Parse("2006-05-05 10:00:00.345"), format: "ffffff"));

            Template template = Template.Parse(@"{{ hi | date:""MMMM"" }}");
            Assert.Equal("hi", template.Render(Hash.FromAnonymousObject(new { hi = "hi" })));
        }

        [Fact]
        public void TestDateV21()
        {
            var context = _contextV21;
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            Assert.Equal(unixEpoch.ToString("g"), StandardFilters.Date(context: context, input: 0, format: "g"));
            Assert.Equal(unixEpoch.AddSeconds(Int32.MaxValue).AddSeconds(1).ToString("g"), StandardFilters.Date(context: context, input: 2147483648, format: "g")); // Beyond Int32 boundary
            Assert.Equal(unixEpoch.AddSeconds(UInt32.MaxValue).AddSeconds(1).ToString("g"), StandardFilters.Date(context: context, input: 4294967296, format: "g")); // Beyond UInt32 boundary
            Helper.AssertTemplateResult(expected: unixEpoch.ToString("g"), template: "{{ 0 | date: 'g' }}");
            Helper.AssertTemplateResult(expected: unixEpoch.AddSeconds(Int32.MaxValue).AddSeconds(1).ToString("g"), template: "{{ 2147483648 | date: 'g' }}");
            Helper.AssertTemplateResult(expected: unixEpoch.AddSeconds(UInt32.MaxValue).AddSeconds(1).ToString("g"), template: "{{ 4294967296 | date: 'g' }}");

            var testDate = new DateTime(2006, 8, 4, 10, 0, 0, DateTimeKind.Unspecified);
            Assert.Equal("-14:00", StandardFilters.Date(context: context, input: new DateTimeOffset(testDate, TimeSpan.FromHours(-14)), format: "zzz"));
            Helper.AssertTemplateResult(expected: "+00:00", template: "{{ '" + testDate.ToString("u") + "' | date: 'zzz' }}");
            Helper.AssertTemplateResult(expected: "-14:00", template: "{{ '" + testDate.ToString("u").Replace("Z", "-14:00") + "' | date: 'zzz' }}");

            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = 0 }));
            Helper.AssertTemplateResult(expected: "2147483648", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = 2147483648 }));
            Helper.AssertTemplateResult(expected: "4294967296", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = 4294967296 }));
            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = unixEpoch.ToUniversalTime() }));
            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = unixEpoch }));
            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = DateTime.SpecifyKind(unixEpoch, DateTimeKind.Unspecified) }));
            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = new DateTimeOffset(unixEpoch) }));
            Helper.AssertTemplateResult(expected: "0", template: "{{ epoch | date: '%s' }}", localVariables: Hash.FromAnonymousObject(new { epoch = new DateTimeOffset(unixEpoch).ToOffset(TimeSpan.FromHours(-14)) }));

            Assert.Equal("now", StandardFilters.Date(context: context, input: "now", format: null));
            Assert.Equal("today", StandardFilters.Date(context: context, input: "today", format: null));
            Assert.Equal("now", StandardFilters.Date(context: context, input: "now", format: string.Empty));
            Assert.Equal("today", StandardFilters.Date(context: context, input: "today", format: string.Empty));

            TestDate(context);
        }

        [Fact]
        public void TestFirstLastUsingCSharp()
        {
            TestFirstLast((name) => char.ToUpperInvariant(name[0]) + name.Substring(1));
        }

        private void TestFirstLast(Func<string, string> filterNameFunc)
        {
            var splitFilter = filterNameFunc("split");
            var firstFilter = filterNameFunc("first");
            var lastFilter = filterNameFunc("last");

            Assert.Null(StandardFilters.First(null));
            Assert.Null(StandardFilters.Last(null));
            Assert.Equal(1, StandardFilters.First(new[] { 1, 2, 3 }));
            Assert.Equal(3, StandardFilters.Last(new[] { 1, 2, 3 }));
            Assert.Null(StandardFilters.First(new object[] { }));
            Assert.Null(StandardFilters.Last(new object[] { }));

            Helper.AssertTemplateResult(
                expected: ".",
                template: "{{ 'Ground control to Major Tom.' | " + lastFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "Tom.",
                template: "{{ 'Ground control to Major Tom.' | " + splitFilter + ": ' ' | " + lastFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "tiger",
                template: "{% assign my_array = 'zebra, octopus, giraffe, tiger' | " + splitFilter + ": ', ' %}{{ my_array." + lastFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "There goes a tiger!",
                template: "{% assign my_array = 'zebra, octopus, giraffe, tiger' | " + splitFilter + ": ', ' %}{% if my_array." + lastFilter + " == 'tiger' %}There goes a tiger!{% endif %}");

            Helper.AssertTemplateResult(
                expected: "G",
                template: "{{ 'Ground control to Major Tom.' | " + firstFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "Ground",
                template: "{{ 'Ground control to Major Tom.' | " + splitFilter + ": ' ' | " + firstFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "zebra",
                template: "{% assign my_array = 'zebra, octopus, giraffe, tiger' | " + splitFilter + ": ', ' %}{{ my_array." + firstFilter + " }}");
            Helper.AssertTemplateResult(
                expected: "There goes a zebra!",
                template: "{% assign my_array = 'zebra, octopus, giraffe, tiger' | " + splitFilter + ": ', ' %}{% if my_array." + firstFilter + " == 'zebra' %}There goes a zebra!{% endif %}");
        }

        public void TestReplace(Context context)
        {
            Assert.Null(StandardFilters.Replace(context: context, input: null, @string: "a", replacement: "b"));
            Assert.Equal(expected: "", actual: StandardFilters.Replace(context: context, input: "", @string: "a", replacement: "b"));
            Assert.Equal(expected: "a a a a", actual: StandardFilters.Replace(context: context, input: "a a a a", @string: null, replacement: "b"));
            Assert.Equal(expected: "a a a a", actual: StandardFilters.Replace(context: context, input: "a a a a", @string: "", replacement: "b"));
            Assert.Equal(expected: "b b b b", actual: StandardFilters.Replace(context: context, input: "a a a a", @string: "a", replacement: "b"));

            Assert.Equal(expected: "Tesvalue\\\"", actual: StandardFilters.Replace(context: context, input: "Tesvalue\"", @string: "\"", replacement: "\\\""));
            Helper.AssertTemplateResult(expected: "Tesvalue\\\"", template: "{{ 'Tesvalue\"' | replace: '\"', '\\\"' }}");
            Helper.AssertTemplateResult(
                expected: "Tesvalue\\\"",
                template: "{{ context | replace: '\"', '\\\"' }}",
                localVariables: Hash.FromAnonymousObject(new { context = "Tesvalue\"" }));
        }

        [Fact]
        public void TestReplaceRegexV21()
        {
            var context = _contextV21;
            Assert.Equal(expected: "a A A a", actual: StandardFilters.Replace(context: context, input: "a A A a", @string: "[Aa]", replacement: "b"));
            TestReplace(context);
        }

        public void TestReplaceFirst(Context context)
        {
            Assert.Null(StandardFilters.ReplaceFirst(context: context, input: null, @string: "a", replacement: "b"));
            Assert.Equal("", StandardFilters.ReplaceFirst(context: context, input: "", @string: "a", replacement: "b"));
            Assert.Equal("a a a a", StandardFilters.ReplaceFirst(context: context, input: "a a a a", @string: null, replacement: "b"));
            Assert.Equal("a a a a", StandardFilters.ReplaceFirst(context: context, input: "a a a a", @string: "", replacement: "b"));
            Assert.Equal("b a a a", StandardFilters.ReplaceFirst(context: context, input: "a a a a", @string: "a", replacement: "b"));
            Helper.AssertTemplateResult(expected: "b a a a", template: "{{ 'a a a a' | replace_first: 'a', 'b' }}");
        }

        [Fact]
        public void TestReplaceFirstRegexV21()
        {
            var context = _contextV21;
            Assert.Equal(expected: "a A A a", actual: StandardFilters.ReplaceFirst(context: context, input: "a A A a", @string: "[Aa]", replacement: "b"));
            TestReplaceFirst(context);
        }

        public void TestRemove(Context context)
        {

            Assert.Equal("   ", StandardFilters.Remove("a a a a", "a"));
            Assert.Equal("a a a", StandardFilters.RemoveFirst(context: context, input: "a a a a", @string: "a "));
            Helper.AssertTemplateResult(expected: "a a a", template: "{{ 'a a a a' | remove_first: 'a ' }}");
        }

        [Fact]
        public void TestRemoveFirstRegexV21()
        {
            var context = _contextV21;
            Assert.Equal(expected: "Mr Jones", actual: StandardFilters.RemoveFirst(context: context, input: "Mr. Jones", @string: "."));
            TestRemove(context);
        }

        [Fact]
        public void TestPipesInStringArguments()
        {
            Helper.AssertTemplateResult("foobar", "{{ 'foo|bar' | remove: '|' }}");
        }

        [Fact]
        public void TestStripWindowsNewlines()
        {
            Helper.AssertTemplateResult("abc", "{{ source | strip_newlines }}", Hash.FromAnonymousObject(new { source = "a\r\nb\r\nc" }));
            Helper.AssertTemplateResult("ab", "{{ source | strip_newlines }}", Hash.FromAnonymousObject(new { source = "a\r\n\r\n\r\nb" }));
        }

        [Fact]
        public void TestStripUnixNewlines()
        {
            Helper.AssertTemplateResult("abc", "{{ source | strip_newlines }}", Hash.FromAnonymousObject(new { source = "a\nb\nc" }));
            Helper.AssertTemplateResult("ab", "{{ source | strip_newlines }}", Hash.FromAnonymousObject(new { source = "a\n\n\nb" }));
        }

        [Fact]
        public void TestWindowsNewlinesToBr()
        {
            Helper.AssertTemplateResult("a<br />\r\nb<br />\r\nc",
                "{{ source | newline_to_br }}",
                Hash.FromAnonymousObject(new { source = "a\r\nb\r\nc" }));
        }

        [Fact]
        public void TestUnixNewlinesToBr()
        {
            Helper.AssertTemplateResult("a<br />\nb<br />\nc",
                "{{ source | newline_to_br }}",
                Hash.FromAnonymousObject(new { source = "a\nb\nc" }));
        }

        private void TestPlus(Context context)
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult(expected: "2", template: "{{ 1 | plus:1 }}");
                Helper.AssertTemplateResult(expected: "5.5", template: "{{ 2  | plus:3.5 }}");
                Helper.AssertTemplateResult(expected: "5.5", template: "{{ 3.5 | plus:2 }}");

                // Test that decimals are not introducing rounding-precision issues
                Helper.AssertTemplateResult(expected: "148397.77", template: "{{ 148387.77 | plus:10 }}");

                Helper.AssertTemplateResult(
                    expected: "2147483648",
                    template: "{{ i | plus: i2 }}",
                    localVariables: Hash.FromAnonymousObject(new { i = (int)Int32.MaxValue, i2 = (Int64)1 }));
            }
        }

        [Fact]
        public void TestPlusStringV21()
        {
            var context = _contextV21;
            Helper.AssertTemplateResult(expected: "2", template: "{{ '1' | plus: 1 }}");
            Helper.AssertTemplateResult(expected: "2", template: "{{ 1 | plus: '1' }}");
            Helper.AssertTemplateResult(expected: "2", template: "{{ '1' | plus: '1' }}");
            Helper.AssertTemplateResult(expected: "5.5", template: "{{ 2 | plus: '3.5' }}");
            Helper.AssertTemplateResult(expected: "5.5", template: "{{ '3.5' | plus: 2 }}");
            TestPlus(context);
        }

        private void TestMinus(Context context)
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult(expected: "4", template: "{{ input | minus:operand }}", localVariables: Hash.FromAnonymousObject(new { input = 5, operand = 1 }));
                Helper.AssertTemplateResult(expected: "-1.5", template: "{{ 2  | minus:3.5 }}");
                Helper.AssertTemplateResult(expected: "1.5", template: "{{ 3.5 | minus:2 }}");
            }
        }

        [Fact]
        public void TestMinusStringV21()
        {
            var context = _contextV21;
            Helper.AssertTemplateResult(expected: "1", template: "{{ '2' | minus: 1 }}");
            Helper.AssertTemplateResult(expected: "1", template: "{{ 2 | minus: '1' }}");
            Helper.AssertTemplateResult(expected: "-1.5", template: "{{ 2 | minus: '3.5' }}");
            Helper.AssertTemplateResult(expected: "-1.5", template: "{{ '2.5' | minus: 4 }}");
            Helper.AssertTemplateResult(expected: "-1", template: "{{ '2.5' | minus: '3.5' }}");
            TestMinus(context);
        }

        [Fact]
        public void TestPlusCombinedWithMinus()
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                // This detects rounding issues not visible with single operation.
                Helper.AssertTemplateResult("0.1", "{{ 0.1 | plus: 10 | minus: 10 }}");
            }
        }

        [Fact]
        public void TestMinusWithFrenchDecimalSeparator()
        {
            using(CultureHelper.SetCulture("fr-FR"))
            {
                Helper.AssertTemplateResult(string.Format("1{0}2", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator),
                    "{{ 3,2 | minus:2 | round:1 }}");
            }
        }

        [Fact]
        public void TestRound()
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult("1.235", "{{ 1.234678 | round:3 }}");
                Helper.AssertTemplateResult("1", "{{ 1 | round }}");

                Assert.Null(StandardFilters.Round("1.2345678", "two"));
            }
        }

        [Fact]
        public void TestCeil()
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult("2", "{{ 1.2 | ceil }}");
                Helper.AssertTemplateResult("2", "{{ 2.0 | ceil }}");
                Helper.AssertTemplateResult("184", "{{ 183.357 | ceil }}");
                Helper.AssertTemplateResult("4", "{{ \"3.5\" | ceil }}");

                Assert.Null(StandardFilters.Ceil(""));
                Assert.Null(StandardFilters.Ceil("two"));
            }
        }

        [Fact]
        public void TestFloor()
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult("1", "{{ 1.2 | floor }}");
                Helper.AssertTemplateResult("2", "{{ 2.0 | floor }}");
                Helper.AssertTemplateResult("183", "{{ 183.357 | floor }}");
                Helper.AssertTemplateResult("3", "{{ \"3.5\" | floor }}");

                Assert.Null(StandardFilters.Floor(""));
                Assert.Null(StandardFilters.Floor("two"));
            }
        }

        private void TestTimes(Context context)
        {
            using(CultureHelper.SetCulture("en-GB"))
            {
                Helper.AssertTemplateResult(expected: "12", template: "{{ 3 | times:4 }}");
                Helper.AssertTemplateResult(expected: "125", template: "{{ 10 | times:12.5 }}");
                Helper.AssertTemplateResult(expected: "125", template: "{{ 10.0 | times:12.5 }}");
                Helper.AssertTemplateResult(expected: "125", template: "{{ 12.5 | times:10 }}");
                Helper.AssertTemplateResult(expected: "125", template: "{{ 12.5 | times:10.0 }}");

                // Test against overflows when we try to be precise but the result exceeds the range of the input type.
                Helper.AssertTemplateResult(
                    expected: ((double)((decimal.MaxValue / 100) + (decimal).1) * (double)((decimal.MaxValue / 100) + (decimal).1)).ToString(),
                    template: $"{{{{ {(decimal.MaxValue / 100) + (decimal).1} | times:{(decimal.MaxValue / 100) + (decimal).1} }}}}");

                // Test against overflows going beyond the double precision float type's range
                Helper.AssertTemplateResult(
                    expected: double.NegativeInfinity.ToString(),
                    template: $"{{{{ 12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.0 | times:-12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.0 }}}}");
                Helper.AssertTemplateResult(
                    expected: double.PositiveInfinity.ToString(),
                    template: $"{{{{ 12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.0 | times:12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.0 }}}}");

                // Ensures no underflow exception is thrown when the result doesn't fit the precision of double.
                Helper.AssertTemplateResult(expected: "0",
                    template: $"{{{{ 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001 | times:0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001 }}}}");
            }

            Assert.Equal(8.43, StandardFilters.Times(context: context, input: 0.843m, operand: 10));
            Assert.Equal(412, StandardFilters.Times(context: context, input: 4.12m, operand: 100));
            Assert.Equal(7556.3, StandardFilters.Times(context: context, input: 7.5563m, operand: 1000));
        }

        [Fact]
        public void TestTimesStringV21()
        {
            var context = _contextV21;
            Helper.AssertTemplateResult(expected: "12", template: "{{ '3' | times: 4 }}");
            Helper.AssertTemplateResult(expected: "12", template: "{{ 3 | times: '4' }}");
            Helper.AssertTemplateResult(expected: "12", template: "{{ '3' | times: '4' }}");
            TestTimes(context);
        }

        [Fact]
        public void TestAppend()
        {
            Hash assigns = Hash.FromAnonymousObject(new { a = "bc", b = "d" });
            Helper.AssertTemplateResult(expected: "bcd", template: "{{ a | append: 'd'}}", localVariables: assigns);
            Helper.AssertTemplateResult(expected: "bcd", template: "{{ a | append: b}}", localVariables: assigns);
            Helper.AssertTemplateResult(expected: "/my/fancy/url.html", template: "{{ '/my/fancy/url' | append: '.html' }}");
            Helper.AssertTemplateResult(expected: "website.com/index.html", template: "{% assign filename = '/index.html' %}{{ 'website.com' | append: filename }}");
        }

        [Fact]
        public void TestPrepend()
        {
            Hash assigns = Hash.FromAnonymousObject(new { a = "bc", b = "a" });
            Helper.AssertTemplateResult("abc", "{{ a | prepend: 'a'}}", assigns);
            Helper.AssertTemplateResult("abc", "{{ a | prepend: b}}", assigns);
        }

        private void TestDividedBy(Context context)
        {
            Helper.AssertTemplateResult(expected: "4", template: "{{ 12 | divided_by:3 }}");
            Helper.AssertTemplateResult(expected: "4", template: "{{ 14 | divided_by:3 }}");
            Helper.AssertTemplateResult(expected: "5", template: "{{ 15 | divided_by:3 }}");
            Assert.Null(StandardFilters.DividedBy(context: context, input: null, operand: 3));
            Assert.Null(StandardFilters.DividedBy(context: context, input: 4, operand: null));

            // Ensure we preserve floating point behavior for division by zero, and don't start throwing exceptions.
            Helper.AssertTemplateResult(expected: double.PositiveInfinity.ToString(), template: "{{ 1.0 | divided_by:0.0 }}");
            Helper.AssertTemplateResult(expected: double.NegativeInfinity.ToString(), template: "{{ -1.0 | divided_by:0.0 }}");
            Helper.AssertTemplateResult(expected: "NaN", template: "{{ 0.0 | divided_by:0.0 }}");
        }

        [Fact]
        public void TestDividedByStringV21()
        {
            var context = _contextV21;
            Helper.AssertTemplateResult(expected: "4", template: "{{ '12' | divided_by: 3 }}");
            Helper.AssertTemplateResult(expected: "4", template: "{{ 12 | divided_by: '3' }}");
            TestDividedBy(context);
        }

        [Fact]
        public void TestInt32DividedByInt64()
        {
            int a = 20;
            long b = 5;
            var c = a / b;
            Assert.Equal(c, (long)4);


            Hash assigns = Hash.FromAnonymousObject(new { a = a, b = b });
            Helper.AssertTemplateResult("4", "{{ a | divided_by:b }}", assigns);
        }

        private void TestModulo(Context context)
        {
            Helper.AssertTemplateResult(expected: "1", template: "{{ 3 | modulo:2 }}");
            Helper.AssertTemplateResult(expected: "7.77", template: "{{ 148387.77 | modulo:10 }}");
            Helper.AssertTemplateResult(expected: "5.32", template: "{{ 3455.32 | modulo:10 }}");
            Helper.AssertTemplateResult(expected: "3.12", template: "{{ 23423.12 | modulo:10 }}");
            Assert.Null(StandardFilters.Modulo(context: context, input: null, operand: 3));
            Assert.Null(StandardFilters.Modulo(context: context, input: 4, operand: null));
        }

        [Fact]
        public void TestModuloStringV21()
        {
            var context = _contextV21;
            Helper.AssertTemplateResult(expected: "1", template: "{{ '3' | modulo: 2 }}");
            Helper.AssertTemplateResult(expected: "1", template: "{{ 3 | modulo: '2' }}");
            TestModulo(context);
        }

        [Fact]
        public void TestUrlencode()
        {
            Assert.Equal("http%3A%2F%2Fdotliquidmarkup.org%2F", StandardFilters.UrlEncode("http://dotliquidmarkup.org/"));
            Assert.Equal("Tetsuro+Takara", StandardFilters.UrlEncode("Tetsuro Takara"));
            Assert.Equal("john%40liquid.com", StandardFilters.UrlEncode("john@liquid.com"));
            Assert.Null(StandardFilters.UrlEncode(null));
        }

        [Fact]
        public void TestUrldecode()
        {
            Assert.Equal("'Stop!' said Fred", StandardFilters.UrlDecode("%27Stop%21%27+said+Fred"));
            Assert.Null(StandardFilters.UrlDecode(null));
        }


        [Fact]
        public void TestDefault()
        {
            Hash assigns = Hash.FromAnonymousObject(new { var1 = "foo", var2 = "bar" });
            Helper.AssertTemplateResult("foo", "{{ var1 | default: 'foobar' }}", assigns);
            Helper.AssertTemplateResult("bar", "{{ var2 | default: 'foobar' }}", assigns);
            Helper.AssertTemplateResult("foobar", "{{ unknownvariable | default: 'foobar' }}", assigns);
        }

        [Fact]
        public void TestCapitalizeV21()
        {
            var context = _contextV21;
            Assert.Null(StandardFilters.Capitalize(context: context, input: null));
            Assert.Equal("", StandardFilters.Capitalize(context: context, input: ""));
            Assert.Equal(" ", StandardFilters.Capitalize(context: context, input: " "));
            Assert.Equal(" My boss is Mr. Doe.", StandardFilters.Capitalize(context: context, input: " my boss is Mr. Doe."));

            Helper.AssertTemplateResult(
                expected: "My great title",
                template: "{{ 'my great title' | capitalize }}");
        }

        [Fact]
        public void TestUniq()
        {
            StandardFilters.Uniq(new string[] { "ants", "bugs", "bees", "bugs", "ants" }).Should().BeEquivalentTo(new[] { "ants", "bugs", "bees" });
            StandardFilters.Uniq(new string[] { }).Should().BeEquivalentTo(new string[] { });
            Assert.Null(StandardFilters.Uniq(null));
            Assert.Equal(new List<object> { 5 }, StandardFilters.Uniq(5));
        }

        [Fact]
        public void TestAbs()
        {
            Assert.Equal(0, StandardFilters.Abs("notNumber"));
            Assert.Equal(10, StandardFilters.Abs(10));
            Assert.Equal(5, StandardFilters.Abs(-5));
            Assert.Equal(19.86, StandardFilters.Abs(19.86));
            Assert.Equal(19.86, StandardFilters.Abs(-19.86));
            Assert.Equal(10, StandardFilters.Abs("10"));
            Assert.Equal(5, StandardFilters.Abs("-5"));
            Assert.Equal(30.60, StandardFilters.Abs("30.60"));
            Assert.Equal(0, StandardFilters.Abs("30.60a"));

            Helper.AssertTemplateResult(
                expected: "17",
                template: "{{ -17 | abs }}");
            Helper.AssertTemplateResult(
                expected: "17",
                template: "{{ 17 | abs }}");
            Helper.AssertTemplateResult(
                expected: "4",
                template: "{{ 4 | abs }}");
            Helper.AssertTemplateResult(
                expected: "19.86",
                template: "{{ '-19.86' | abs }}");
        }

        [Fact]
        public void TestAtLeast()
        {
            Assert.Equal("notNumber", StandardFilters.AtLeast("notNumber", 5));
            Assert.Equal(5, StandardFilters.AtLeast(5, 5));
            Assert.Equal(5, StandardFilters.AtLeast(3, 5));
            Assert.Equal(6, StandardFilters.AtLeast(6, 5));
            Assert.Equal(10, StandardFilters.AtLeast(10, 5));
            Assert.Equal(9.85, StandardFilters.AtLeast(9.85, 5));
            Assert.Equal(5, StandardFilters.AtLeast(3.56, 5));
            Assert.Equal(10, StandardFilters.AtLeast("10", 5));
            Assert.Equal(5, StandardFilters.AtLeast("4", 5));
            Assert.Equal("10a", StandardFilters.AtLeast("10a", 5));
            Assert.Equal("4b", StandardFilters.AtLeast("4b", 5));

            Helper.AssertTemplateResult(
                expected: "5",
                template: "{{ 4 | at_least: 5 }}");
            Helper.AssertTemplateResult(
                expected: "4",
                template: "{{ 4 | at_least: 3 }}");
        }

        [Fact]
        public void TestAtMost()
        {
            Assert.Equal("notNumber", StandardFilters.AtMost("notNumber", 5));
            Assert.Equal(5, StandardFilters.AtMost(5, 5));
            Assert.Equal(3, StandardFilters.AtMost(3, 5));
            Assert.Equal(5, StandardFilters.AtMost(6, 5));
            Assert.Equal(5, StandardFilters.AtMost(10, 5));
            Assert.Equal(5, StandardFilters.AtMost(9.85, 5));
            Assert.Equal(3.56, StandardFilters.AtMost(3.56, 5));
            Assert.Equal(5, StandardFilters.AtMost("10", 5));
            Assert.Equal(4, StandardFilters.AtMost("4", 5));
            Assert.Equal("4a", StandardFilters.AtMost("4a", 5));
            Assert.Equal("10b", StandardFilters.AtMost("10b", 5));

            Helper.AssertTemplateResult(
                expected: "4",
                template: "{{ 4 | at_most: 5 }}");
            Helper.AssertTemplateResult(
                expected: "3",
                template: "{{ 4 | at_most: 3 }}");
        }

        [Fact]
        public void TestCompact()
        {
            StandardFilters.Compact(new string[] { "business", null, "celebrities", null, null, "lifestyle", "sports", null, "technology", null }).Should().BeEquivalentTo(new[] { "business", "celebrities", "lifestyle", "sports", "technology" });
            StandardFilters.Compact(new string[] { "business", "celebrities" }).Should().BeEquivalentTo(new[] { "business", "celebrities" });
            Assert.Equal(new List<object> { 5 }, StandardFilters.Compact(5));
            StandardFilters.Compact(new string[] { }).Should().BeEquivalentTo(new string[] { });
            Assert.Null(StandardFilters.Compact(null));

            var siteAnonymousObject = new
            {
                site = new
                {
                    pages = new[]
                    {
                        new { title = "Shopify", category = "business" },
                        new { title = "Rihanna", category = "celebrities" },
                        new { title = "foo", category = null as string },
                        new { title = "World traveler", category = "lifestyle" },
                        new { title = "Soccer", category = "sports" },
                        new { title = "foo", category = null as string },
                        new { title = "Liquid", category = "technology" },
                    }
                }
            };

            Helper.AssertTemplateResult(
                expected: @"
- business
- celebrities
- 
- lifestyle
- sports
- 
- technology
",
                template: @"{% assign site_categories = site.pages | map: 'category' %}
{% for category in site_categories %}- {{ category }}
{% endfor %}",
                localVariables: Hash.FromAnonymousObject(siteAnonymousObject));

            Helper.AssertTemplateResult(
                expected: @"
- business
- celebrities
- lifestyle
- sports
- technology
",
                template: @"{% assign site_categories = site.pages | map: 'category' | compact %}
{% for category in site_categories %}- {{ category }}
{% endfor %}",
                localVariables: Hash.FromAnonymousObject(siteAnonymousObject));
        }
    }
}
