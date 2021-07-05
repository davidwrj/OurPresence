// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;
using FluentAssertions;
using OurPresence.Modeller.Domain.Extensions;

namespace OurPresence.Modeller.CoreTests
{
    public class NameExtensionFacts
    {
        [Theory]
        [InlineData("Decimal", "@Decimal")]
        [InlineData("decimal", "@decimal")]
        [InlineData("MyValue", "MyValue")]
        [InlineData("values", "values")]
        public void NameService_FieldName_PrefixesReservedWords(string name, string expected)
        {
            var newName = name.CheckKeyword();
            newName.Should().Be(expected);
        }
    }
}
