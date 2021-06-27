// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using OurPresence.Core.Money.Extensions;
using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Tests.Serialization
{
    public static class JsonSerializableSpec
    {
        public class GivenIWantToSerializeAmountWithJsonSerializer
        {
            public static IEnumerable<object[]> TestData => new[]
            {
                new object[] { new Amount(765m, Currency.FromCode("JPY")) },
                new object[] { new Amount(765.43m, Currency.FromCode("EUR")) }
            };

            [Theory]
            [MemberData(nameof(TestData))]
            public void WhenSerializing_ThenThisShouldSucceed(Amount money)
            {
                var json = money.ToJson();
                var clone = json.FromJson<Amount>();

                clone.Should().Be(money);
            }
        }

        public class GivenIWantToDeserializeAmountWithJsonSerializer
        {
            private static readonly string s_currentCultureCode = new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol;

            public static IEnumerable<object[]> ValidJsonData => new[]
            {
                new object[] { $"{{ \"value\": 200, \"currency\": \"{s_currentCultureCode}\" }}" },
                new object[] { $"{{ \"currency\": \"{s_currentCultureCode}\", \"value\": 200 }}" },
            };

            public static IEnumerable<object[]> InvalidJsonData => new[]
            {
                new object[] { "{ value: '200' }" },
                new object[] { "{ value: 200 }" },
            };

            public static IEnumerable<object[]> ValidNestedJsonData => new[]
            {
                new object[] { $"{{ cash: {{ value: '200', currency: '{s_currentCultureCode}' }} }}", },
                new object[] { $"{{ cash: {{ value: 200, currency: '{s_currentCultureCode}' }} }}" },
                new object[] { $"{{ cash: {{ currency: '{s_currentCultureCode}', value: 200 }} }}" },
                new object[] { $"{{ cash: {{ currency: '{s_currentCultureCode}', value: '200' }} }}" }
            };

            public static IEnumerable<object[]> ValidNestedNullableJsonData => new[]
            {
                new object[] { $"{{ cash: {{ value: '200', currency: '{s_currentCultureCode}' }} }}", },
                new object[] { $"{{ cash: {{ value: 200, currency: '{s_currentCultureCode}' }} }}" },
                new object[] { $"{{ cash: {{ currency: '{s_currentCultureCode}', value: 200 }} }}" },
                new object[] { $"{{ cash: {{ currency: '{s_currentCultureCode}', value: '200' }} }}" },
                new object[] { $"{{ cash: null }}" },
            };

            [Theory]
            [MemberData(nameof(ValidJsonData))]
            public void WhenDeserializingWithValidJson_ThenThisShouldSucceed(string json)
            {
                var money = new Amount(200, Currency.FromCode(s_currentCultureCode));

                var clone = json.FromJson<Amount>();

                clone.Should().Be(money);
            }

            [Theory]
            [MemberData(nameof(InvalidJsonData))]
            public void WhenDeserializingWithInvalidJson_ThenThisShouldFail(string json)
            {
                Action a = () => json.FromJson<Amount>();
                a.Should().Throw<ArgumentNullException>();
            }

            public class TypeWithMoneyProperty
            {
                public Amount Cash { get; set; }
            }

            [Theory]
            [MemberData(nameof(ValidNestedJsonData))]
            public void WhenDeserializingWithNested_ThenThisShouldSucceed(string json)
            {
                var money = new Amount(200, Currency.FromCode(s_currentCultureCode));

                var clone = json.FromJson<TypeWithMoneyProperty>();

                clone.Cash.Should().Be(money);
            }

            public class TypeWithNullableMoneyProperty
            {
                public Amount? Cash { get; set; }
            }

            [Theory]
            [MemberData(nameof(ValidNestedNullableJsonData))]
            public void WhenDeserializingWithNestedNullable_ThenThisShouldSucceed(string json)
            {
                var money = new Amount(200, Currency.FromCode(s_currentCultureCode));

                var clone = json.FromJson<TypeWithNullableMoneyProperty>();

                if (!json.Contains("null"))
                {
                    clone.Cash.Should().Be(money);
                }
                else
                {
                    clone.Cash.Should().BeNull();
                }
            }
        }
    }
}
