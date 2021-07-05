// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using OurPresence.Core.Money.Tests.Helpers;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace OurPresence.Core.Money.Tests.AmountBinaryOperatorsSpec
{
    public class GivenIWantToAddAndSubstractAmount
    {
        private readonly ITestOutputHelper _output;

        public GivenIWantToAddAndSubstractAmount(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] { 101m, 99m, 200m }, // whole numbers
            new object[] { 100m, 0.01m, 100.01m }, // fractions
            new object[] { 100.999m, 0.9m, 101.899m }, // overflow 
            new object[] { 100.5m, 0.9m, 101.4m }, // overflow
            new object[] { 100.999m, -0.9m, 100.099m }, // negative
            new object[] { -100.999m, -0.9m, -101.899m } // negative
        };

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingAdditionOperator_ThenAmountShouldBeAdded(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = money1 + money2;

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money1);
            result.Should().NotBeSameAs(money2);
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingAdditionMethod_ThenAmountShouldBeAdded(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = Amount.Add(money1, money2);

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money1);
            result.Should().NotBeSameAs(money2);
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingSubstractionOperator_ThenAmountShouldBeSubtracted(decimal expected, decimal value2, decimal value1)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = money1 - money2;

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money1);
            result.Should().NotBeSameAs(money2);
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingSubstractionMethod_ThenAmountShouldBeSubtracted(decimal expected, decimal value2, decimal value1)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = Amount.Subtract(money1, money2);

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money1);
            result.Should().NotBeSameAs(money2);
        }

        [Theory, MemberData(nameof(TestData))]
        [UseCulture("en-us")]
        public void WhenUsingAddtionOperatorWithDecimal_ThenAmountShouldBeAdded(decimal value1, decimal value2, decimal expected)
        {
            _output.WriteLine($"en-US : {CultureInfo.CurrentCulture.Name},{CultureInfo.CurrentUICulture.Name}");
            var money1 = new Amount(value1, "EUR");

            var result1 = money1 + value2;
            var result2 = value2 + money1;

            result1.Should().Be(new Amount(expected, "EUR"));
            result1.Should().NotBeSameAs(money1);
            result2.Should().Be(new Amount(expected, "EUR"));
            result2.Should().NotBeSameAs(money1);
        }

        [Theory, MemberData(nameof(TestData))]
        [UseCulture("en-us")]
        public void WhenUsingAdditionMethodWithDecimal_ThenAmountShouldBeAdded(decimal value1, decimal value2, decimal expected)
        {
            _output.WriteLine($"en-US : {CultureInfo.CurrentCulture.Name},{CultureInfo.CurrentUICulture.Name}");
            var money1 = new Amount(value1, "EUR");

            var result = Amount.Add(money1, value2);

            result.Should().Be(new Amount(expected, "EUR"));
            result.Should().NotBeSameAs(money1);
        }

        [Theory, MemberData(nameof(TestData))]
        [UseCulture("en-us")]
        public void WhenUsingSubstractionOperatorWithDecimal_ThenAmountShouldBeAdded(decimal expected, decimal value2, decimal value1)
        {
            _output.WriteLine($"en-US : {CultureInfo.CurrentCulture.Name},{CultureInfo.CurrentUICulture.Name}");
            var money1 = new Amount(value1, "EUR");

            var result1 = money1 - value2;
            var result2 = value2 - money1;

            result1.Should().Be(new Amount(expected, "EUR"));
            result1.Should().NotBeSameAs(money1);
            result2.Should().Be(new Amount(expected, "EUR"));
            result2.Should().NotBeSameAs(money1);
        }

        [Theory, MemberData(nameof(TestData))]
        [UseCulture("en-us")]
        public void WhenUsingSubstractionMethodWithDecimal_ThenAmountShouldBeSubtracted(decimal expected, decimal value2, decimal value1)
        {
            _output.WriteLine($"en-US : {CultureInfo.CurrentCulture.Name},{CultureInfo.CurrentUICulture.Name}");
            var money1 = new Amount(value1, "EUR");

            var result = Amount.Subtract(money1, value2);

            result.Should().Be(new Amount(expected, "EUR"));
            result.Should().NotBeSameAs(money1);
        }

        [Theory, MemberData(nameof(TestData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public void WhenUsingAdditionOperatorWithDifferentCurrency_ThenThrowException(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1, "EUR");
            var money2 = new Amount(value2, "USD");

            Action action = () => { _ = money1 + money2; };

            action.Should().Throw<InvalidCurrencyException>().WithMessage("The requested operation expected the currency*");
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingAdditionMethodWithDifferentCurrency_ThenThrowException(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1, "EUR");
            var money2 = new Amount(value2, "USD");

            Action action = () => Amount.Add(money1, money2);

            action.Should().Throw<InvalidCurrencyException>().WithMessage("The requested operation expected the currency*");
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingSubstractionOperatorWithDifferentCurrency_ThenThrowException(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1, "EUR");
            var money2 = new Amount(value2, "USD");

            Action action = () => { _ = money1 - money2; };

            action.Should().Throw<InvalidCurrencyException>().WithMessage("The requested operation expected the currency*");
        }

        [Theory, MemberData(nameof(TestData))]
        public void WhenUsingSubstractionMethodWithDifferentCurrency_ThenThrowException(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1, "EUR");
            var money2 = new Amount(value2, "USD");

            Action action = () => Amount.Subtract(money1, money2);

            action.Should().Throw<InvalidCurrencyException>().WithMessage("The requested operation expected the currency*");
        }
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    }

    public class GivenIWantToMultiplyAndDivideAmount
    {
        public static IEnumerable<object[]> TestDataDecimal => new[]
        {
            new object[] { 10m, 15m, 150m },
            new object[] { 100.12m, 0.5m, 50.06m },
            new object[] { 100.12m, 5m, 500.60m },
            new object[] { -100.12m, 0.5m, -50.06m },
            new object[] { -100.12m, 5m, -500.60m }
        };

        [Theory, MemberData(nameof(TestDataDecimal))]
        public void WhenUsingMultiplyOperatorWithDecimal_ThenAmountShouldBeMultipled(decimal value, decimal multiplier, decimal expected)
        {
            var money = new Amount(value);

            var result1 = money * multiplier;
            var result2 = multiplier * money;

            result1.Should().Be(new Amount(expected));
            result1.Should().NotBeSameAs(money);
            result2.Should().Be(new Amount(expected));
            result2.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataDecimal))]
        public void WhenUsingMultiplyMethodWithDecimal_ThenAmountShouldBeMultipled(decimal value, decimal multiplier, decimal expected)
        {
            var money = new Amount(value);

            var result = Amount.Multiply(money, multiplier);

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataDecimal))]
        public void WhenUsingDivisionOperatorWithDecimal_ThenAmountShouldBeDivided(decimal expected, decimal divider, decimal value)
        {
            var money = new Amount(value);

            var result = money / divider;

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataDecimal))]
        public void WhenUsingDivisionMethodWithDecimal_ThenAmountShouldBeDivided(decimal expected, decimal divider, decimal value)
        {
            var money = new Amount(value);

            var result = Amount.Divide(money, divider);

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money);
        }

        public static IEnumerable<object[]> TestDataInteger => new[]
        {
            new object[] { 10m, 15, 150 },
            new object[] { 100.12m, 3, 300.36m },
            new object[] { 100.12m, 5, 500.60m },
            new object[] { -100.12m, 3, -300.36m },
            new object[] { -100.12m, 5, -500.60m }
        };

        [Theory, MemberData(nameof(TestDataInteger))]
        public void WhenUsingMultiplyOperatorWithInteger_ThenAmountShouldBeMultipled(decimal value, int multiplier, decimal expected)
        {
            var money = new Amount(value);

            var result1 = money * multiplier;
            var result2 = multiplier * money;

            result1.Should().Be(new Amount(expected));
            result1.Should().NotBeSameAs(money);
            result2.Should().Be(new Amount(expected));
            result2.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataInteger))]
        public void WhenUsingMultiplyMethodWithInteger_ThenAmountShouldBeMultipled(decimal value, int multiplier, decimal expected)
        {
            var currency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            var money = new Amount(value, currency);

            var result = Amount.Multiply(money, multiplier);

            result.Should().Be(new Amount(expected, currency));
            result.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataInteger))]
        public void WhenUsingDivisionOperatorWithInteger_ThenAmountShouldBeDivided(decimal expected, int divider, decimal value)
        {
            var money = new Amount(value);

            var result = money / divider;

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money);
        }

        [Theory, MemberData(nameof(TestDataInteger))]
        public void WhenUsingDivisionMethodWithInteger_ThenAmountShouldBeDivided(decimal expected, int divider, decimal value)
        {
            var money = new Amount(value);

            var result = Amount.Divide(money, divider);

            result.Should().Be(new Amount(expected));
            result.Should().NotBeSameAs(money);
        }

        public static IEnumerable<object[]> TestDataAmount => new[]
        {
            new object[] { 150m, 15m, 10m },
            new object[] { 100.12m, 3m, 100.12m/3m },
        };

        [Theory, MemberData(nameof(TestDataAmount))]
        public void WhenUsingDivisionOperatorWithAmount_ThenResultShouldBeRatio(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = money1 / money2;

            result.Should().Be(expected); // ratio
        }

        [Theory, MemberData(nameof(TestDataAmount))]
        public void WhenUsingDivisionMethodWithAmount_ThenResultShouldBeRatio(decimal value1, decimal value2, decimal expected)
        {
            var money1 = new Amount(value1);
            var money2 = new Amount(value2);

            var result = Amount.Divide(money1, money2);

            result.Should().Be(expected); // ratio
        }
    }
}
