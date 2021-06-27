// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class LazyHashTest
    {

        public class LazyHash : Hash
        {
            #region Fields

            private Lazy<Dictionary<string, PropertyInfo>> lazyProperties = null;
            private Dictionary<string, PropertyInfo> PropertyInfos => lazyProperties.Value;


            private object ObjectWithLazyProperty { get; set; }

            #endregion

            #region Constructors
            
            public LazyHash(object bo)
            {
                ObjectWithLazyProperty = bo;
                Initialize(bo);
            }
            
            private void Initialize(object bo)
            {
                lazyProperties = new Lazy<Dictionary<string, PropertyInfo>>(delegate ()
                {
                    var boProperties = new Dictionary<string, PropertyInfo>();
                    foreach (var pi in bo.GetType().GetProperties())
                    {
                        if (!boProperties.ContainsKey(pi.Name.ToLower()))
                        {
                            boProperties.Add(pi.Name.ToLower(), pi);
                        }
                    }
                    return boProperties;
                });

            }
            
            #endregion

            protected override object GetValue(string key)
            {
                if (PropertyInfos.ContainsKey(key.ToLower()))
                {
                    return PropertyInfos[key.ToLower()].GetValue(ObjectWithLazyProperty, null);
                }
                return base.GetValue(key);
            }
            
            public override bool Contains(object key)
            {
                var dicKey = key.ToString().ToLower();
                if (PropertyInfos.ContainsKey(dicKey))
                    return true;
                return base.Contains(key);
            }
        }



        public class TestLazyObject {
            public Lazy<string> _lazyProperty1 => new Lazy<string>(() =>
            {
                return "LAZY_PROPERTY_1";
            });
            public string LazyProperty1 => _lazyProperty1.Value;

            public Lazy<string> _lazyProperty2 => new Lazy<string>(() =>
            {
                return "LAZY_PROPERTY_2";
            });
            public string LazyProperty2 => _lazyProperty2.Value;

            public string StaticProperty => "STATIC_PROPERTY";
        }

        [Fact]
        public void TestLazyHashProperty1WithoutAccessingProperty2()
        {
            var lazyObject = new TestLazyObject();
            Template template = Template.Parse("{{LazyProperty1}}");
            var output = template.Render(new LazyHash(lazyObject));
            Assert.Equal("LAZY_PROPERTY_1", output);
            Assert.False(lazyObject._lazyProperty2.IsValueCreated, "LazyObject LAZY_PROPERTY_2 has been created");
        }

        [Fact]
        public void TestLazyHashProperty2WithoutAccessingProperty1()
        {
            var lazyObject = new TestLazyObject();
            Template template = Template.Parse("{{LazyProperty2}}");
            var output = template.Render(new LazyHash(lazyObject));
            Assert.Equal("LAZY_PROPERTY_2", output);
            Assert.False(lazyObject._lazyProperty1.IsValueCreated, "LazyObject LAZY_PROPERTY_1 has been created");
        }

        [Fact]
        public void TestLazyHashWithoutAccessingAny()
        {
            var lazyObject = new TestLazyObject();
            Template template = Template.Parse("{{StaticProperty}}");
            var output = template.Render(new LazyHash(lazyObject));
            Assert.Equal("STATIC_PROPERTY", output);
            Assert.False(lazyObject._lazyProperty1.IsValueCreated, "LazyObject LAZY_PROPERTY_1 has been created");
            Assert.False(lazyObject._lazyProperty2.IsValueCreated, "LazyObject LAZY_PROPERTY_2 has been created");
        }

        [Fact]
        public void TestLazyHashWithAccessingAllProperties()
        {
            var lazyObject = new TestLazyObject();
            Template template = Template.Parse("{{LazyProperty1}}-{{LazyProperty2}}-{{StaticProperty}}");
            var output = template.Render(new LazyHash(lazyObject));
            Assert.Equal($"LAZY_PROPERTY_1-LAZY_PROPERTY_2-STATIC_PROPERTY", output);
        }
    }
}
