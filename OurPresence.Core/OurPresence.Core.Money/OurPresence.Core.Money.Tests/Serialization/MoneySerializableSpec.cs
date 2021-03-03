using System.Collections.Generic;
using OurPresence.Core.Money.Extensions;
using OurPresence.Core.Money.Tests.Serialization;
using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Serialization.Tests.AmountSerializableSpec
{
    public class GivenIWantToDeserializeAmount
    {
        [Theory]
        [ClassData(typeof(ValidJsonTestData))]
        public void WhenDeserializing_ThenThisShouldSucceed(string json, Amount expected)
        {
            var clone = json.FromJson<Amount>();

            clone.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(NestedJsonTestData))]
        public void WhenDeserializingWithNested_ThenThisShouldSucceed(string json, Order expected)
        {
            var clone = json.FromJson<Order>();

            clone.Should().BeEquivalentTo(expected);
            clone.Discount.Should().BeNull();
        }
    }

    public class GivenIWantToSerializeAmountWithJsonSerializer
    {
        public static IEnumerable<object[]> TestData => new[]
        {
                new object[] { new Amount(765.4321m, Currency.FromCode("JPY")) },
                new object[] { new Amount(765.4321m, Currency.FromCode("EUR")) },
                new object[] { new Amount(765.4321m, Currency.FromCode("USD")) },
                new object[] { new Amount(765.4321m, Currency.FromCode("BHD")) }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void WhenSerializingCurrency_ThenThisShouldSucceed(Amount money)
        {
            var json = money.Currency.ToJson();
            var clone = json.FromJson<Currency>();

            clone.Should().Be(money.Currency);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void WhenSerializingMoney_ThenThisShouldSucceed(Amount money)
        {
            var json = money.ToJson();
            var clone = json.FromJson<Amount>();

            clone.Should().Be(money);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void WhenSerializingOrder_ThenThisShouldSucceed(Amount money)
        {
            var order = new Order
            {
                Id = 123,
                Price = money,
                Name = "Foo"
            };

            var x = order.Price.ToJson();
            _ = x.FromJson<Amount>();

            var json = order.ToJson();
            var clone = json.FromJson<Order>();

            clone.Should().BeEquivalentTo(order);
        }
    }
}
