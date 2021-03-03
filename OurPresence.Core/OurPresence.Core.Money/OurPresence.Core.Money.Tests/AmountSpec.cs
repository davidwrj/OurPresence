using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

using Xunit.Abstractions;

namespace OurPresence.Core.Money.Tests.AmountSpec
{
    public class GivenIWantMoneyFromNumericTypeAndAnExplicitCurrencyObject
    {
        private readonly Currency _euro = Currency.FromCode("EUR");

        [Fact]
        public void WhenValueIsByte_ThenCreatingShouldSucceed()
        {
            const byte byteValue = 50;
            var money = new Amount(byteValue, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(50m);
        }

        [Fact]
        public void WhenValueIsSbyte_ThenCreatingShouldSucceed()
        {
            const sbyte sbyteValue = 75;
            var money = new Amount(sbyteValue, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(75m);
        }

        [Fact]
        public void WhenValueIsInt16_ThenCreatingShouldSucceed()
        {
            const short int16Value = 100;
            var money = new Amount(int16Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(100m);
        }

        [Fact]
        public void WhenValueIsInt32_ThenCreatingShouldSucceed()
        {
            const int int32Value = 200;
            var money = new Amount(int32Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(200m);
        }

        [Fact]
        public void WhenValueIsInt64_ThenCreatingShouldSucceed()
        {
            const long int64Value = 300;
            var money = new Amount(int64Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(300m);
        }

        [Fact]
        public void WhenValueIsUint16_ThenCreatingShouldSucceed()
        {
            const ushort uInt16Value = 400;
            var money = new Amount(uInt16Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(400m);
        }

        [Fact]
        public void WhenValueIsUint32_ThenCreatingShouldSucceed()
        {
            const uint uInt32Value = 500;
            var money = new Amount(uInt32Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(500m);
        }

        [Fact]
        public void WhenValueIsUint64_ThenCreatingShouldSucceed()
        {
            const ulong uInt64Value = 600;
            var money = new Amount(uInt64Value, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(600m);
        }

        [Fact]
        public void WhenValueIsSingle_ThenCreatingShouldSucceed()
        {
            const float singleValue = 700;
            var money = new Amount(singleValue, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(700m);
        }

        [Fact]
        public void WhenValueIsDouble_ThenCreatingShouldSucceed()
        {
            const double doubleValue = 800;
            var money = new Amount(doubleValue, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(800m);
        }

        [Fact]
        public void WhenValueIsDecimal_ThenCreatingShouldSucceed()
        {
            const decimal decimalValue = 900;
            var money = new Amount(decimalValue, _euro);

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(900m);
        }
    }

    public class GivenIWantMoneyFromNumericTypeAndAnExplicitIsoCurrencyCode
    {
        private readonly Currency _euro = Currency.FromCode("EUR");

        [Fact]
        public void WhenValueIsByte_ThenCreatingShouldSucceed()
        {
            const byte byteValue = 50;
            var money = new Amount(byteValue, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(50);
        }

        [Fact]
        public void WhenValueIsSbyte_ThenCreatingShouldSucceed()
        {
            const sbyte sbyteValue = 75;
            var money = new Amount(sbyteValue, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(75);
        }

        [Fact]
        public void WhenValueIsInt16_ThenCreatingShouldSucceed()
        {
            const short int16Value = 100;
            var money = new Amount(int16Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(100);
        }

        [Fact]
        public void WhenValueIsInt32_ThenCreatingShouldSucceed()
        {
            const int int32Value = 200;
            var money = new Amount(int32Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(200);
        }

        [Fact]
        public void WhenValueIsInt64_ThenCreatingShouldSucceed()
        {
            const long int64Value = 300;
            var money = new Amount(int64Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(300);
        }

        [Fact]
        public void WhenValueIsUint16_ThenCreatingShouldSucceed()
        {
            const ushort uInt16Value = 400;
            var money = new Amount(uInt16Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(400);
        }

        [Fact]
        public void WhenValueIsUint32_ThenCreatingShouldSucceed()
        {
            const uint uInt32Value = 500;
            var money = new Amount(uInt32Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(500);
        }

        [Fact]
        public void WhenValueIsUint64_ThenCreatingShouldSucceed()
        {
            const ulong uInt64Value = 600;
            var money = new Amount(uInt64Value, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(600);
        }

        [Fact]
        public void WhenValueIsSingle_ThenCreatingShouldSucceed()
        {
            const float singleValue = 700;
            var money = new Amount(singleValue, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(700);
        }

        [Fact]
        public void WhenValueIsDouble_ThenCreatingShouldSucceed()
        {
            const double doubleValue = 800;
            var money = new Amount(doubleValue, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(800);
        }

        [Fact]
        public void WhenValueIsDecimal_ThenCreatingShouldSucceed()
        {
            const decimal decimalValue = 900;
            var money = new Amount(decimalValue, "EUR");

            money.Currency.Should().Be(_euro);
            money.Value.Should().Be(900);
        }

        [Fact]
        public void WhenUnknownIsoCurrencySymbol_ThenThrowEception()
        {
            Action action = () => new Amount(123.25M, "XYZ");

            action.Should().Throw<ArgumentException>();
        }
    }

    public class GivenIWantMoneyWithDifferentRounding
    {
        [Fact]
        public void WhenOnlyAmount_ThenItShouldRoundUp()
        {
            var amount = 0.525m;
            var defaultRounding = new Amount(amount);
            var differentRounding = new Amount(amount, MidpointRounding.AwayFromZero);

            defaultRounding.Value.Should().Be(0.52m);
            differentRounding.Value.Should().Be(0.53m);
        }

        [Fact]
        public void WhenAmountAndCode_ThenItShouldRoundUp()
        {
            var amount = 0.525m;
            var defaultRounding = new Amount(amount, "EUR");
            var differentRounding = new Amount(amount, "EUR", MidpointRounding.AwayFromZero);

            defaultRounding.Value.Should().Be(0.52m);
            differentRounding.Value.Should().Be(0.53m);
        }
    }

    public class GivenIWantToCreateMoneyWithDoubleValue
    {
        [Theory]
        [InlineData(0.03, 0.03)]
        [InlineData(0.3333333333333333, 0.33)]
        public void WhenValueIsDoubleAndWithCurrency_ThenMoneyShouldBeCorrect(double input, decimal expected)
        {
            var money = new Amount(input, "EUR");

            money.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData(0.03, 0.03)]
        [InlineData(0.3333333333333333, 0.33)]
        public void WhenValueIsDoubleWithoutCurrency_ThenMoneyShouldBeCorrect(double input, decimal expected)
        {
            var money = new Amount(input);

            money.Value.Should().Be(expected);
        }
    }

    public class GivenIWantMoneyInMalagasyAriaryWhichHasFiveSubunits
    {
        [Theory]
        [InlineData(0.01, 0.0)]
        [InlineData(0.09, 0.0)]
        [InlineData(0.10, 0.0)]
        [InlineData(0.15, 0.2)]
        [InlineData(0.22, 0.2)]
        [InlineData(0.29, 0.2)]
        [InlineData(0.30, 0.4)]
        [InlineData(0.33, 0.4)]
        [InlineData(0.38, 0.4)]
        [InlineData(0.40, 0.4)]
        [InlineData(0.41, 0.4)]
        [InlineData(0.45, 0.4)]
        [InlineData(0.46, 0.4)]
        [InlineData(0.50, 0.4)]
        [InlineData(0.54, 0.6)]
        [InlineData(0.57, 0.6)]
        [InlineData(0.60, 0.6)]
        [InlineData(0.68, 0.6)]
        [InlineData(0.70, 0.8)]
        [InlineData(0.74, 0.8)]
        [InlineData(0.77, 0.8)]
        [InlineData(0.80, 0.8)]
        [InlineData(0.83, 0.8)]
        [InlineData(0.85, 0.8)]
        [InlineData(0.86, 0.8)]
        [InlineData(0.90, 0.8)]
        [InlineData(0.91, 1.0)]
        [InlineData(0.95, 1.0)]
        [InlineData(0.99, 1.0)]
        public void WhenOnlyAmount_ThenItShouldRoundUp(decimal input, decimal expected)
        {
            // 1 MGA = 5 iraimbilanja
            var money = new Amount(input, "MGA");

            money.Value.Should().Be(expected);
        }
    }

    public class GivenIWantToConvertDoubleToDecimal
    {
        private readonly Currency _euro = Currency.FromCode("EUR");

        private readonly ITestOutputHelper _output;

        public GivenIWantToConvertDoubleToDecimal(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void WhenValueIsNormalValue_ThenCreatingShouldSucceed()
        {
            var value = 251426433.75935m;
            var value1 = 251426433.75935;
            var value2 = new decimal(value1);
            var value3 = (decimal)value1;
            var value4 = decimal.Parse(value1.ToString());

            var result0 = value.ToString(CultureInfo.InvariantCulture);
            var result1 = value1.ToString(CultureInfo.InvariantCulture);
            var result2 = value2.ToString(CultureInfo.InvariantCulture);
            var result3 = value3.ToString(CultureInfo.InvariantCulture);
            var result4 = value4.ToString(CultureInfo.InvariantCulture);

            _output.WriteLine(result0);
            _output.WriteLine(result1);
            _output.WriteLine(result2);
            _output.WriteLine(result3);
            _output.WriteLine(result4);
        }

        [Fact]
        public void WhenValueIsBigValue_ThenCreatingShouldSucceed()
        {
            var value = 7922816251426433.7593543950335m;
            var value1 = 7922816251426433.7593543950335;
            var value2 = new decimal(value1);
            var value3 = (decimal)value1;

            var result0 = value.ToString(CultureInfo.InvariantCulture);
            var result1 = value1.ToString(CultureInfo.InvariantCulture);
            var result2 = value2.ToString(CultureInfo.InvariantCulture);
            var result3 = value3.ToString(CultureInfo.InvariantCulture);

            _output.WriteLine(result0);
            _output.WriteLine(result1);
            _output.WriteLine(result2);
            _output.WriteLine(result3);
        }

        [Fact]
        public void WhenValueIsVeryBigValue_ThenCreatingShouldSucceed()
        {
            var value = 79228162514264337593543.950335m;
            var value1 = 79228162514264337593543.950335;
            var value2 = new decimal(value1);
            var value3 = (decimal)value1;

            var result0 = value.ToString(CultureInfo.InvariantCulture);
            var result1 = value1.ToString(CultureInfo.InvariantCulture);
            var result2 = value2.ToString(CultureInfo.InvariantCulture);
            var result3 = value3.ToString(CultureInfo.InvariantCulture);

            _output.WriteLine(result0);
            _output.WriteLine(result1);
            _output.WriteLine(result2);
            _output.WriteLine(result3);
        }

        [Fact]
        public void WhenValueIsVerySmall_ThenCreatingShouldSucceed()
        {
            var value = 0.0079228162514264337593543950335m;
            var value1 = 0.0079228162514264337593543950335;
            var value2 = new decimal(value1);
            var value3 = (decimal)value1;
            var value4 = decimal.Parse(value1.ToString());

            var result0 = value.ToString(CultureInfo.InvariantCulture);
            var result1 = value1.ToString(CultureInfo.InvariantCulture);
            var result2 = value2.ToString(CultureInfo.InvariantCulture);
            var result3 = value3.ToString(CultureInfo.InvariantCulture);
            var result4 = value4.ToString(CultureInfo.InvariantCulture);

            _output.WriteLine(result0);
            _output.WriteLine(result1);
            _output.WriteLine(result2);
            _output.WriteLine(result3);
            _output.WriteLine(result4);
        }
    }

    public class GivenIWantToDeconstructMoney
    {
        [Fact]
        public void WhenDeconstructing_ThenShouldSucceed()
        {
            var money = new Amount(10m, "EUR");

            var (amount, currency) = money;

            amount.Should().Be(10m);
            currency.Should().Be(Currency.FromCode("EUR"));
        }
    }
}
