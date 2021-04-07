using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class CustomIndexableTests
    {
        #region Test classes
        internal class VirtualList : IIndexable, IEnumerable
        {
            internal VirtualList(params object[] items)
            {
                this.items = items;
            }

            private readonly object[] items;

            public object this[object key]
            {
                get
                {
                    int? index = key as int?;
                    if (!index.HasValue || index.Value < 0 || index.Value >= items.Length) {
                        throw new KeyNotFoundException();
                    }
                    return items[index.Value];
                }
            }

            public bool ContainsKey(object key)
            {
                int? index = key as int?;
                return index.HasValue && index.Value >= 0 && index.Value < items.Length;
            }

            public IEnumerator GetEnumerator()
            {
                return items.GetEnumerator();
            }
        }

        internal class VirtualLongList : IIndexable, IEnumerable
        {
            internal VirtualLongList(params object[] items)
            {
                this.items = items;
            }

            private readonly object[] items;

            public object this[object key]
            {
                get
                {
                    var index = key != null ? (long?)System.Convert.ToInt64(key) : null;
                    if (!index.HasValue || index.Value < 0L || index.Value >= items.Length)
                    {
                        throw new KeyNotFoundException();
                    }
                    return items[index.Value];
                }
            }

            public bool ContainsKey(object key)
            {
                var index = key != null ? (long?)System.Convert.ToInt64(key) : null;
                return index.HasValue && index.Value >= 0L && index.Value < items.Length;
            }

            public IEnumerator GetEnumerator()
            {
                return items.GetEnumerator();
            }
        }

        internal class CustomIndexable : IIndexable, ILiquidizable
        {
            public object this[object key]
            {
                get
                {
                    if (key == null) {
                        return "null";
                    }
                    return key.GetType() + " " + key.ToString();
                }
            }

            public bool ContainsKey(object key)
            {
                return true;
            }

            public object ToLiquid()
            {
                return this;
            }
        }

        internal class OnlyIndexable : IIndexable
        {
            public object this[object key]
            {
                get
                {
                    if (key == null)
                    {
                        return "null";
                    }
                    return key.GetType() + " " + key.ToString();
                }
            }

            public bool ContainsKey(object key)
            {
                return true;
            }
        }

        #endregion

        [Fact]
        public void TestVirtualListLoop()
        {
            string output = Template.Parse("{%for item in list%}{{ item }} {%endfor%}")
                .Render(Hash.FromAnonymousObject(new {list = new VirtualList(1, "Second", 3)}));
            Assert.Equal("1 Second 3 ", output);
        }

        [Fact]
        public void TestVirtualListIntIndex()
        {
            string output = Template.Parse("1: {{ list[0] }}, 2: {{ list[1] }}, 3: {{ list[2] }}")
                .Render(Hash.FromAnonymousObject(new {list = new VirtualList(1, "Second", 3)}));
            Assert.Equal("1: 1, 2: Second, 3: 3", output);
        }

        [Fact]
        public void TestVirtualListLongIndex()
        {
            string output = Template.Parse("1: {{ list[0] }}, 2: {{ list[1] }}, 3: {{ list[2] }}")
                .Render(Hash.FromAnonymousObject(new { list = new VirtualLongList(1L, "Second", 3L) }));
            Assert.Equal("1: 1, 2: Second, 3: 3", output);
        }

        [Fact]
        public void TestCustomIndexableRender()
        {
            Assert.Equal(
                expected: string.Empty,
                actual: Template
                    .Parse("{{container}}")
                    .Render(Hash.FromAnonymousObject(new { container = new CustomIndexable() })));
        }

        [Fact]
        public void TestOnlyIndexableRender()
        {
            Assert.Equal(
                expected: "Liquid syntax error: Object 'OurPresence.Modeller.Liquid.Tests.CustomIndexableTests+OnlyIndexable' is invalid because it is neither a built-in type nor implements ILiquidizable",
                actual: Template
                    .Parse("{{container}}")
                    .Render(Hash.FromAnonymousObject(new { container = new OnlyIndexable() })));
        }

        [Fact]
        public void TestCustomIndexableIntKeys()
        {
            string output = Template.Parse("1: {{container[0]}}, 2: {{container[1]}}")
                .Render(Hash.FromAnonymousObject(new {container = new CustomIndexable()}));
            Assert.Equal("1: System.Int32 0, 2: System.Int32 1", output);
        }

        [Fact]
        public void TestCustomIndexableLongKeys()
        {
            string output = Template.Parse("1: {{container[2147483648]}}, 2: {{container[2999999999]}}")
                .Render(Hash.FromAnonymousObject(new { container = new CustomIndexable() }));
            Assert.Equal("1: System.Int64 2147483648, 2: System.Int64 2999999999", output);
        }

        [Fact]
        public void TestCustomIndexableStringKeys() {
            string output = Template.Parse("abc: {{container.abc}}")
                .Render(Hash.FromAnonymousObject(new {container = new CustomIndexable()}));
            Assert.Equal("abc: System.String abc", output);
        }
    }
}