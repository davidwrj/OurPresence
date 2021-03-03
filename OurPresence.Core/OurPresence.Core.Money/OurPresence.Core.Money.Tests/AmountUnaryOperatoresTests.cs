using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Tests
{
    public class AmountUnaryOperatoresSpecs
    {
        public class GivenIWantToIncrementMoney
        {
            private Amount _yens = new(765m, Currency.FromCode("JPY"));
            private Amount _euros = new(765.43m, Currency.FromCode("EUR"));
            private Amount _dollars = new(765.43m, Currency.FromCode("USD"));
            private Amount _dinars = new(765.432m, Currency.FromCode("BHD"));

            [Fact]
            public void WhenIncrementing_ThenAmountShouldIncrementWithMinorUnit()
            {
                var yens = ++_yens;
                var euros = ++_euros;
                var dollars = ++_dollars;
                var dinars = ++_dinars;

                yens.Value.Should().Be(766m);
                yens.Currency.Should().Be(_yens.Currency);

                euros.Value.Should().Be(765.44m);
                euros.Currency.Should().Be(_euros.Currency);

                dollars.Value.Should().Be(765.44m);
                dollars.Currency.Should().Be(_dollars.Currency);

                dinars.Value.Should().Be(765.433m);
                dinars.Currency.Should().Be(_dinars.Currency);
            }
        }
        
        public class GivenIWantToDecrementMoney
        {
            private Amount _yens = new(765m, Currency.FromCode("JPY"));
            private Amount _euros = new(765.43m, Currency.FromCode("EUR"));
            private Amount _dollars = new(765.43m, Currency.FromCode("USD"));
            private Amount _dinars = new(765.432m, Currency.FromCode("BHD"));

            [Fact]
            public void WhenDecrementing_ThenAmountShouldDecrementWithMinorUnit()
            {
                var yens = --_yens;
                var euros = --_euros;
                var dollars = --_dollars;
                var dinars = --_dinars;

                yens.Value.Should().Be(764m);
                yens.Currency.Should().Be(_yens.Currency);

                euros.Value.Should().Be(765.42m);
                euros.Currency.Should().Be(_euros.Currency);

                dollars.Value.Should().Be(765.42m);
                dollars.Currency.Should().Be(_dollars.Currency);

                dinars.Value.Should().Be(765.431m);
                dinars.Currency.Should().Be(_dinars.Currency);
            }
        }

        public class GivenIWantToAddAndSubtractMoneyUnary
        {
            private readonly Amount _tenEuroPlus = new(10.00m, "EUR");
            private readonly Amount _tenEuroMin = new(-10.00m, "EUR");

            [Fact]
            public void WhenUsingUnaryPlusOperator_ThenThisSucceed()
            {
                var r1 = +_tenEuroPlus;
                var r2 = +_tenEuroMin;

                r1.Value.Should().Be(10.00m);
                r1.Currency.Code.Should().Be("EUR");
                r2.Value.Should().Be(-10.00m);
                r2.Currency.Code.Should().Be("EUR");
            }

            [Fact]
            public void WhenUsingUnaryMinOperator_ThenThisSucceed()
            {
                var r1 = -_tenEuroPlus;
                var r2 = -_tenEuroMin;

                r1.Value.Should().Be(-10.00m);
                r1.Currency.Code.Should().Be("EUR");
                r2.Value.Should().Be(10.00m);
                r2.Currency.Code.Should().Be("EUR");
            }
        }
    }
}