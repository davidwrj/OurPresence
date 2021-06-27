// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class StatementTests
    {
        [Fact]
        public void TestTrueEqlTrue()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if true == true %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestTrueNotEqlTrue()
        {
            Assert.Equal("  false  ", Template.Parse(" {% if true != true %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestTrueLqTrue()
        {
            Assert.Equal("  false  ", Template.Parse(" {% if 0 > 0 %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestOneLqZero()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 1 > 0 %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestZeroLqOne()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 0 < 1 %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestZeroLqOrEqualOne()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 0 <= 0 %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestZeroLqOrEqualOneInvolvingNil()
        {
            Assert.Equal("  false  ", Template.Parse(" {% if null <= 0 %} true {% else %} false {% endif %} ").Render());
            Assert.Equal("  false  ", Template.Parse(" {% if 0 <= null %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestZeroLqqOrEqualOne()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 0 >= 0 %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestStrings()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 'test' == 'test' %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestStringsNotEqual()
        {
            Assert.Equal("  false  ", Template.Parse(" {% if 'test' != 'test' %} true {% else %} false {% endif %} ").Render());
        }

        [Fact]
        public void TestVarAndStringEqual()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if var == 'hello there!' %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = "hello there!" })));
        }

        [Fact]
        public void TestVarAndStringAreEqualBackwards()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if 'hello there!' == var %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = "hello there!" })));
        }

        [Fact]
        public void TestIsCollectionEmpty()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if array == empty %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { array = new object[] { } })));
        }

        [Fact]
        public void TestIsNotCollectionEmpty()
        {
            Assert.Equal("  false  ", Template.Parse(" {% if array == empty %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { array = new[] { 1, 2, 3 } })));
        }

        [Fact]
        public void TestNil()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if var == nil %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = (object) null })));
            Assert.Equal("  true  ", Template.Parse(" {% if var == null %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = (object) null })));
        }

        [Fact]
        public void TestNotNil()
        {
            Assert.Equal("  true  ", Template.Parse(" {% if var != nil %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = 1 })));
            Assert.Equal("  true  ", Template.Parse(" {% if var != null %} true {% else %} false {% endif %} ").Render(Hash.FromAnonymousObject(new { var = 1 })));
        }
    }
}
