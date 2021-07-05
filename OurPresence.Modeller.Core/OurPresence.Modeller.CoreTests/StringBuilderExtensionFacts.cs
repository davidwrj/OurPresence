// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using System.Text;

namespace OurPresence.Modeller.CoreTests
{
    public class StringBuilderExtensionFacts
    {
        [Theory]
        [InlineData("Id", "ValueId", "Value")]
        [InlineData("Id", "Value Id", "Value ")]
        [InlineData("Id", "ValueIds", "ValueIds")]
        [InlineData(null, "ValueIds", "ValueIds")]
        [InlineData("x", null, "")]
        public void StringBuilderExtension_TrimEnd_RemovesValuesCorrectly(string trim, string value, string expected)
        {
            var sb = new StringBuilder(value);

            sb.TrimEnd(trim);

            sb.ToString().Should().Be(expected);
        }
    }
}
