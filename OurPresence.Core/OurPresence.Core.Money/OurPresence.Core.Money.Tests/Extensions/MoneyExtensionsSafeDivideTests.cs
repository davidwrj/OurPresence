// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using OurPresence.Core.Money.Extensions;

namespace OurPresence.Core.Money.Tests.Extensions
{
    public class MoneyExtensionsSafeDivideTests
    {
        public class GivenIWantToSafelyDivideMoney
        {
            private readonly Amount _eurocent5 = new(0.05m, "EUR");
            private readonly Amount _euro1 = new(1.0m, "EUR");

            [Fact]
            public void WhenDividing5CentsByTwo_ThenDivisionShouldNotLoseCents()
            {
                // Foemmel's Conundrum test (see PEAA, page 495)
                // http://thierryroussel.free.fr/java/books/martinfowler/www.martinfowler.com/isa/money.html

                var enumerable = _eurocent5.SafeDivide(2).ToList();

                enumerable.Should().NotBeNullOrEmpty()
                    .And.HaveCount(2)
                    .And.ContainItemsAssignableTo<Amount>()
                    .And.ContainInOrder(new[]
                    { 
                        new Amount(0.02m, "EUR"),
                        new Amount(0.03m, "EUR")
                    });

                enumerable.Sum(m => m.Value).Should().Be(_eurocent5.Value);
            }

            [Fact]
            public void WhenDividing5CentsByThree_ThenDivisionShouldNotLoseCents()
            {
                var enumerable = _eurocent5.SafeDivide(3).ToList();

                enumerable.Should().NotBeNullOrEmpty()
                    .And.HaveCount(3)
                    .And.ContainItemsAssignableTo<Amount>()
                    .And.ContainInOrder(new[]
                    {
                        new Amount(0.02m, "EUR"),
                        new Amount(0.02m, "EUR"),
                        new Amount(0.01m, "EUR")
                    });

                enumerable.Sum(m => m.Value).Should().Be(_eurocent5.Value);
            }

            [Fact]
            public void WhenDividing1EuroByGivenRatios_ThenDivisionShouldNotLoseCents()
            {             
                var enumerable = _euro1.SafeDivide(new[] { 2, 3, 3 }).ToList();

                enumerable.Should().NotBeNullOrEmpty()
                    .And.HaveCount(3)
                    .And.ContainItemsAssignableTo<Amount>()
                    .And.ContainInOrder(new[]
                    {
                        new Amount(0.25m, "EUR"),
                        new Amount(0.38m, "EUR"),
                        new Amount(0.37m, "EUR")
                    });

                enumerable.Sum(m => m.Value).Should().Be(_euro1.Value);
            }

            [Fact]
            public void WhenDividing1EuroByGivenRatiosWhereOneIsVerySmall_ThenDivisionShouldNotLoseCents()
            {
                var enumerable = _euro1.SafeDivide(new[] { 200, 300, 1 }).ToList();

                enumerable.Should().NotBeNullOrEmpty()
                    .And.HaveCount(3)
                    .And.ContainItemsAssignableTo<Amount>()
                    .And.ContainInOrder(new[]
                    {
                        new Amount(0.40m, "EUR"),
                        new Amount(0.60m, "EUR"),
                        new Amount(0.0m, "EUR")
                    });

                enumerable.Sum(m => m.Value).Should().Be(_euro1.Value);
            }

            [Fact]
            public void WhenDividingByMinus1_ThenAnExceptionShouldBeThrowed()
            {               
                Action action = () => _euro1.SafeDivide(-1).ToList();

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void WhenDividingByGivenRatiosWithMinus1_ThenAnExceptionShouldBeThrowed()
            {
                Action action = () => _euro1.SafeDivide(new[] { 2, -1, 3 }).ToList();

                action.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("*1*");
            }
        }
    }
}
