// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace OurPresence.Core.Money
{
    /// <summary>A conversion of money of one currency into money of another currency.</summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Exchange_rate .</remarks>
    public struct ExchangeRate : IEquatable<ExchangeRate>
    {
        /// <summary>Initializes a new instance of the <see cref="ExchangeRate"/> struct.</summary>
        /// <param name="baseCurrency">The base currency.</param>
        /// <param name="quoteCurrency">The quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        /// <exception cref="ArgumentNullException">The value of 'baseCurrency' or 'quoteCurrency' cannot be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Rate must be greater than zero.</exception>
        /// <exception cref="ArgumentException">The base and quote currency can't be equal.</exception>
        public ExchangeRate(Currency baseCurrency, Currency quoteCurrency, decimal rate)
            : this()
        {
            if (baseCurrency == default)
            {
                throw new ArgumentNullException(nameof(baseCurrency));
            }

            if (rate < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be greater than zero.");
            }

            if (quoteCurrency == default)
            {
                throw new ArgumentNullException(nameof(quoteCurrency));
            }

            if (baseCurrency == quoteCurrency)
            {
                throw new ArgumentException("The base and quote currency can't be equal.");
            }

            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            Value = rate; // value is a ratio
        }

        /// <summary>Initializes a new instance of the <see cref="ExchangeRate"/> struct.</summary>
        /// <param name="baseCurrency">The base currency.</param>
        /// <param name="quoteCurrency">The quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        /// <param name="numberOfDecimals">The number of decimals to round the exchange rate to.</param>
        public ExchangeRate(Currency baseCurrency, Currency quoteCurrency, double rate, int numberOfDecimals = 6)
            : this(baseCurrency, quoteCurrency, Math.Round((decimal)rate, numberOfDecimals))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExchangeRate"/> struct.</summary>
        /// <param name="baseCode">The code of the base currency.</param>
        /// <param name="quoteCode">The code of the quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        public ExchangeRate(string baseCode, string quoteCode, decimal rate)
            : this(Currency.FromCode(baseCode), Currency.FromCode(quoteCode), rate)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExchangeRate"/> struct.</summary>
        /// <param name="baseCode">The code of the base currency.</param>
        /// <param name="quoteCode">The code of the quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        public ExchangeRate(string baseCode, string quoteCode, double rate)
            : this(Currency.FromCode(baseCode), Currency.FromCode(quoteCode), rate, 6)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ExchangeRate"/> struct.</summary>
        /// <param name="baseCode">The code of the base currency.</param>
        /// <param name="quoteCode">The code of the quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        /// <param name="numberOfDecimals">The number of decimals to round the exchange rate to.</param>
        public ExchangeRate(string baseCode, string quoteCode, double rate, int numberOfDecimals)
            : this(Currency.FromCode(baseCode), Currency.FromCode(quoteCode), rate, numberOfDecimals)
        {
        }

        /// <summary>Gets the base currency.</summary>
        /// <value>The base currency.</value>
        public Currency BaseCurrency { get; private init; }

        /// <summary>Gets the quote currency.</summary>
        /// <value>The quote currency.</value>
        public Currency QuoteCurrency { get; private init; }

        /// <summary>Gets the value of the exchange rate.</summary>
        /// <value>The value of the exchange rate.</value>
        public decimal Value { get; private init; }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="left">The left ExchangeRate.</param>
        /// <param name="right">The right ExchangeRate.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ExchangeRate left, ExchangeRate right) => left.Equals(right);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="left">The left ExchangeRate.</param>
        /// <param name="right">The right ExchangeRate.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ExchangeRate left, ExchangeRate right) => !(left == right);

        /// <summary>Converts the string representation of an exchange rate to its <see cref="ExchangeRate"/> equivalent.</summary>
        /// <param name="rate">The string representation of the exchange rate to convert.</param>
        /// <returns>The equivalent to the exchange rate contained in rate.</returns>
        /// <exception cref="FormatException">rate is not in the correct format.</exception>
        public static ExchangeRate Parse(string? rate)
        {
            if (rate is null)
                throw new ArgumentNullException(nameof(rate));
            return !TryParse(rate, out var fx)
                ? throw new FormatException("Rate is not in the correct format. Currencies are the same or the rate is not a number.")
                : fx;
        }

        /// <summary>Converts the string representation of an exchange rate to its <see cref="ExchangeRate"/> equivalent. A return
        /// value indicates whether the conversion succeeded or failed.</summary>
        /// <param name="rate">The string representation of the exchange rate to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="ExchangeRate"/> that is equivalent to the exchange rate contained in
        /// rate, if the conversion succeeded,
        /// or is zero if the conversion failed. The conversion fails if the rate parameter is null, is not a exchange rate in a
        /// valid format, or represents a number less than MinValue
        /// or greater than MaxValue. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if rate was converted successfully; otherwise, <b>false</b>.</returns>
        public static bool TryParse(string rate, out ExchangeRate result)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rate))
                {
                    throw new ArgumentNullException(nameof(rate));
                }

                rate = rate.Trim();
                var baseCurrency = Currency.FromCode(rate.Substring(0, 3));
                var index = rate.Substring(3, 1) == "/" ? 4 : 3;
                var quoteCurrency = Currency.FromCode(rate.Substring(index, 3));
                var value = decimal.Parse(rate.Remove(0, index + 3), NumberFormatInfo.CurrentInfo);

                result = new ExchangeRate(baseCurrency, quoteCurrency, value);
                return true;
            }
            catch (Exception ex)
            {
                if (!(ex is FormatException) && !(ex is OverflowException) && !(ex is ArgumentException))
                {
                    throw;
                }

                result = new ExchangeRate
                         {
                                 BaseCurrency = Currency.FromCode("XXX"),
                                 QuoteCurrency = Currency.FromCode("XXX"),
                                 Value = 0,
                         };

                return false;
            }
        }

        /// <summary>Deconstructs the current instance into its components.</summary>
        /// <param name="baseCurrency">The base currency.</param>
        /// <param name="quoteCurrency">The quote currency.</param>
        /// <param name="rate">The rate of the exchange.</param>
        public void Deconstruct(out Currency baseCurrency, out Currency quoteCurrency, out decimal rate)
        {
            baseCurrency = BaseCurrency;
            quoteCurrency = QuoteCurrency;
            rate = Value;
        }

        /// <summary>Converts the specified money.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The converted money.</returns>
        /// <exception cref="ArgumentException">Money should have the same currency as the base currency or the quote
        /// currency.</exception>
        public Amount Convert(Amount money)
        {
            if (money.Currency != BaseCurrency && money.Currency != QuoteCurrency)
            {
                throw new ArgumentException(
                    "Money should have the same currency as the base currency or the quote currency.",
                    nameof(money));
            }

            return money.Currency == BaseCurrency
                       ? new Amount(money.Value * Value, QuoteCurrency)
                       : new Amount(money.Value / Value, BaseCurrency);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() 
            => Value.GetHashCode() + 397 * BaseCurrency.GetHashCode() + 397 * QuoteCurrency.GetHashCode();

        /// <summary>Indicates whether this instance and a specified <see cref="ExchangeRate"/> are equal.</summary>
        /// <param name="other">Another object to compare to.</param>
        /// <returns>true if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise,
        /// false.</returns>
        public bool Equals(ExchangeRate other)
            => Value == other.Value && BaseCurrency == other.BaseCurrency && QuoteCurrency == other.QuoteCurrency;

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise,
        /// false.</returns>
        public override bool Equals(object? obj) 
            => obj is ExchangeRate fx && Equals(fx);

        /// <summary>Converts this <see cref="ExchangeRate"/> instance to its equivalent <see cref="string"/> representation.</summary>
        /// <returns>A string that represents this <see cref="ExchangeRate"/> instance.</returns>
        /// <remarks>See http://en.wikipedia.org/wiki/Currency_Pair for more info about how an ExchangeRate can be presented.</remarks>
        public override string ToString() 
            => $"{BaseCurrency.Code}/{QuoteCurrency.Code} {Value}";
    }
}
