using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace OurPresence.Core.Money
{
    /// <summary>Represents Money, an amount defined in a specific Currency.</summary>
    /// <remarks>
    /// The <see cref="Amount"/> structure allows development of applications that handle
    /// various types of Currency. Amount will hold the <see cref="Currency"/> and Value of monetary values,
    /// and ensure that two different currencies cannot be combined arithmetically.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Amount : IEquatable<Amount>
    {
        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</remarks>
        public Amount(decimal value)
            : this(value, Currency.CurrentCurrency)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on a ISO 4217 Currency code.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="currency">A ISO 4217 Currency code, like EUR or USD.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</remarks>
        [JsonConstructor]
        public Amount(decimal value, string currency)
            : this(value, Currency.FromCode(currency))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>).</remarks>
        public Amount(decimal value, MidpointRounding rounding)
            : this(value, Currency.CurrentCurrency, rounding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="currency">The Currency of the money.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</remarks>
        public Amount(decimal value, Currency currency)
            : this(value, currency, MidpointRounding.ToEven)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on a ISO 4217 Currency code.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="currency">A ISO 4217 Currency code, like EUR or USD.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>).</remarks>
        public Amount(decimal value, string currency, MidpointRounding rounding)
            : this(value, Currency.FromCode(currency), rounding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="currency">The Currency of the money.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <remarks>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>).</remarks>
        public Amount(decimal value, Currency currency, MidpointRounding rounding)
        {
            Currency = currency;
            Value = Round(value, currency, rounding);
        }

        // int, uint ([CLSCompliant(false)]) // auto-casting to decimal so not needed

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="value">The Amount of money as <see langword="double"/> or <see langword="float"/> (float is implicitly
        /// casted to double).</param>
        /// <remarks>This constructor will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public Amount(double value)
            : this((decimal)value)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on a ISO 4217 Currency code.</summary>
        /// <param name="value">The Amount of money as <see langword="double"/> or <see langword="float"/> (float is implicitly
        /// casted to double).</param>
        /// <param name="currency">A ISO 4217 Currency code, like EUR or USD.</param>
        /// <remarks>This constructor will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public Amount(double value, string currency)
            : this((decimal)value, Currency.FromCode(currency))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="double"/> or <see langword="float"/> (float is implicitly
        /// casted to double).</param>
        /// <param name="currency">The Currency of the money.</param>
        /// <remarks>This constructor will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public Amount(double value, Currency currency)
            : this((decimal)value, currency)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="double"/> or <see langword="float"/> (float is implicitly
        /// casted to double).</param>
        /// <param name="currency">The Currency of the money.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <remarks>This constructor will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Money.Currency.DecimalDigits"/>).</para></remarks>
        public Amount(double value, Currency currency, MidpointRounding rounding)
            : this((decimal)value, currency, rounding)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="value">The Amount of money as <see langword="long"/>, <see langword="int"/>, <see langword="short"/> or<see cref="byte"/>.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        public Amount(long value)
            : this((decimal)value)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on a ISO 4217 Currency code.</summary>
        /// <param name="value">The Amount of money as <see langword="long"/>, <see langword="int"/>, <see langword="short"/> or<see cref="byte"/>.</param>
        /// <param name="currency">A ISO 4217 Currency code, like EUR or USD.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        public Amount(long value, string currency)
            : this((decimal)value, Currency.FromCode(currency))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="long"/>, <see langword="int"/>, <see langword="short"/> or<see cref="byte"/>.</param>
        /// <param name="currency">The Currency of the money.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        public Amount(long value, Currency currency)
            : this((decimal)value, currency)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="value">The Amount of money as <see langword="ulong"/>, <see langword="uint"/>, <see langword="ushort"/>
        /// or <see cref="byte"/>.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        [CLSCompliant(false)]
        public Amount(ulong value)
            : this((decimal)value)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct, based on a ISO 4217 Currency code.</summary>
        /// <param name="value">The Amount of money as <see langword="ulong"/>, <see langword="uint"/>, <see langword="ushort"/>
        /// or <see cref="byte"/>.</param>
        /// <param name="currency">A ISO 4217 Currency code, like EUR or USD.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        [CLSCompliant(false)]
        public Amount(ulong value, string currency)
            : this((decimal)value, Currency.FromCode(currency))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Amount"/> struct.</summary>
        /// <param name="value">The Amount of money as <see langword="ulong"/>, <see langword="uint"/>, <see langword="ushort"/>
        /// or <see cref="byte"/>.</param>
        /// <param name="currency">The Currency of the monetry instance.</param>
        /// <remarks>The integral types are implicitly converted to long and the result evaluates to decimal. Therefore you can
        /// initialize a Money object using an integer literal, without the suffix, as follows:
        /// <code>Money money = new Money(10, "EUR");</code></remarks>
        [CLSCompliant(false)]
        public Amount(ulong value, Currency currency)
            : this((decimal)value, currency)
        {
        }

        /// <summary>Gets the value of the monetry instance.</summary>
        public decimal Value { get; private set; }

        /// <summary>Gets the <see cref="Currency"/> of the monetry instance.</summary>
        public Currency Currency { get; private set; }

        /// <summary>Returns a value indicating whether this instance and a specified <see cref="Amount"/> object represent the same
        /// value.</summary>
        /// <param name="other">A <see cref="Amount"/> object.</param>
        /// <returns>true if value is equal to this instance; otherwise, false.</returns>
        public bool Equals(Amount other) => Value == other.Value && Currency == other.Currency;

        /// <summary>Returns a value indicating whether this instance and a specified <see cref="object"/> represent the same type
        /// and value.</summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns>true if value is equal to this instance; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is Amount money && Equals(money);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + Value.GetHashCode();
                return (hash * 23) + Currency.GetHashCode();
            }
        }

        /// <summary>Deconstructs the current instance into its components.</summary>
        /// <param name="amount">The Amount of money as <see langword="decimal"/>.</param>
        /// <param name="currency">The Currency of the money.</param>
        public void Deconstruct(out decimal amount, out Currency currency)
        {
            amount = Value;
            currency = Currency;
        }

        private static decimal Round(decimal amount, Currency currency, MidpointRounding rounding)
        {
            return currency.DecimalDigits switch
            {
                DefaultCurrencies.NotApplicable => Math.Round(amount),
                DefaultCurrencies.Z07 => Math.Round(amount / 0.2m, 0, rounding) * 0.2m,// divided into five subunits rather than by a power of ten. 5 is 10 to the power of log(5) = 0.69897...
                _ => Math.Round(amount, (int)currency.DecimalDigits, rounding),
            };
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1305:SpecifyIFormatProvider",
            MessageId = "System.String.Format(System.String,System.Object[])",
            Justification = "Test fail when Invariant is used. Inline JIT bug? When cloning CultureInfo it works.")]
        private static void AssertIsSameCurrency(Amount left, Amount right)
        {
            if (left.Currency != right.Currency)
            {
                throw new InvalidCurrencyException(left.Currency, right.Currency);
            }
        }
    }
}
