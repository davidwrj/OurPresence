using System;

namespace OurPresence.Core.Money
{
    /// <summary>Represents Money, an amount defined in a specific Currency.</summary>
    public partial struct Amount : IComparable, IComparable<Amount>
    {
        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is equal to another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is equal to right; otherwise, false.</returns>
        public static bool operator ==(Amount left, Amount right) => Equals(left, right);

        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is not equal to another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is not equal to right; otherwise, false.</returns>
        public static bool operator !=(Amount left, Amount right) => !Equals(left, right);

        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is less than another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is less than right; otherwise, false.</returns>
        public static bool operator <(Amount left, Amount right) => Compare(left, right) < 0;

        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is greater than another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is greater than right; otherwise, false.</returns>
        public static bool operator >(Amount left, Amount right) => Compare(left, right) > 0;

        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is less than or equal to another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is less than or equal to right; otherwise, false.</returns>
        public static bool operator <=(Amount left, Amount right) => Compare(left, right) <= 0;

        /// <summary>Returns a value indicating whether a specified <see cref="Amount"/> is greater than or equal to another specified <see cref="Amount"/>.</summary>
        /// <param name="left">A <see cref="Amount"/> object on the left side.</param>
        /// <param name="right">A <see cref="Amount"/> object on the right side.</param>
        /// <returns>true if left is greater than or equal to right; otherwise, false.</returns>
        public static bool operator >=(Amount left, Amount right) => Compare(left, right) >= 0;

        /// <summary>Compares two specified <see cref="Amount"/> values.</summary>
        /// <param name="left">The first <see cref="Amount"/> object.</param>
        /// <param name="right">The second <see cref="Amount"/> object.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value.
        /// <list type="table">
        /// <listheader>
        ///   <term>Return Value</term>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <description>This instance is less than value.</description>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <description>This instance is equal to value.</description>
        /// </item>
        /// <item>
        ///   <term>Greater than zero </term>
        ///   <description>This instance is greater than value.</description>
        /// </item>
        /// </list>
        /// </returns>
        public static int Compare(Amount left, Amount right) => left.CompareTo(right);

        /// <summary>Compares this instance to a specified <see cref="Amount"/> object.</summary>
        /// <param name="obj">A <see cref="Amount"/> object.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value.
        /// <list type="table">
        /// <listheader>
        ///   <term>Return Value</term>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <description>This instance is less than value.</description>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <description>This instance is equal to value.</description>
        /// </item>
        /// <item>
        ///   <term>Greater than zero </term>
        ///   <description>This instance is greater than value.</description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">object is not the same type as this instance.</exception>
        public int CompareTo(object? obj)
        {
            return obj == null
                ? 1
                : !(obj is Amount amount)
                ? throw new ArgumentException("obj is not the same type as this instance", nameof(obj))
                : CompareTo(amount);
        }

        /// <summary>Compares this instance to a specified <see cref="object"/>.</summary>
        /// <param name="other">An <see cref="object"/> or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value.
        /// <list type="table">
        /// <listheader>
        ///   <term>Return Value</term>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <term>Less than zero</term>
        ///   <description>This instance is less than value.</description>
        /// </item>
        /// <item>
        ///   <term>Zero</term>
        ///   <description>This instance is equal to value.</description>
        /// </item>
        /// <item>
        ///   <term>Greater than zero </term>
        ///   <description>This instance is greater than value.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(Amount other)
        {
            AssertIsSameCurrency(this, other);
            return Value.CompareTo(other.Value);
        }
    }
}
