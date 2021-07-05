// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace OurPresence.Core.Money.Tests.Serialization
{
    public class ValidJsonTestData : TheoryData<string, Amount>
    {
        public ValidJsonTestData()
        {
            var amount = new Amount(234.25m, Currency.FromCode("EUR"));

            Add("{ \"Value\": 234.25, \"Currency\": \"EUR\" }", amount); // PascalCase, Value as number
            Add("{ \"Currency\": \"EUR\", \"Value\": 234.25 }", amount); // PascalCase, Value as number, Reversed members
            Add("{ \"Value\": \"234.25\", \"Currency\": \"EUR\" }", amount); // PascalCase, Value as string
            Add("{ \"Currency\": \"EUR\", \"Value\": \"234.25\" }", amount); // PascalCase, Value as string, Reversed members
            Add("{ \"value\": 234.25, \"currency\": \"EUR\" }", amount); // camelCase, Value as number
            Add("{ \"currency\": \"EUR\", \"value\": 234.25 }", amount); // camelCase, Value as number, Reversed members
            Add("{ \"value\": \"234.25\", \"currency\": \"EUR\" }", amount); // camelCase, Value as string
            Add("{ \"currency\": \"EUR\", \"value\": \"234.25\" }", amount); // camelCase, Value as string, Reversed members

            // Members no quotation marks
            Add("{ Value: 234.25, Currency: \"EUR\" }", amount); // PascalCase, Value as number
            Add("{ Currency: \"EUR\", Value: 234.25 }", amount); // PascalCase, Value as number, Reversed members
            Add("{ Value: \"234.25\", Currency: \"EUR\" }", amount); // PascalCase, Value as string
            Add("{ Currency: \"EUR\", Value: \"234.25\" }", amount); // PascalCase, Value as string
            Add("{ value: 234.25, currency: \"EUR\" }", amount); // camelCase, Value as number
            Add("{ currency: \"EUR\", value: 234.25 }", amount); // camelCase, Value as number, Reversed members
            Add("{ value: \"234.25\", currency: \"EUR\" }", amount); // camelCase, Value as string, Members no quotation marks
            Add("{ currency: \"EUR\", value: \"234.25\" }", amount); // camelCase, Value as string, Reversed members

            // Members no quotation marks, Values single quotes
            Add("{ Value: 234.25, Currency: 'EUR' }", amount); // PascalCase, Value as number, 
            Add("{ Currency: 'EUR', Value: 234.25 }", amount); // PascalCase, Value as number, Reversed members
            Add("{ Value: '234.25', Currency: 'EUR' }", amount); // PascalCase, Value as string
            Add("{ Currency: 'EUR', Value: '234.25' }", amount); // PascalCase, Value as string, Reversed members
            Add("{ value: 234.25, currency: 'EUR' }", amount); // camelCase, Value as number
            Add("{ currency: 'EUR', value: 234.25 }", amount); // camelCase, Value as number, Reversed members
            Add("{ value: '234.25', currency: 'EUR' }", amount); // camelCase, Value as string
            Add("{ currency: 'EUR', value: '234.25' }", amount); // camelCase, Value as string, Reversed members

            // Currency with namespace
            Add("{ \"Value\": 234.25, \"Currency\": \"EUR\", \"Namespace\": \"ISO-4217\" }", amount); // PascalCase, Value as number
            Add("{ \"Currency\": \"EUR\", \"Namespace\": \"ISO-4217\", \"Value\": 234.25 }", amount); // PascalCase, Value as number, Reversed members
            Add("{ \"Value\": \"234.25\", \"Currency\": \"EUR\", \"Namespace\": \"ISO-4217\" }", amount); // PascalCase, Value as string
            Add("{ \"Currency\": \"EUR\", \"namespace\": \"ISO-4217\", \"Value\": \"234.25\" }", amount); // PascalCase, Value as string, Reversed members
            Add("{ \"value\": 234.25, \"currency\": \"EUR\", \"namespace\": \"ISO-4217\" }", amount); // camelCase, Value as number
            Add("{ \"currency\": \"EUR\", \"namespace\": \"ISO-4217\", \"value\": 234.25 }", amount); // camelCase, Value as number, Reversed members
            Add("{ \"value\": \"234.25\", \"currency\": \"EUR\", \"namespace\": \"ISO-4217\" }", amount); // camelCase, Value as string
            Add("{ \"currency\": \"EUR\", \"namespace\": \"ISO-4217\", \"value\": \"234.25\" }", amount); // camelCase, Value as string, Reversed members
        }
    }

    public class NestedJsonTestData : TheoryData<string, Order>
    {
        public NestedJsonTestData()
        {
            var order = new Order
            {
                Id = 123,
                Name = "Abc",
                Price = new Amount(234.25m, Currency.FromCode("EUR"))
            };

            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Value\": 234.25, \"Currency\": \"EUR\" } }", order); // Value as number
            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Value\": \"234.25\", \"Currency\": \"EUR\" } }", order); // Value as string

            // Reversed mebers
            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Currency\": \"EUR\", \"Value\": 234.25 } }", order); // Value as number
            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Currency\": \"EUR\", \"Value\": \"234.25\" } }", order); // Value as string

            // camelCase
            Add("{ \"id\": 123, \"name\": \"Abc\", \"price\": { \"value\": 234.25, \"currency\": \"EUR\" } }", order); // Value as number
            Add("{ \"id\": 123, \"name\": \"Abc\", \"price\": { \"value\": \"234.25\", \"currency\": \"EUR\" } }", order); // Value as string

            // Discount explicit null
            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Value\": 234.25, \"Currency\": \"EUR\" }, \"Discount\": null }", order); // Value as number
            Add("{ \"Id\": 123, \"Name\": \"Abc\", \"Price\": { \"Value\": \"234.25\", \"Currency\": \"EUR\" }, \"Discount\": null }", order); // Value as string
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Amount Price { get; set; }
        public Amount? Discount { get; set; }
    }
}
