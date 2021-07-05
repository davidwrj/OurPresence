// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Core.Money.Extensions
{
    /// <summary>
    /// Compares two double values
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Check if 2 double values are nearly the equal, within .00001 of ewach other.
        /// </summary>
        /// <param name="initialValue">The first value</param>
        /// <param name="value">The second value</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool IsApproximatelyEqualTo(this double initialValue, double value)
        {
            return IsApproximatelyEqualTo(initialValue, value, 0.00001);
        }

        /// <summary>
        /// Check if 2 double values are nearly the equal.
        /// </summary>
        /// <param name="initialValue">The first value</param>
        /// <param name="value">The second value</param>
        /// <param name="maximumDifferenceAllowed">The amount of difference allowed.</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed)
        {
            // Handle comparisons of floating point values that may not be exactly the same
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }
    }
}
