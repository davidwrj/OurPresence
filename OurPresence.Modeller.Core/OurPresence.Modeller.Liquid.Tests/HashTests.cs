// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace OurPresence.Modeller.Liquid.Tests.Ns1
{
    public class TestClass
    {
        public string TestClassProp1 { get; set; }
    }
}

namespace OurPresence.Modeller.Liquid.Tests.Ns2
{
    public class TestClass
    {
        public string TestClassProp2 { get; set; }
    }
}


namespace OurPresence.Modeller.Liquid.Tests
{
    public class HashTests
    {
        public class TestBaseClass
        {
            public string TestBaseClassProp { get; set; }
        }

        public class TestClass : TestBaseClass
        {
            public string TestClassProp { get; set; }
        }

        #region Mapper Cache Tests
        /// <summary>
        /// "mapperCache" should consider namespace.
        /// Types with same name (but different namespace) should be cached separately
        /// </summary>
        [Fact]
        public void TestMapperCacheShouldCacheSeperateNamespaces()
        {
            var testClass1 = new Ns1.TestClass()
            {
                TestClassProp1 = "TestClassProp1Value"
            };

            var value1 = Hash.FromAnonymousObject(testClass1);

            Assert.Equal(
                testClass1.TestClassProp1,
                value1[nameof(OurPresence.Modeller.Liquid.Tests.Ns1.TestClass.TestClassProp1)]);

            //Same type name but different namespace
            var testClass2 = new Ns2.TestClass()
            {
                TestClassProp2 = "TestClassProp2Value"
            };
            var value2 = Hash.FromAnonymousObject(testClass2);

            Assert.Equal(
                testClass2.TestClassProp2,
                value2[nameof(OurPresence.Modeller.Liquid.Tests.Ns2.TestClass.TestClassProp2)]);
        }

        #endregion

        #region Including Base Class Properties Tests

        private void IncludeBaseClassPropertiesOrNot(bool includeBaseClassProperties)
        {
            var TestClassPropValue = "TestClassPropValueValue";
            var TestBaseClassPropValue = "TestBaseClassPropValue";

            var value = Hash.FromAnonymousObject(new TestClass()
            {
                TestClassProp = TestClassPropValue,
                TestBaseClassProp = TestBaseClassPropValue
            }, includeBaseClassProperties);

            Assert.Equal(
                TestClassPropValue,
                value[nameof(TestClass.TestClassProp)]);

            Assert.Equal(
                includeBaseClassProperties ? TestBaseClassPropValue :  null,
                value[nameof(TestClass.TestBaseClassProp)]);
        }

        /// <summary>
        /// Mapping without properties from base class 
        /// </summary>
        [Fact]
        public void TestShouldNotMapPropertiesFromBaseClass()
        {
            IncludeBaseClassPropertiesOrNot(includeBaseClassProperties : false);
        }

        /// <summary>
        /// Mapping with properties from base class 
        /// </summary>
        [Fact]
        public void TestShouldMapPropertiesFromBaseClass()
        {
            IncludeBaseClassPropertiesOrNot(includeBaseClassProperties : true);
        }

        /// <summary>
        /// Mapping/Not mapping properties from base class should work for same class. 
        /// "mapperCache" should consider base class property mapping option ("includeBaseClassProperties").
        /// </summary>
        [Fact]
        public void TestUpperTwoScenarioWithSameClass()
        {
            //These two need to be called together to be sure same cache is being used for two  
            IncludeBaseClassPropertiesOrNot(false);
            IncludeBaseClassPropertiesOrNot(true);
        }
        #endregion
    }
}
