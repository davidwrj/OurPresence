// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Exceptions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class ConditionTests
    {
        #region Classes used in tests
        public class Car : Drop, System.IEquatable<Car>, System.IEquatable<string>
        {
            public string Make { get; set; }
            public string Model { get; set; }

            public Car(Template template) : base(template)
            { }

            public override string ToString()
            {
                return $"{Make} {Model}";
            }

            public override bool Equals(object other)
            {
                if(other is Car @car)
                    return Equals(@car);

                if(other is string @string)
                    return Equals(@string);

                return false;
            }

            public bool Equals(Car other)
            {
                return other.Make == this.Make && other.Model == this.Model;
            }

            public bool Equals(string other)
            {
                return other == this.ToString();
            }
        }
        #endregion

        [Fact]
        public void TestBasicCondition()
        {
            var template = new Template();

            Assert.False(new Condition(left: "1", @operator: "==", right: "2").Evaluate(template, context: null, formatProvider: CultureInfo.InvariantCulture));
            Assert.True(new Condition(left: "1", @operator: "==", right: "1").Evaluate(template, context: null, formatProvider: CultureInfo.InvariantCulture));

            // NOTE(David Burg): Validate that type conversion order preserves legacy behavior
            // Even if it's out of Shopify spec compliance (all type but null and false should evaluate to true).
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if true == 'true' %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if 'true' == true %}TRUE{% else %}FALSE{% endif %}");

            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if true %}TRUE{% endif %}");
            Helper.AssertTemplateResult(expected: "", template: "{% if false %}TRUE{% endif %}");
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if true %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if false %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if '1' == '1' %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if '1' == '2' %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "This condition will always be true.", template: "{% assign tobi = 'Tobi' %}{% if tobi %}This condition will always be true.{% endif %}");

            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if true == true %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if true == false %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if false == false %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if false == true %}TRUE{% else %}FALSE{% endif %}");

            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if true != true %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if true != false %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "FALSE", template: "{% if false != false %}TRUE{% else %}FALSE{% endif %}");
            Helper.AssertTemplateResult(expected: "TRUE", template: "{% if false != true %}TRUE{% else %}FALSE{% endif %}");

            // NOTE(David Burg): disabled test due to https://github.com/dotliquid/dotliquid/issues/394
            ////Helper.AssertTemplateResult(expected: "This text will always appear if \"name\" is defined.", template: "{% assign name = 'Tobi' %}{% if name == true %}This text will always appear if \"name\" is defined.{% endif %}");
        }

        [Fact]
        public void TestDefaultOperatorsEvaluateTrue()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            this.AssertEvaluatesTrue(context, left: "1", op: "==", right: "1");
            this.AssertEvaluatesTrue(context, left: "1", op: "!=", right: "2");
            this.AssertEvaluatesTrue(context, left: "1", op: "<>", right: "2");
            this.AssertEvaluatesTrue(context, left: "1", op: "<", right: "2");
            this.AssertEvaluatesTrue(context, left: "2", op: ">", right: "1");
            this.AssertEvaluatesTrue(context, left: "1", op: ">=", right: "1");
            this.AssertEvaluatesTrue(context, left: "2", op: ">=", right: "1");
            this.AssertEvaluatesTrue(context, left: "1", op: "<=", right: "2");
            this.AssertEvaluatesTrue(context, left: "1", op: "<=", right: "1");
        }

        [Fact]
        public void TestDefaultOperatorsEvaluateFalse()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesFalse(context, "1", "==", "2");
            AssertEvaluatesFalse(context, "1", "!=", "1");
            AssertEvaluatesFalse(context, "1", "<>", "1");
            AssertEvaluatesFalse(context, "1", "<", "0");
            AssertEvaluatesFalse(context, "2", ">", "4");
            AssertEvaluatesFalse(context, "1", ">=", "3");
            AssertEvaluatesFalse(context, "2", ">=", "4");
            AssertEvaluatesFalse(context, "1", "<=", "0");
            AssertEvaluatesFalse(context, "1", "<=", "0");
        }

        [Fact]
        public void TestContainsWorksOnStrings()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesTrue(context, "'bob'", "contains", "'o'");
            AssertEvaluatesTrue(context, "'bob'", "contains", "'b'");
            AssertEvaluatesTrue(context, "'bob'", "contains", "'bo'");
            AssertEvaluatesTrue(context, "'bob'", "contains", "'ob'");
            AssertEvaluatesTrue(context, "'bob'", "contains", "'bob'");

            AssertEvaluatesFalse(context, "'bob'", "contains", "'bob2'");
            AssertEvaluatesFalse(context, "'bob'", "contains", "'a'");
            AssertEvaluatesFalse(context, "'bob'", "contains", "'---'");
        }

        [Fact]
        public void TestContainsWorksOnIntArrays()
        {
            // NOTE(daviburg): OurPresence.Modeller.Liquid is in violation of explicit non-support of arrays for contains operators, quote:
            // "contains can only search strings. You cannot use it to check for an object in an array of objects."
            // https://shopify.github.io/liquid/basics/operators/
            // This is a rather harmless violation as all it does in generate useful output for a request which would fail
            // in the canonical Shopify implementation.
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            context["array"] = new[] { 1, 2, 3, 4, 5 };

            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "1");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "2");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "3");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "4");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "5");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "0");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "6");

            // NOTE(daviburg): Historically testing for equality cross integer and string boundaries resulted in not equal.
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'1'");
        }

        [Fact]
        public void TestContainsWorksOnLongArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            context["array"] = new long[] { 1, 2, 3, 4, 5 };

            AssertEvaluatesTrue(context, "array", "contains", "1");
            AssertEvaluatesTrue(context, "array", "contains", "1.0");
            AssertEvaluatesTrue(context, "array", "contains", "2");
            AssertEvaluatesTrue(context, "array", "contains", "3");
            AssertEvaluatesTrue(context, "array", "contains", "4");
            AssertEvaluatesTrue(context, "array", "contains", "5");
            AssertEvaluatesFalse(context, "array", "contains", "6");
            AssertEvaluatesFalse(context, "array", "contains", "0");
            AssertEvaluatesFalse(context, "array", "contains", "'1'");
        }

        [Fact]
        public void TestStringArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            var _array = new List<string>() { "Apple", "Orange", null, "Banana" };
            context["array"] = _array.ToArray();
            context["first"] = _array.First();
            context["last"] = _array.Last();

            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'Apple'");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "first");
            AssertEvaluatesTrue(context, left: "array.first", op: "==", right: "first");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'apple'");
            AssertEvaluatesFalse(context, left: "array", op: "startsWith", right: "'apple'");
            AssertEvaluatesFalse(context, left: "array.first", op: "==", right: "'apple'");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'Mango'");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'Orange'");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'Banana'");
            AssertEvaluatesTrue(context, left: "array", op: "endsWith", right: "last");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'Orang'");
        }

        [Fact]
        public void TestClassArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            var _array = new List<Car>()
            {
                new Car(template) { Make = "Honda", Model = "Accord" },
                new Car(template) { Make = "Ford", Model = "Explorer" }
            };
            context["array"] = _array.ToArray();
            context["first"] = _array.First();
            context["last"] = _array.Last();
            context["clone"] = new Car(template) { Make = "Honda", Model = "Accord" };
            context["camry"] = new Car(template) { Make = "Toyota", Model = "Camry" };

            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "first");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "first");
            AssertEvaluatesTrue(context, left: "array.first", op: "==", right: "first");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "clone");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "clone");
            AssertEvaluatesTrue(context, left: "array", op: "endsWith", right: "last");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "camry");
        }

        [Fact]
        public void TestTruthyArray()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            var _array = new List<bool>() { true };
            context["array"] = _array.ToArray();
            context["first"] = _array.First();

            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "first");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "first");
            AssertEvaluatesTrue(context, left: "array.first", op: "==", right: "'true'");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "'true'");

            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'true'"); // to be re-evaluated in #362
        }

        [Fact]
        public void TestCharArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            var _array = new List<char> { 'A', 'B', 'C' };
            context["array"] = _array.ToArray();
            context["first"] = _array.First();
            context["last"] = _array.Last();

            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'A'");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "first");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "first");
            AssertEvaluatesTrue(context, left: "array.first", op: "==", right: "first");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'a'");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'X'");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'B'");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "'C'");
            AssertEvaluatesTrue(context, left: "array", op: "endsWith", right: "last");
        }

        [Fact]
        public void TestByteArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            var _array = new List<byte> { 0x01, 0x02, 0x03, 0x30 };
            context["array"] = _array.ToArray();
            context["first"] = _array.First();
            context["last"] = _array.Last();

            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "0");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "'0'");
            AssertEvaluatesTrue(context, left: "array", op: "startsWith", right: "first");
            AssertEvaluatesTrue(context, left: "array.first", op: "==", right: "first");
            AssertEvaluatesTrue(context, left: "array", op: "contains", right: "first");
            AssertEvaluatesFalse(context, left: "array", op: "contains", right: "1");
            AssertEvaluatesTrue(context, left: "array", op: "endsWith", right: "last");
        }

        [Fact]
        public void TestContainsWorksOnDoubleArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            context["array"] = new double[] { 1.0, 2.1, 3.25, 4.333, 5.0 };

            AssertEvaluatesTrue(context, "array", "contains", "1.0");
            AssertEvaluatesFalse(context, "array", "contains", "0");
            AssertEvaluatesTrue(context, "array", "contains", "2.1");
            AssertEvaluatesFalse(context, "array", "contains", "3");
            AssertEvaluatesFalse(context, "array", "contains", "4.33");
            AssertEvaluatesTrue(context, "array", "contains", "5.00");
            AssertEvaluatesFalse(context, "array", "contains", "6");

            AssertEvaluatesFalse(context, "array", "contains", "'1'");
        }

        [Fact]
        public void TestContainsReturnsFalseForNilCommands()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesFalse(context, "not_assigned", "contains", "0");
            AssertEvaluatesFalse(context, "0", "contains", "not_assigned");
        }

        [Fact]
        public void TestStartsWithWorksOnStrings()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesTrue(context, "'dave'", "startswith", "'d'");
            AssertEvaluatesTrue(context, "'dave'", "startswith", "'da'");
            AssertEvaluatesTrue(context, "'dave'", "startswith", "'dav'");
            AssertEvaluatesTrue(context, "'dave'", "startswith", "'dave'");

            AssertEvaluatesFalse(context, "'dave'", "startswith", "'ave'");
            AssertEvaluatesFalse(context, "'dave'", "startswith", "'e'");
            AssertEvaluatesFalse(context, "'dave'", "startswith", "'---'");
        }

        [Fact]
        public void TestStartsWithWorksOnArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            context["array"] = new[] { 1, 2, 3, 4, 5 };

            AssertEvaluatesFalse(context, "array", "startswith", "0");
            AssertEvaluatesTrue(context, "array", "startswith", "1");
        }

        [Fact]
        public void TestStartsWithReturnsFalseForNilCommands()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesFalse(context, "not_assigned", "startswith", "0");
            AssertEvaluatesFalse(context, "0", "startswith", "not_assigned");
        }

        [Fact]
        public void TestEndsWithWorksOnStrings()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesTrue(context, "'dave'", "endswith", "'e'");
            AssertEvaluatesTrue(context, "'dave'", "endswith", "'ve'");
            AssertEvaluatesTrue(context, "'dave'", "endswith", "'ave'");
            AssertEvaluatesTrue(context, "'dave'", "endswith", "'dave'");

            AssertEvaluatesFalse(context, "'dave'", "endswith", "'dav'");
            AssertEvaluatesFalse(context, "'dave'", "endswith", "'d'");
            AssertEvaluatesFalse(context, "'dave'", "endswith", "'---'");
        }

        [Fact]
        public void TestEndsWithWorksOnArrays()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            context["array"] = new[] { 1, 2, 3, 4, 5 };

            AssertEvaluatesFalse(context, "array", "endswith", "0");
            AssertEvaluatesTrue(context, "array", "endswith", "5");
        }

        [Fact]
        public void TestEndsWithReturnsFalseForNilCommands()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            AssertEvaluatesFalse(context, "not_assigned", "endswith", "0");
            AssertEvaluatesFalse(context, "0", "endswith", "not_assigned");
        }

        [Fact]
        public void TestDictionaryHasKey()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            Dictionary<string, string> testDictionary = new Dictionary<string, string>
            {
                { "dave", "0" },
                { "bob", "4" }
            };
            context["dictionary"] = testDictionary;

            AssertEvaluatesTrue(context, "dictionary", "haskey", "'bob'");
            AssertEvaluatesFalse(context, "dictionary", "haskey", "'0'");
        }

        [Fact]
        public void TestDictionaryHasValue()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            Dictionary<string, string> testDictionary = new Dictionary<string, string>
            {
                { "dave", "0" },
                { "bob", "4" }
            };
            context["dictionary"] = testDictionary;

            AssertEvaluatesTrue(context, "dictionary", "hasvalue", "'0'");
            AssertEvaluatesFalse(context, "dictionary", "hasvalue", "'bob'");
        }

        [Fact]
        public void TestOrCondition()
        {
            var template = new Template();

            Condition condition = new Condition("1", "==", "2");
            Assert.False(condition.Evaluate(template, null, CultureInfo.InvariantCulture));

            condition.Or(new Condition("2", "==", "1"));
            Assert.False(condition.Evaluate(template, null, CultureInfo.InvariantCulture));

            condition.Or(new Condition("1", "==", "1"));
            Assert.True(condition.Evaluate(template, null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void TestAndCondition()
        {
            var template = new Template();

            Condition condition = new Condition("1", "==", "1");
            Assert.True(condition.Evaluate(template, null, CultureInfo.InvariantCulture));

            condition.And(new Condition("2", "==", "2"));
            Assert.True(condition.Evaluate(template, null, CultureInfo.InvariantCulture));

            condition.And(new Condition("2", "==", "1"));
            Assert.False(condition.Evaluate(template, null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void TestShouldAllowCustomProcOperator()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            try
            {
                context.Condition.Operators.Add("starts_with", (left, right) => Regex.IsMatch(left.ToString(), string.Format("^{0}", right.ToString())));

                AssertEvaluatesTrue(context, "'bob'", "starts_with", "'b'");
                AssertEvaluatesFalse(context, "'bob'", "starts_with", "'o'");
            }
            finally
            {
                context.Condition.Operators.Remove("starts_with");
            }
        }

        [Fact]
        public void TestCapitalInCustomOperatorInt()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            try
            {
                context.Condition.Operators.Add("IsMultipleOf", (left, right) => (int)left % (int)right == 0);

                // exact match
                AssertEvaluatesTrue(context, "16", "IsMultipleOf", "4");
                AssertEvaluatesTrue(context, "2147483646", "IsMultipleOf", "2");
                AssertError(context, "2147483648", "IsMultipleOf", "2", typeof(System.InvalidCastException));
                AssertEvaluatesFalse(context, "16", "IsMultipleOf", "5");

                // lower case: compatibility
                AssertEvaluatesTrue(context, "16", "ismultipleof", "4");
                AssertEvaluatesFalse(context, "16", "ismultipleof", "5");

                AssertEvaluatesTrue(context, "16", "is_multiple_of", "4");
                AssertEvaluatesFalse(context, "16", "is_multiple_of", "5");

                // camel case : incompatible
                AssertError(context, "16", "isMultipleOf", "4", typeof(ArgumentException));

                //Run tests through the template to verify that capitalization rules are followed through template parsing
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 IsMultipleOf 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 IsMultipleOf 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 ismultipleof 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 ismultipleof 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 is_multiple_of 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 is_multiple_of 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("Liquid error: Unknown operator isMultipleOf", "{% if 16 isMultipleOf 4 %} TRUE {% endif %}");
            }
            finally
            {
                context.Condition.Operators.Remove("IsMultipleOf");
            }
        }

        [Fact]
        public void TestCapitalInCustomOperatorLong()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);
            try
            {
                context.Condition.Operators.Add("IsMultipleOf", (left, right) => System.Convert.ToInt64(left) % System.Convert.ToInt64(right) == 0);

                // exact match
                AssertEvaluatesTrue(context, "16", "IsMultipleOf", "4");
                AssertEvaluatesTrue(context, "2147483646", "IsMultipleOf", "2");
                AssertEvaluatesTrue(context, "2147483648", "IsMultipleOf", "2");
                AssertEvaluatesFalse(context, "16", "IsMultipleOf", "5");

                // lower case: compatibility
                AssertEvaluatesTrue(context, "16", "ismultipleof", "4");
                AssertEvaluatesFalse(context, "16", "ismultipleof", "5");

                AssertEvaluatesTrue(context, "16", "is_multiple_of", "4");
                AssertEvaluatesFalse(context, "16", "is_multiple_of", "5");

                // camel case : incompatible
                AssertError(context, "16", "isMultipleOf", "4", typeof(ArgumentException));

                //Run tests through the template to verify that capitalization rules are followed through template parsing
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 IsMultipleOf 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 IsMultipleOf 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 ismultipleof 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 ismultipleof 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 is_multiple_of 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 14 is_multiple_of 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("Liquid error: Unknown operator isMultipleOf", "{% if 16 isMultipleOf 4 %} TRUE {% endif %}");
            }
            finally
            {
                context.Condition.Operators.Remove("IsMultipleOf");
            }
        }

        [Fact]
        public void TestCapitalInCustomCSharpOperatorInt()
        {
            var template = new Template();

            var context = new Context(template, CultureInfo.InvariantCulture);

            try
            {
                context.Condition.Operators.Add("DivisibleBy", (left, right) => (int)left % (int)right == 0);

                // exact match
                AssertEvaluatesTrue(context, "16", "DivisibleBy", "4");
                AssertEvaluatesTrue(context, "2147483646", "DivisibleBy", "2");
                AssertError(context, "2147483648", "DivisibleBy", "2", typeof(System.InvalidCastException));
                AssertEvaluatesFalse(context, "16", "DivisibleBy", "5");

                // lower case: compatibility
                AssertEvaluatesTrue(context, "16", "divisibleby", "4");
                AssertEvaluatesFalse(context, "16", "divisibleby", "5");

                // camel case : compatibility
                AssertEvaluatesTrue(context, "16", "divisibleBy", "4");
                AssertEvaluatesFalse(context, "16", "divisibleBy", "5");

                // snake case : incompatible
                AssertError(context, "16", "divisible_by", "4", typeof(ArgumentException));

                //Run tests through the template to verify that capitalization rules are followed through template parsing
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 DivisibleBy 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 16 DivisibleBy 5 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 divisibleby 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 16 divisibleby 5 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("Liquid error: Unknown operator divisible_by", "{% if 16 divisible_by 4 %} TRUE {% endif %}");
            }
            finally
            {
                context.Condition.Operators.Remove("DivisibleBy");
            }
        }

        [Fact]
        public void TestCapitalInCustomCSharpOperatorLong()
        {
            //have to run this test in a lock because it requires
            //changing the globally static NamingConvention
            var template = new Template();
            var context = new Context(template, CultureInfo.InvariantCulture);

            try
            {
                context.Condition.Operators.Add("DivisibleBy", (left, right) => System.Convert.ToInt64(left) % System.Convert.ToInt64(right) == 0);

                // exact match
                AssertEvaluatesTrue(context, "16", "DivisibleBy", "4");
                AssertEvaluatesTrue(context, "2147483646", "DivisibleBy", "2");
                AssertEvaluatesTrue(context, "2147483648", "DivisibleBy", "2");
                AssertEvaluatesFalse(context, "16", "DivisibleBy", "5");

                // lower case: compatibility
                AssertEvaluatesTrue(context, "16", "divisibleby", "4");
                AssertEvaluatesFalse(context, "16", "divisibleby", "5");

                // camel case: compatibility
                AssertEvaluatesTrue(context, "16", "divisibleBy", "4");
                AssertEvaluatesFalse(context, "16", "divisibleBy", "5");

                // snake case: incompatible
                AssertError(context, "16", "divisible_by", "4", typeof(ArgumentException));

                //Run tests through the template to verify that capitalization rules are followed through template parsing
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 DivisibleBy 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 16 DivisibleBy 5 %} TRUE {% endif %}");
                Helper.AssertTemplateResult(" TRUE ", "{% if 16 divisibleby 4 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("", "{% if 16 divisibleby 5 %} TRUE {% endif %}");
                Helper.AssertTemplateResult("Liquid error: Unknown operator divisible_by", "{% if 16 divisible_by 4 %} TRUE {% endif %}");
            }
            finally
            {
                context.Condition.Operators.Remove("DivisibleBy");
            }
        }

        [Fact]
        public void TestLessThanDecimal()
        {
            var model = new { value = new decimal(-10.5) };

            string output = Template.Parse("{% if model.value < 0 %}passed{% endif %}")
                .Render(Hash.FromAnonymousObject(new { model }));

            Assert.Equal("passed", output);
        }

        [Fact]
        public void TestCompareBetweenDifferentTypes()
        {
            var row = new Dictionary<string, object>();

            short id = 1;
            row.Add("MyID", id);

            var current = "MyID is {% if MyID == 1 %}1{%endif%}";
            var parse = OurPresence.Modeller.Liquid.Template.Parse(current);
            var parsedOutput = parse.Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = Hash.FromDictionary(row) });
            Assert.Equal("MyID is 1", parsedOutput);
        }

        [Fact]
        public void TestShouldAllowCustomProcOperatorCapitalized()
        {
            var template = new Template();
            var context = new Context(template,CultureInfo.InvariantCulture);
            try
            {
                context.Condition.Operators.Add("StartsWith", (left, right) => Regex.IsMatch(left.ToString(), string.Format("^{0}", right.ToString())));

                Helper.AssertTemplateResult("", "{% if 'bob' StartsWith 'B' %} YES {% endif %}", null);
                AssertEvaluatesTrue(context, "'bob'", "StartsWith", "'b'");
                AssertEvaluatesFalse(context, "'bob'", "StartsWith", "'o'");
            }
            finally
            {
                context.Condition.Operators.Remove("StartsWith");
            }
        }

        [Fact]
        public void TestCSharp_LowerCaseAccepted()
        {
            Helper.AssertTemplateResult("", "{% if 'bob' startswith 'B' %} YES {% endif %}", null);
            Helper.AssertTemplateResult(" YES ", "{% if 'Bob' startswith 'B' %} YES {% endif %}", null);
        }

        [Fact]
        public void TestCSharp_PascalCaseAccepted()
        {
            Helper.AssertTemplateResult("", "{% if 'bob' StartsWith 'B' %} YES {% endif %}", null);
            Helper.AssertTemplateResult(" YES ", "{% if 'Bob' StartsWith 'B' %} YES {% endif %}", null);
        }

        [Fact]
        public void TestCSharp_LowerPascalCaseAccepted()
        {
            Helper.AssertTemplateResult("", "{% if 'bob' startsWith 'B' %} YES {% endif %}", null);
            Helper.AssertTemplateResult(" YES ", "{% if 'Bob' startsWith 'B' %} YES {% endif %}", null);
        }

        [Fact]
        public void TestCSharp_SnakeCaseNotAccepted()
        {
            Helper.AssertTemplateResult("Liquid error: Unknown operator starts_with", "{% if 'bob' starts_with 'B' %} YES {% endif %}", null);
        }

        #region Helper methods

        private void AssertEvaluatesTrue(Context context, string left, string op, string right)
        {
            new Condition(left, op, right)
                .Evaluate(context.Template, context ?? new Context(context.Template, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture).Should().BeTrue();
        }

        private void AssertEvaluatesFalse(Context context, string left, string op, string right)
        {
            Assert.False(new Condition(left, op, right).Evaluate(context.Template, context ?? new Context(context.Template,CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }

        private void AssertError(Context context, string left, string op, string right, System.Type errorType)
        {
            Assert.Throws(errorType, () => new Condition(left, op, right).Evaluate(context.Template, context ?? new Context(context.Template, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
