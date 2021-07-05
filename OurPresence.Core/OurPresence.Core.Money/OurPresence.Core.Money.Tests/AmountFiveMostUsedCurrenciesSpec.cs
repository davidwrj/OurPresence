// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Tests.AmountCommonlyUsedCurrenciesSpec
{
    public class GivenIWantEuros
    {
        [Fact]
        public void WhenDecimal_ThenCreatingShouldSucceed()
        {
            // from decimal (other integral types are implicitly converted to decimal)
            var euros = Amount.Euro(10.00m);

            euros.Currency.Should().Be(Currency.FromCode("EUR"));
            euros.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDecimalAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            // from decimal (other integral types are implicitly converted to decimal)
            var euros1 = Amount.Euro(10.005m);
            var euros2 = Amount.Euro(10.005m, MidpointRounding.AwayFromZero);

            euros2.Currency.Should().Be(Currency.FromCode("EUR"));
            euros2.Value.Should().Be(10.01m);
            euros1.Value.Should().NotBe(euros2.Value);
        }

        [Fact]
        public void WhenDouble_ThenCreatingShouldSucceed()
        {
            // from double (float is implicitly converted to double)
            var euros = Amount.Euro(10.00);

            euros.Currency.Should().Be(Currency.FromCode("EUR"));
            euros.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDoubleAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            // from double (float is implicitly converted to double)
            var euros1 = Amount.Euro(10.005);
            var euros2 = Amount.Euro(10.005, MidpointRounding.AwayFromZero);

            euros2.Currency.Should().Be(Currency.FromCode("EUR"));
            euros2.Value.Should().Be(10.01m);
            euros1.Value.Should().NotBe(euros2.Value);
        }

        [Fact]
        public void WhenLong_ThenCreatingShouldSucceed()
        {
            // from long (byte, short and int are implicitly converted to long)
            var euros = Amount.Euro(10L);

            euros.Currency.Should().Be(Currency.FromCode("EUR"));
            euros.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenULong_ThenCreatingShouldSucceed()
        {
            var euros = Amount.Euro(10UL);

            euros.Currency.Should().Be(Currency.FromCode("EUR"));
            euros.Value.Should().Be(10.00m);
        }
    }

    public class GivenIWantUSDollars
    {
        [Fact]
        public void WhenDecimal_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var dollars = Amount.USDollar(10.00m);

            dollars.Currency.Should().Be(Currency.FromCode("USD"));
            dollars.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDecimalAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            // from decimal (other integral types are implicitly converted to decimal)
            var dollars1 = Amount.USDollar(10.005m);
            var dollars2 = Amount.USDollar(10.005m, MidpointRounding.AwayFromZero);

            dollars2.Currency.Should().Be(Currency.FromCode("USD"));
            dollars2.Value.Should().Be(10.01m);
            dollars1.Value.Should().NotBe(dollars2.Value);
        }

        [Fact]
        public void WhenDouble_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var dollars = Amount.USDollar(10.00);

            dollars.Currency.Should().Be(Currency.FromCode("USD"));
            dollars.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDoubleAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var dollars1 = Amount.USDollar(10.005);
            var dollars2 = Amount.USDollar(10.005, MidpointRounding.AwayFromZero);

            dollars2.Currency.Should().Be(Currency.FromCode("USD"));
            dollars2.Value.Should().Be(10.01m);
            dollars1.Value.Should().NotBe(dollars2.Value);
        }

        [Fact]
        public void WhenLong_ThenCreatingShouldSucceed()
        {
            //from long (byte, short and int are implicitly converted to long)
            var dollars = Amount.USDollar(10L);

            dollars.Currency.Should().Be(Currency.FromCode("USD"));
            dollars.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenULong_ThenCreatingShouldSucceed()
        {
            //from long (byte, short and int are implicitly converted to long)
            var dollars = Amount.USDollar(10UL);

            dollars.Currency.Should().Be(Currency.FromCode("USD"));
            dollars.Value.Should().Be(10.00m);
        }
    }

    public class GivenIWantYens
    {
        [Fact]
        public void WhenDecimal_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var yens = Amount.Yen(10.00m);

            yens.Should().NotBeNull();
            yens.Currency.Should().Be(Currency.FromCode("JPY"));
            yens.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDecimalAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var yen1 = Amount.Yen(10.5m);
            var yen2 = Amount.Yen(10.5m, MidpointRounding.AwayFromZero);

            yen2.Currency.Should().Be(Currency.FromCode("JPY"));
            yen2.Value.Should().Be(11m);
            yen1.Value.Should().NotBe(yen2.Value);
        }

        [Fact]
        public void WhenDouble_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var yens = Amount.Yen(10.00);

            yens.Should().NotBeNull();
            yens.Currency.Should().Be(Currency.FromCode("JPY"));
            yens.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDoubleAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var yen1 = Amount.Yen(10.5);
            var yen2 = Amount.Yen(10.5, MidpointRounding.AwayFromZero);

            yen2.Currency.Should().Be(Currency.FromCode("JPY"));
            yen2.Value.Should().Be(11m);
            yen1.Value.Should().NotBe(yen2.Value);
        }

        [Fact]
        public void WhenLong_ThenCreatingShouldSucceed()
        {
            //from long (byte, short and int are implicitly converted to long)
            var yens = Amount.Yen(10L);

            yens.Should().NotBeNull();
            yens.Currency.Should().Be(Currency.FromCode("JPY"));
            yens.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenULong_ThenCreatingShouldSucceed()
        {
            var yens = Amount.Yen(10UL);

            yens.Should().NotBeNull();
            yens.Currency.Should().Be(Currency.FromCode("JPY"));
            yens.Value.Should().Be(10.00m);
        }
    }

    public class GivenIWantPounds
    {
        [Fact]
        public void WhenDecimal_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var pounds = Amount.PoundSterling(10.00m);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDecimalAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var pounds1 = Amount.PoundSterling(10.005m);
            var pounds2 = Amount.PoundSterling(10.005m, MidpointRounding.AwayFromZero);

            pounds2.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds2.Value.Should().Be(10.01m);
            pounds1.Value.Should().NotBe(pounds2.Value);
        }

        [Fact]
        public void WhenDouble_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var pounds = Amount.PoundSterling(10.00);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDoubleAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var pounds1 = Amount.PoundSterling(10.005);
            var pounds2 = Amount.PoundSterling(10.005, MidpointRounding.AwayFromZero);

            pounds2.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds2.Value.Should().Be(10.01m);
            pounds1.Value.Should().NotBe(pounds2.Value);
        }

        [Fact]
        public void WhenLong_ThenCreatingShouldSucceed()
        {
            //from long (byte, short and int are implicitly converted to long)
            var pounds = Amount.PoundSterling(10L);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenULong_ThenCreatingShouldSucceed()
        {
            var pounds = Amount.PoundSterling(10UL);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("GBP"));
            pounds.Value.Should().Be(10.00m);
        }
    }

    public class GivenIWantYuan
    {
        [Fact]
        public void WhenDecimal_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var pounds = Amount.Yuan(10.00m);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDecimalAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from decimal (other integral types are implicitly converted to decimal)
            var pounds1 = Amount.Yuan(10.005m);
            var pounds2 = Amount.Yuan(10.005m, MidpointRounding.AwayFromZero);

            pounds2.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds2.Value.Should().Be(10.01m);
            pounds1.Value.Should().NotBe(pounds2.Value);
        }

        [Fact]
        public void WhenDouble_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var pounds = Amount.Yuan(10.00);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenDoubleAndRoundingAwayFromZero_ThenCreatingShouldSucceed()
        {
            //from double (float is implicitly converted to double)
            var pounds1 = Amount.Yuan(10.005);
            var pounds2 = Amount.Yuan(10.005, MidpointRounding.AwayFromZero);

            pounds2.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds2.Value.Should().Be(10.01m);
            pounds1.Value.Should().NotBe(pounds2.Value);
        }

        [Fact]
        public void WhenLong_ThenCreatingShouldSucceed()
        {
            //from long (byte, short and int are implicitly converted to long)
            var pounds = Amount.Yuan(10L);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds.Value.Should().Be(10.00m);
        }

        [Fact]
        public void WhenULong_ThenCreatingShouldSucceed()
        {
            var pounds = Amount.Yuan(10UL);

            pounds.Should().NotBeNull();
            pounds.Currency.Should().Be(Currency.FromCode("CNY"));
            pounds.Value.Should().Be(10.00m);
        }
    }
}