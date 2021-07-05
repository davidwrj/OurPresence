// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Core.Money
{
    /// <summary>Represents Money, an amount defined in a specific Currency.</summary>
    public partial struct Amount : IEquatable<Amount>
    {
        /// <summary>Adds two specified <see cref="Amount"/> values.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of adding left and right.</returns>
        public static Amount operator +(Amount left, Amount right) => Add(left, right);

        /// <summary>Add the <see cref="Amount"/> value with the given value.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="decimal"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of adding left and right.</returns>
        public static Amount operator +(Amount left, decimal right) => Add(left, right);

        /// <summary>Add the <see cref="Amount"/> value with the given value.</summary>
        /// <param name="left">A <see cref="decimal"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of adding left and right.</returns>
        public static Amount operator +(decimal left, Amount right) => Add(right, left);

        /// <summary>Subtracts two specified <see cref="Amount"/> values.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of subtracting right from left.</returns>
        public static Amount operator -(Amount left, Amount right) => Subtract(left, right);

        /// <summary>Subtracts <see cref="Amount"/> value with the given value.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="decimal"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of subtracting right from left.</returns>
        public static Amount operator -(Amount left, decimal right) => Subtract(left, right);

        /// <summary>Subtracts <see cref="Amount"/> value with the given value.</summary>
        /// <param name="left">A <see cref="decimal"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of subtracting right from left.</returns>
        public static Amount operator -(decimal left, Amount right) => Subtract(right, left);

        /// <summary>Multiplies the <see cref="Amount"/> value by the given value.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="decimal"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of multiplying right with left.</returns>
        public static Amount operator *(Amount left, decimal right) => Multiply(left, right);

        /// <summary>Multiplies the <see cref="Amount"/> value by the given value.</summary>
        /// <param name="left">A <see cref="decimal"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of multiplying left with right.</returns>
        public static Amount operator *(decimal left, Amount right) => Multiply(right, left);

        /// <summary>Divides the <see cref="Amount"/> value by the given value.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="decimal"/> object on the right side.</param>
        /// <returns>The <see cref="Amount"/> result of dividing left with right.</returns>
        /// <remarks>This division can lose money. Use <see cref="Extensions.AmountExtensions.SafeDivide(Amount, int)"/> to do a safe division.</remarks>
        public static Amount operator /(Amount left, decimal right) => Divide(left, right);

        /// <summary>Divides the <see cref="Amount"/> value by the given value.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>The <see cref="decimal"/> result of dividing left with right.</returns>
        /// <remarks>Division of Money by Money, means the unit is lost, so the result will be a ratio <see cref="decimal"/>.</remarks>
        public static decimal operator /(Amount left, Amount right) => Divide(left, right);

        /// <summary>Adds two specified <see cref="Amount"/> values.</summary>
        /// <param name="money1">The first <see cref="Amount"/> object.</param>
        /// <param name="money2">The second <see cref="Amount"/> object.</param>
        /// <returns>A <see cref="Amount"/> object with the values of both <see cref="Amount"/> objects added.</returns>
        public static Amount Add(Amount money1, Amount money2)
        {
            AssertIsSameCurrency(money1, money2);
            return new Amount(decimal.Add(money1.Value, money2.Value), money1.Currency);
        }

        /// <summary>Adds two specified <see cref="Amount"/> values.</summary>
        /// <param name="money1">The first <see cref="Amount"/> object.</param>
        /// <param name="money2">The second <see cref="decimal"/> object.</param>
        /// <returns>A <see cref="Amount"/> object with the values of both <see cref="decimal"/> objects added.</returns>
        public static Amount Add(Amount money1, decimal money2) => new(decimal.Add(money1.Value, money2), money1.Currency);

        /// <summary>Subtracts one specified <see cref="Amount"/> value from another.</summary>
        /// <param name="money1">The first <see cref="Amount"/> object.</param>
        /// <param name="money2">The second <see cref="Amount"/> object.</param>
        /// <returns>A <see cref="Amount"/> object where the second <see cref="Amount"/> object is subtracted from the first.</returns>
        public static Amount Subtract(Amount money1, Amount money2)
        {
            AssertIsSameCurrency(money1, money2);
            return new Amount(decimal.Subtract(money1.Value, money2.Value), money1.Currency);
        }

        /// <summary>Subtracts one specified <see cref="Amount"/> value from another.</summary>
        /// <param name="money1">The first <see cref="Amount"/> object.</param>
        /// <param name="money2">The second <see cref="decimal"/> object.</param>
        /// <returns>A <see cref="Amount"/> object where the second <see cref="decimal"/> object is subtracted from the first.</returns>
        public static Amount Subtract(Amount money1, decimal money2) => new(decimal.Subtract(money1.Value, money2), money1.Currency);

        /// <summary>Multiplies the specified money.</summary>
        /// <param name="money">The money.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The result as <see cref="Amount"/> after multiplying.</returns>
        public static Amount Multiply(Amount money, decimal multiplier) => new(decimal.Multiply(money.Value, multiplier), money.Currency);

        /// <summary>Divides the specified money.</summary>
        /// <param name="money">The money.</param>
        /// <param name="divisor">The divider.</param>
        /// <returns>The division as <see cref="Amount"/>.</returns>
        /// <remarks>This division can lose money. Use <see cref="Extensions.AmountExtensions.SafeDivide(Amount, int)"/> to do a safe division.</remarks>
        public static Amount Divide(Amount money, decimal divisor) => new(decimal.Divide(money.Value, divisor), money.Currency);

        /// <summary>Divides the specified money.</summary>
        /// <param name="money1">The money.</param>
        /// <param name="money2">The divider.</param>
        /// <returns>The <see cref="decimal"/> result of dividing left with right.</returns>
        /// <remarks>Division of Money by Money, means the unit is lost, so the result will be Decimal.</remarks>
        public static decimal Divide(Amount money1, Amount money2)
        {
            AssertIsSameCurrency(money1, money2);
            return decimal.Divide(money1.Value, money2.Value);
        }
    }
}