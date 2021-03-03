using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OurPresence.Core.Money.Extensions
{
    /// <summary>Extensions for <see cref="Amount"/>.</summary>
    public static class AmountExtensions
    {
        /// <summary>Divide the Money in equal shares, without losing Money.</summary>
        /// <param name="money">The <see cref="Amount"/> instance.</param>
        /// <param name="shares">The number of shares to divide in.</param>
        /// <returns>An <see cref="IEnumerable{Money}"/> of Money.</returns>
        /// <exception cref="ArgumentOutOfRangeException">shares;Number of shares must be greater than 1.</exception>
        /// <remarks>As rounding mode, MidpointRounding.ToEven is used (<seealso cref="MidpointRounding"/>).
        /// The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called
        /// rounding to nearest, or banker's rounding. It minimizes rounding errors that result from consistently rounding a
        /// midpoint value in a single direction.</remarks>
        public static IEnumerable<Amount> SafeDivide(this Amount money, int shares) => SafeDivide(money, shares, MidpointRounding.ToEven);

        /// <summary>Divide the Money in equal shares, without losing Money.</summary>
        /// <param name="money">The <see cref="Amount"/> instance.</param>
        /// <param name="shares">The number of shares to divide in.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <returns>An <see cref="IEnumerable{Money}"/> of Money.</returns>
        /// <exception cref="ArgumentOutOfRangeException">shares;Number of shares must be greater than 1.</exception>
        [SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "number-1", Justification = "Can't be lower than zero.")]
        public static IEnumerable<Amount> SafeDivide(this Amount money, int shares, MidpointRounding rounding)
        {
            return shares <= 1
                ? throw new ArgumentOutOfRangeException(nameof(shares), $"Number of {nameof(shares)} must be greater than 1")
                : SafeDivideIterator();

            IEnumerable<Amount> SafeDivideIterator()
            {
                var shareAmount = Math.Round(money.Value / shares, (int)money.Currency.DecimalDigits, rounding);
                var remainder = money.Value;

                for (var i = 0; i < shares - 1; i++)
                {
                    remainder -= shareAmount;
                    yield return new Amount(shareAmount, money.Currency);
                }

                yield return new Amount(remainder, money.Currency);
            }
        }

        /// <summary>Divide the Money in shares with a specific ratio, without losing Money.</summary>
        /// <param name="money">The <see cref="Amount"/> instance.</param>
        /// <param name="ratios">The number of shares as an array of ratios.</param>
        /// <returns>An <see cref="IEnumerable{Money}"/> of Money.</returns>
        /// <exception cref="ArgumentOutOfRangeException">ratios;Sum of ratios must be greater than 1.</exception>
        /// <remarks>As rounding mode, MidpointRounding.ToEven is used (<seealso cref="MidpointRounding"/>).
        /// The behavior of this method follows IEEE Standard 754, section 4. This kind of rounding is sometimes called
        /// rounding to nearest, or banker's rounding. It minimizes rounding errors that result from consistently rounding a
        /// midpoint value in a single direction.</remarks>
        public static IEnumerable<Amount> SafeDivide(this Amount money, int[] ratios) => SafeDivide(money, ratios, MidpointRounding.ToEven);

        /// <summary>Divide the Money in shares with a specific ratio, without losing Money.</summary>
        /// <param name="money">The <see cref="Amount"/> instance.</param>
        /// <param name="ratios">The number of shares as an array of ratios.</param>
        /// <param name="rounding">The rounding mode.</param>
        /// <returns>An <see cref="IEnumerable{Money}"/> of Money.</returns>
        /// <exception cref="ArgumentOutOfRangeException">shares;Number of shares must be greater than 1.</exception>
        public static IEnumerable<Amount> SafeDivide(this Amount money, int[] ratios, MidpointRounding rounding)
        {
            return ratios.Any(ratio => ratio < 1)
                ? throw new ArgumentOutOfRangeException(nameof(ratios), $"All {nameof(ratios)} must be greater or equal than 1")
                : SafeDivideIterator();

            IEnumerable<Amount> SafeDivideIterator()
            {
                var remainder = money.Value;

                for (var i = 0; i < ratios.Length - 1; i++)
                {
                    var ratioAmount = Math.Round(
                        money.Value * ratios[i] / ratios.Sum(),
                        (int)money.Currency.DecimalDigits,
                        rounding);

                    remainder -= ratioAmount;

                    yield return new Amount(ratioAmount, money.Currency);
                }

                yield return new Amount(remainder, money.Currency);
            }
        }
    }
}
