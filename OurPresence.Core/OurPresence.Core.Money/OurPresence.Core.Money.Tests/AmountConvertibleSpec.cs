using System.Threading;

using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Tests.AmountConvertibleSpec
{
    public class GivenIWantToConvertAmount
    {
        readonly Amount _euros = new(765.43m, "EUR");

        [Fact]
        public void WhenConvertingToDecimal_ThenThisShouldSucceed()
        {
            var result = Amount.ToDecimal(_euros);

            result.Should().Be(765.43m);
        }

        [Fact]
        public void WhenConvertingToDouble_ThenThisShouldSucceed()
        {
            var result = Amount.ToDouble(_euros);

            result.Should().BeApproximately(765.43d, 0.001d);
        }

        [Fact]
        public void WhenConvertingToSingle_ThenThisShouldSucceed()
        {
            var result = Amount.ToSingle(_euros);

            result.Should().BeApproximately(765.43f, 0.001f);
        }
    }

    public class GivenIWantToExplicitCastAmountToNumericType
    {
        readonly Amount _euros = new(10.00m, "EUR");

        [Fact]
        public void WhenExplicitCastingToDecimal_ThenCastingShouldSucceed()
        {
            var m = (decimal)_euros;

            m.Should().Be(10.00m);
        }

        [Fact]
        public void WhenExplicitCastingToDouble_ThenCastingShouldSucceed()
        {
            var d = (double)_euros;

            d.Should().Be(10.00d);
        }

        [Fact]
        public void WhenExplicitCastingToFloat_ThenCastingShouldSucceed()
        {
            var f = (float)_euros;

            f.Should().Be(10.00f);
        }

        [Fact]
        public void WhenExplicitCastingToLong_ThenCastingShouldSucceed()
        {
            var l = (long)_euros;

            l.Should().Be(10L);
        }

        [Fact]
        public void WhenExplicitCastingToByte_ThenCastingShouldSucceed()
        {
            var b = (byte)_euros;

            b.Should().Be(10);
        }

        [Fact]
        public void WhenExplicitCastingToShort_ThenCastingShouldSucceed()
        {
            var s = (short)_euros;

            s.Should().Be(10);
        }

        [Fact]
        public void WhenExplicitCastingToInt_ThenCastingShouldSucceed()
        {
            var i = (int)_euros;

            i.Should().Be(10);
        }
    }

    public class GivenIWantToCastNumericTypeToAmountWithImplicitCurrencyFromTheCurrentCulture
    {
        [Fact]
        public void WhenValueIsByte_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);
            const byte byteValue = 50;
            Amount money = byteValue;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(50);
        }

        [Fact]
        public void WhenValueIsSbyte_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);
            const sbyte sbyteValue = 75;
            Amount money = sbyteValue;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(75);
        }

        [Fact]
        public void WhenValueIsInt16_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const short int16Value = 100;
            Amount money = int16Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(100);
        }

        [Fact]
        public void WhenValueIsInt32_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const int int32Value = 200;
            Amount money = int32Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(200);
        }

        [Fact]
        public void WhenValueIsInt64_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const long int64Value = 300;
            Amount money = int64Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(300);
        }

        [Fact]
        public void WhenValueIsUint16_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const ushort uInt16Value = 400;
            Amount money = uInt16Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(400);
        }

        [Fact]
        public void WhenValueIsUint32_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const uint uInt32Value = 500;
            Amount money = uInt32Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(500);
        }

        [Fact]
        public void WhenValueIsUint64_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const ulong uInt64Value = 600;
            Amount money = uInt64Value;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(600);
        }

        [Fact]
        public void WhenValueIsSingleAndExplicitCast_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            const float singleValue = 700;
            var money = (Amount)singleValue;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(700);
        }

        [Fact]
        public void WhenValueIsDoubleAndExplicitCast_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            var money = (Amount)25.00;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(25.00m);
        }

        [Fact]
        public void WhenValueIsDecimal_ThenCreatingShouldSucceed()
        {
            var currentCurrency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            Amount money = 25.00m;

            money.Currency.Should().Be(currentCurrency);
            money.Value.Should().Be(25.00m);
        }
    }
}