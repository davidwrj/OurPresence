// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

using OurPresence.Modeller.Liquid.Util;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests.Util
{
    public class ListExtensionMethodsTests
    {
        [Fact]
        public void TestGetAtIndexForNullList()
        {
            var list = (List<string>)null;

            Assert.Null(list.TryGetAtIndex(0));
            Assert.Null(list.TryGetAtIndex(-1));
            Assert.Null(list.TryGetAtIndex(1));
        }

        [Fact]
        public void TestGetAtIndexForEmptyList()
        {
            var list = new List<string>();

            Assert.Null(list.TryGetAtIndex(1));
            Assert.Null(list.TryGetAtIndex(-1));
            Assert.Null(list.TryGetAtIndex(1));
        }

        [Fact]
        public void TestGetAtIndexForNonNullList()
        {
            const string item0 = "foo";
            const string item1 = "bar";
            var list = new List<string> { item0, item1 };

            Assert.Equal(item0, list.TryGetAtIndex(0));
            Assert.Equal(item1, list.TryGetAtIndex(1));
            Assert.Null(list.TryGetAtIndex(2));
            Assert.Null(list.TryGetAtIndex(-1));
        }

        [Fact]
        public void TestGetAtIndexReverseForNullList()
        {
            var list = (List<string>)null;

            Assert.Null(list.TryGetAtIndexReverse(0));
            Assert.Null(list.TryGetAtIndexReverse(-1));
            Assert.Null(list.TryGetAtIndexReverse(1));
        }

        [Fact]
        public void TestGetAtIndexReverseForEmptyList()
        {
            var list = new List<string>();

            Assert.Null(list.TryGetAtIndexReverse(1));
            Assert.Null(list.TryGetAtIndexReverse(-1));
            Assert.Null(list.TryGetAtIndexReverse(1));
        }

        [Fact]
        public void TestGetAtIndexReverseForNonNullList()
        {
            const string item0 = "foo";
            const string item1 = "bar";
            var list = new List<string> { item0, item1 };

            Assert.Equal(item1, list.TryGetAtIndexReverse(0));
            Assert.Equal(item0, list.TryGetAtIndexReverse(1));
            Assert.Null(list.TryGetAtIndexReverse(2));
            Assert.Null(list.TryGetAtIndexReverse(-1));
        }
    }
}