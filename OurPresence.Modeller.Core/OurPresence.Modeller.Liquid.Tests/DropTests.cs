// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class DropTests
    {
        #region Classes used in tests

        internal class NullDrop : Drop
        {
            public NullDrop(Template template) : base(template)
            {
            }

            public override object BeforeMethod(string method)
            {
                return null;
            }
        }

        internal class ContextDrop : Drop
        {
            public ContextDrop(Template template) : base(template)
            {
            }

            public int Scopes => Context.Scopes.Count();

            public IEnumerable<int> ScopesAsArray
            {
                get { return Enumerable.Range(1, Context.Scopes.Count()); }
            }

            public int LoopPos
            {
                get { return (int)Context["forloop.index"]; }
            }

            public void Break()
            {
                Debugger.Break();
            }

            public override object BeforeMethod(string method)
            {
                return Context[method];
            }
        }

        internal class ProductDrop : Drop
        {
            public ProductDrop(Template template) : base(template)
            {
            }

            internal class ComplexDrop : Drop
            {
                public ComplexDrop(Template template) : base(template)
                {
                }

                public TextDrop[] ArrayOfDrops
                {
                    get { return new[] { new TextDrop(base.Template), new TextDrop(base.Template) }; }
                }

                public TextDrop SingleDrop
                {
                    get { return new TextDrop(base.Template); }
                }
            }

            internal class TextDrop : Drop
            {
                public TextDrop(Template template) : base(template)
                {
                }

                public string[] Array
                {
                    get { return new[] { "text1", "text2" }; }
                }

                public List<string> List
                {
                    get { return new List<string>(new[] { "text1", "text2" }); }
                }

                public string Text
                {
                    get { return "text1"; }
                }
            }

            internal class CatchallDrop : Drop
            {
                public CatchallDrop(Template template) : base(template)
                {
                }

                public override object BeforeMethod(string method)
                {
                    return "method: " + method;
                }
            }

            public TextDrop Texts()
            {
                return new TextDrop(base.Template);
            }

            public ComplexDrop Complex()
            {
                return new ComplexDrop(base.Template);
            }

            public CatchallDrop Catchall()
            {
                return new CatchallDrop(base.Template);
            }

            public new ContextDrop Context
            {
                get { return new ContextDrop(base.Template); }
            }

            protected string CallMeNot()
            {
                return "protected";
            }
        }

        internal class EnumerableDrop : Drop, IEnumerable
        {
            public EnumerableDrop(Template template) : base(template)
            {
            }

            public int Size
            {
                get { return 3; }
            }

            public IEnumerator GetEnumerator()
            {
                yield return 1;
                yield return 2;
                yield return 3;
            }
        }

#if !CORE
        internal class DataRowDrop : Drop
        {
            private readonly System.Data.DataRow _dataRow;

            public DataRowDrop(Template template, System.Data.DataRow dataRow) : base(template)
            {
                _dataRow = dataRow;
            }

            public override object BeforeMethod(string method)
            {
                if(_dataRow.Table.Columns.Contains(method))
                    return _dataRow[method];
                return null;
            }
        }
#endif

        internal class CamelCaseDrop : Drop
        {
            public CamelCaseDrop(Template template) : base(template)
            {
            }

            public int ProductID
            {
                get { return 1; }
            }
        }

        internal static class ProductFilter
        {
            public static string ProductText(object input)
            {
                return ((ProductDrop)input).Texts().Text;
            }
        }

        #endregion

        [Fact]
        public void TestProductDrop()
        {
            Action action = () =>
            {
                Template tpl = Template.Parse("  ");
                tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }));
            };
            action.Should().NotThrow();
        }

        [Fact]
        public void TestDropDoesNotOutputItself()
        {
            var tpl = Template.Parse(" {{ product }} ");
            var output = tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }));
            Assert.Equal("  ", output);
        }

        [Fact]
        public void TestDropWithFilters()
        {
            var tpl = Template.Parse(" {{ product | product_text }} ");
            var output = tpl.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }),
                Filters = new[] { typeof(ProductFilter) }
            });
            Assert.Equal(" text1 ", output);
        }

        [Fact]
        public void TestTextDrop()
        {
            var tpl = Template.Parse(" {{ product.texts.text }} ");
            var output = tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }));
            Assert.Equal(" text1 ", output);
        }

        [Fact]
        public void TestTextDrop2()
        {
            var tpl = Template.Parse(" {{ product.catchall.unknown }} ");
            var output = tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }));
            Assert.Equal(" method: unknown ", output);
        }

        [Fact]
        public void TestTextArrayDrop()
        {
            var tpl1 = Template.Parse("{{product.texts.array}}");
            var actual1 = tpl1.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl1) }));
            Assert.Equal("text1text2", actual1);

            var tpl2 = Template.Parse("{% for text in product.texts.array %} {{text}} {% endfor %}");
            var actual2 = tpl2.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl2) }));
            Assert.Equal(" text1  text2 ", actual2);
        }

        [Fact]
        public void TestTextListDrop()
        {
            var tpl1 = Template.Parse("{{product.texts.list}}");
            var actual1 = tpl1.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl1) }));
            Assert.Equal("text1text2", actual1);

            var tpl2 = Template.Parse("{% for text in product.texts.list %} {{text}} {% endfor %}");
            var actual2 = tpl2
                .Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl2) }));
            Assert.Equal(" text1  text2 ", actual2);
        }

        //[Fact]
        //public void TestComplexDrop()
        //{
        //    // Drop objects do not output themselves.
        //    Assert.Equal(
        //        expected: string.Empty,
        //        actual: Template
        //            .Parse("{{ product.complex.single_drop }}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));

        //    // A complex drop object is still a drop object hence does not output oneself.
        //    Assert.Equal(
        //        expected: string.Empty,
        //        actual: Template
        //            .Parse("{{ product.complex }}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));

        //    // Public properties within complex drop object do render when exactly accessed
        //    Assert.Equal(
        //        expected: "text1",
        //        actual: Template
        //            .Parse("{{ product.complex.single_drop.text }}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));

        //    // While arrays are supported for render, when the array content is of drop object type, the rendering of each object is still empty.
        //    Assert.Equal(
        //        expected: string.Empty,
        //        actual: Template
        //            .Parse("{% for text in product.complex.array_of_drops %}{{text}}{% endfor %}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));

        //    // We can still iterate through an array of drop objects then access the public properties of said object
        //    Assert.Equal(
        //        expected: "text1text1",
        //        actual: Template
        //            .Parse("{% for text in product.complex.array_of_drops %}{{text.text}}{% endfor %}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));

        //    // The array of drop objects may itself contain a property of type array which can be rendered
        //    Assert.Equal(
        //        expected: "text1text2text1text2",
        //        actual: Template
        //            .Parse("{% for text in product.complex.array_of_drops %}{{text.array}}{% endfor %}")
        //            .Render(Hash.FromAnonymousObject(new { product = new ProductDrop() })));
        //}

        [Fact]
        public void TestContextDrop()
        {
            var tpl = Template.Parse(" {{ context.bar }} ");
            string output = tpl.Render(Hash.FromAnonymousObject(new { context = new ContextDrop(tpl), bar = "carrot" }));
            Assert.Equal(" carrot ", output);
        }

        [Fact]
        public void TestNestedContextDrop()
        {
            var tpl = Template.Parse(" {{ product.context.foo }} ");
            string output = tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl), foo = "monkey" }));
            Assert.Equal(" monkey ", output);
        }

        [Fact]
        public void TestProtected()
        {
            var tpl = Template.Parse(" {{ product.call_me_not }} ");
            string output = tpl.Render(Hash.FromAnonymousObject(new { product = new ProductDrop(tpl) }));
            Assert.Equal("  ", output);
        }

        //[Fact]
        //public void TestScope()
        //{
        //    Assert.Equal("1", Template.Parse("{{ context.scopes }}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop() })));
        //    Assert.Equal("2", Template.Parse("{%for i in dummy%}{{ context.scopes }}{%endfor%}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //    Assert.Equal("3", Template.Parse("{%for i in dummy%}{%for i in dummy%}{{ context.scopes }}{%endfor%}{%endfor%}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //}

        //[Fact]
        //public void TestScopeThroughProc()
        //{
        //    Assert.Equal("1", Template.Parse("{{ s }}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), s = (Proc)(c => c["context.scopes"]) })));
        //    Assert.Equal("2", Template.Parse("{%for i in dummy%}{{ s }}{%endfor%}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), s = (Proc)(c => c["context.scopes"]), dummy = new[] { 1 } })));
        //    Assert.Equal("3", Template.Parse("{%for i in dummy%}{%for i in dummy%}{{ s }}{%endfor%}{%endfor%}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), s = (Proc)(c => c["context.scopes"]), dummy = new[] { 1 } })));
        //}

        //[Fact]
        //public void TestScopeWithAssigns()
        //{
        //    Assert.Equal("variable", Template.Parse("{% assign a = 'variable'%}{{a}}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop() })));
        //    Assert.Equal("variable", Template.Parse("{% assign a = 'variable'%}{%for i in dummy%}{{a}}{%endfor%}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //    Assert.Equal("test", Template.Parse("{% assign header_gif = \"test\"%}{{header_gif}}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop() })));
        //    Assert.Equal("test", Template.Parse("{% assign header_gif = 'test'%}{{header_gif}}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop() })));
        //}

        //[Fact]
        //public void TestScopeFromTags()
        //{
        //    Assert.Equal("1", Template.Parse("{% for i in context.scopes_as_array %}{{i}}{% endfor %}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //    Assert.Equal("12", Template.Parse("{%for a in dummy%}{% for i in context.scopes_as_array %}{{i}}{% endfor %}{% endfor %}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //    Assert.Equal("123", Template.Parse("{%for a in dummy%}{%for a in dummy%}{% for i in context.scopes_as_array %}{{i}}{% endfor %}{% endfor %}{% endfor %}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1 } })));
        //}

        //[Fact]
        //public void TestAccessContextFromDrop()
        //{
        //    Assert.Equal("123", Template.Parse("{% for a in dummy %}{{ context.loop_pos }}{% endfor %}").Render(Hash.FromAnonymousObject(new { context = new ContextDrop(), dummy = new[] { 1, 2, 3 } })));
        //}

        //[Fact]
        //public void TestEnumerableDrop()
        //{
        //    Assert.Equal("123", Template.Parse("{% for c in collection %}{{c}}{% endfor %}").Render(Hash.FromAnonymousObject(new { collection = new EnumerableDrop() })));
        //}

        //[Fact]
        //public void TestEnumerableDropSize()
        //{
        //    Assert.Equal("3", Template.Parse("{{collection.size}}").Render(Hash.FromAnonymousObject(new { collection = new EnumerableDrop() })));
        //}

        //[Fact]
        //public void TestNullCatchAll()
        //{
        //    Assert.Equal("", Template.Parse("{{ nulldrop.a_method }}").Render(Hash.FromAnonymousObject(new { nulldrop = new NullDrop() })));
        //}

#if !CORE
        [Fact]
        public void TestDataRowDrop()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Column1");
            dataTable.Columns.Add("Column2");

            System.Data.DataRow dataRow = dataTable.NewRow();
            dataRow["Column1"] = "Hello";
            dataRow["Column2"] = "World";

            Template tpl = Template.Parse(" {{ row.column1 }} ");
            Assert.Equal(" Hello ", tpl.Render(Hash.FromAnonymousObject(new { row = new DataRowDrop(tpl,dataRow) })));
        }
#endif

        //[Fact]
        //public void TestRubyNamingConventionPrintsHelpfulErrorIfMissingPropertyWouldMatchCSharpNamingConvention()
        //{
        //    Helper.AssertTemplateResult(
        //        expected: "Missing property. Did you mean 'product_id'?",
        //        template: "{{ value.ProductID }}",
        //        anonymousObject: new { value = new CamelCaseDrop() },
        //        namingConvention: new RubyNamingConvention());
        //}
    }
}
