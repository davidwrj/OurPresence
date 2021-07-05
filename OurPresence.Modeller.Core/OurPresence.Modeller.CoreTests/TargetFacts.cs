// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator;
using FluentAssertions;
using Xunit;

namespace OurPresence.Modeller.CoreTests
{
    public class TargetFacts
    {
        [Fact]
        public void Targets_Default_ReturnsExpectedValue()
        {
            var sut = new Targets();
            Targets.Default.Should().BeOneOf(sut.Supported);
        }

        [Fact]
        public void Targets_Supported_ReturnsExpectedCount()
        {
            var sut = new Targets();
            sut.Supported.Should().HaveCount(3);
        }

        [Fact]
        public void Targets_Shared_ReturnsSameInstance()
        {
            var sut = Targets.Shared;
            var another = Targets.Shared;

            sut.Should().BeSameAs(another);

            sut.RegisterTarget("new One");
            another.Supported.Should().Contain("new one");
        }

        [Fact]
        public void Targets_Reset_ReturnsSupportedToDefault()
        {
            var sut = new Targets();
            sut.RegisterTarget("new");
            sut.Supported.Should().HaveCount(4);

            sut.Reset();

            sut.Supported.Should().HaveCount(3);
        }
    }
}
