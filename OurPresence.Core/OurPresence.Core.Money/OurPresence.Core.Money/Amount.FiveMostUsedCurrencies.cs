using System;

namespace OurPresence.Core.Money
{
    /// <summary>Represents Money, an amount defined in a specific Currency.</summary>
    public partial struct Amount
    {
        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        public static Amount Euro(decimal amount) => new(amount, Currency.FromCode("EUR"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        public static Amount Euro(decimal amount, MidpointRounding rounding) => new(amount, Currency.FromCode("EUR"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public static Amount Euro(double amount) => new(amount, Currency.FromCode("EUR"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>).</para></remarks>
        public static Amount Euro(double amount, MidpointRounding rounding) => new(amount, Currency.FromCode("EUR"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        public static Amount Euro(long amount) => new(amount, Currency.FromCode("EUR"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in euro's.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <returns>A <see cref="Amount"/> structure with EUR as <see cref="Currency"/>.</returns>
        [CLSCompliant(false)]
        public static Amount Euro(ulong amount) => new(amount, Currency.FromCode("EUR"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in US dollar.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        public static Amount USDollar(decimal amount) => new(amount, Currency.FromCode("USD"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        public static Amount USDollar(decimal amount, MidpointRounding rounding) => new(amount, Currency.FromCode("USD"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in US dollar.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public static Amount USDollar(double amount) => new(amount, Currency.FromCode("USD"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in US dollar.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>).</para></remarks>
        public static Amount USDollar(double amount, MidpointRounding rounding) => new(amount, Currency.FromCode("USD"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in US dollar.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        public static Amount USDollar(long amount) => new(amount, Currency.FromCode("USD"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in US dollars.</summary>
        /// <param name="amount">The Amount of money in US dollar.</param>
        /// <returns>A <see cref="Amount"/> structure with USD as <see cref="Currency"/>.</returns>
        [CLSCompliant(false)]
        public static Amount USDollar(ulong amount) => new(amount, Currency.FromCode("USD"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yen.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        public static Amount Yen(decimal amount) => new(amount, Currency.FromCode("JPY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yens.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        public static Amount Yen(decimal amount, MidpointRounding rounding) => new(amount, Currency.FromCode("JPY"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yen.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public static Amount Yen(double amount) => new(amount, Currency.FromCode("JPY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yen.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>).</para></remarks>
        public static Amount Yen(double amount, MidpointRounding rounding) => new(amount, Currency.FromCode("JPY"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yen.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        public static Amount Yen(long amount) => new(amount, Currency.FromCode("JPY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Japanese Yens.</summary>
        /// <param name="amount">The Amount of money in Japanese Yen.</param>
        /// <returns>A <see cref="Amount"/> structure with JPY as <see cref="Currency"/>.</returns>
        [CLSCompliant(false)]
        public static Amount Yen(ulong amount) => new(amount, Currency.FromCode("JPY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in Pound Sterling.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        public static Amount PoundSterling(decimal amount) => new(amount, Currency.FromCode("GBP"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in euro.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        public static Amount PoundSterling(decimal amount, MidpointRounding rounding) => new(amount, Currency.FromCode("GBP"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in Pound Sterling.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public static Amount PoundSterling(double amount) => new(amount, Currency.FromCode("GBP"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in Pound Sterling.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>).</para></remarks>
        public static Amount PoundSterling(double amount, MidpointRounding rounding) => new(amount, Currency.FromCode("GBP"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in Pound Sterling.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        public static Amount PoundSterling(long amount) => new(amount, Currency.FromCode("GBP"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in British pounds.</summary>
        /// <param name="amount">The Amount of money in Pound Sterling.</param>
        /// <returns>A <see cref="Amount"/> structure with GBP as <see cref="Currency"/>.</returns>
        [CLSCompliant(false)]
        public static Amount PoundSterling(ulong amount) => new(amount, Currency.FromCode("GBP"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        public static Amount Yuan(decimal amount) => new(amount, Currency.FromCode("CNY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        public static Amount Yuan(decimal amount, MidpointRounding rounding) => new(amount, Currency.FromCode("CNY"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>). As rounding mode, MidpointRounding.ToEven is used
        /// (<see cref="MidpointRounding"/>). The behavior of this method follows IEEE Standard 754, section 4. This
        /// kind of rounding is sometimes called rounding to nearest, or banker's rounding. It minimizes rounding errors that
        /// result from consistently rounding a midpoint value in a single direction.</para></remarks>
        public static Amount Yuan(double amount) => new(amount, Currency.FromCode("CNY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <param name="rounding">The rounding.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        /// <remarks>This method will first convert to decimal by rounding the value to 15 significant digits using rounding
        /// to nearest. This is done even if the number has more than 15 digits and the less significant digits are zero.
        /// <para>The amount will be rounded to the number of decimal digits of the specified currency
        /// (<see cref="Currency.DecimalDigits"/>).</para></remarks>
        public static Amount Yuan(double amount, MidpointRounding rounding) => new(amount, Currency.FromCode("CNY"), rounding);

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        public static Amount Yuan(long amount) => new(amount, Currency.FromCode("CNY"));

        /// <summary>Initializes a new instance of the <see cref="Amount"/> structure in Chinese Yuan.</summary>
        /// <param name="amount">The Amount of money in Chinese Yuan.</param>
        /// <returns>A <see cref="Amount"/> structure with CNY as <see cref="Currency"/>.</returns>
        [CLSCompliant(false)]
        public static Amount Yuan(ulong amount) => new(amount, Currency.FromCode("CNY"));
    }
}
