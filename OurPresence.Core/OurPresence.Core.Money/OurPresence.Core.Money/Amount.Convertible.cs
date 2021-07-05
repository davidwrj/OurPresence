// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Core.Money
{
    /// <summary>Represents Money, an amount defined in a specific Currency.</summary>
    public partial struct Amount
#if !NETSTANDARD1_3
        // : IConvertible
#endif
    {
        /// <summary>Performs an explicit conversion from <see cref="Amount"/> to <see cref="double"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Amount money) => Convert.ToDouble(money.Value);

        /// <summary>Performs an explicit conversion from <see cref="Amount"/> to <see cref="long"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator long(Amount money) => Convert.ToInt64(money.Value);

        /// <summary>Performs an explicit conversion from <see cref="Amount"/> to <see cref="decimal"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator decimal(Amount money) => money.Value;

        /// <summary>Performs an implicit conversion from <see cref="long"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Amount(long money) => new(money);

        /// <summary>Performs an implicit conversion from <see cref="ulong"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Amount(ulong money) => new(money);

        /// <summary>Performs an implicit conversion from <see cref="byte"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Amount(byte money) => new(money);

        /// <summary>Performs an implicit conversion from <see cref="ushort"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Amount(ushort money) => new(money);

        /// <summary>Performs an implicit conversion from <see cref="uint"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Amount(uint money) => new(money);

        /// <summary>Performs an implicit conversion from <see cref="double"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Amount(double money) => new((decimal)money);

        /// <summary>Performs an implicit conversion from <see cref="decimal"/> to <see cref="Amount"/>.</summary>
        /// <param name="money">The money.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Amount(decimal money) => new(money);

        /// <summary>Converts the value of this instance to an <see cref="float"/>.</summary>
        /// <param name="money">A <see cref="Amount"/> value.</param>
        /// <returns>The value of the <see cref="Amount"/> instance, converted to a <see cref="float"/>.</returns>
        /// <remarks>Because a <see cref="float"/> has fewer significant digits than a <see cref="Amount"/> value, this operation may
        /// produce round-off errors. Also the <see cref="Currency"/> information is lost.</remarks>
        public static float ToSingle(Amount money) => Convert.ToSingle(money.Value);

        /// <summary>Converts the value of this instance to an <see cref="double"/>.</summary>
        /// <param name="money">A <see cref="Amount"/> value.</param>
        /// <returns>The value of the current instance, converted to a <see cref="double"/>.</returns>
        /// <remarks>Because a Double has fewer significant digits than a <see cref="Amount"/> value, this operation may produce round-off
        /// errors.</remarks>
        public static double ToDouble(Amount money) => Convert.ToDouble(money.Value);

        /// <summary>Converts the value of this instance to an <see cref="decimal"/>.</summary>
        /// <param name="money">A <see cref="Amount"/> value.</param>
        /// <returns>The value of the <see cref="Amount"/> instance, converted to a <see cref="decimal"/>.</returns>
        /// <remarks>The <see cref="Currency"/> information is lost.</remarks>
        public static decimal ToDecimal(Amount money) => money.Value;

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="long"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="long"/> instance, converted to a <see cref="Amount"/>.</returns>
        public static Amount FromInt64(long money)
        {
            return money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="ulong"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="ulong"/> instance, converted to a <see cref="Amount"/>.</returns>
        [CLSCompliant(false)]
        public static Amount FromUInt64(ulong money)
        {
            return money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="byte"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="byte"/> instance, converted to a <see cref="Amount"/>.</returns>
        public static Amount FromByte(long money)
        {
            return money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="ushort"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="ushort"/> instance, converted to a <see cref="Amount"/>.</returns>
        [CLSCompliant(false)]
        public static Amount FromUInt16(ushort money)
        {
            return money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="uint"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="uint"/> instance, converted to a <see cref="Amount"/>.</returns>
        [CLSCompliant(false)]
        public static Amount FromUInt32(uint money)
        {
            return money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="double"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="double"/> instance, converted to a <see cref="Amount"/>.</returns>
        public static Amount FromDouble(double money)
        {
            return (Amount)money;
        }

        /// <summary>Converts the value to a <see cref="Amount"/> struct, based on the current culture.</summary>
        /// <param name="money">The <see cref="decimal"/> value on which the returned <see cref="Amount"/> should be based.</param>
        /// <returns>The value of the <see cref="decimal"/> instance, converted to a <see cref="Amount"/>.</returns>
        public static Amount FromDecimal(decimal money)
        {
            return money;
        }

#if !NETSTANDARD1_3
#pragma warning disable CA1822 // Mark members as static => Needed for implimentation of IConvertible
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        public TypeCode GetTypeCode() => TypeCode.Object;
#pragma warning restore CA1822 // Mark members as static
#endif

        /// <summary>Converts the value of this instance to an equivalent Boolean value using the specified culture-specific
        /// formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A Boolean value equivalent to the value of this instance.</returns>
        public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
        public byte ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent Unicode character using the specified culture-specific
        /// formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A Unicode character equivalent to the value of this instance.</returns>
        public char ToChar(IFormatProvider provider) => Convert.ToChar(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent <see cref="DateTime"/> using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A <see cref="DateTime"/> instance equivalent to the value of this instance.</returns>
        public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent <see cref="decimal"/> number using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A <see cref="decimal"/> number equivalent to the value of this instance.</returns>
        public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent double-precision floating-point number using the
        /// specified culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
        public double ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 16-bit signed integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
        public short ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 32-bit signed integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
        public int ToInt32(IFormatProvider provider) => Convert.ToInt32(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 64-bit signed integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
        public long ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific
        /// formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
        [CLSCompliant(false)]
        public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent single-precision floating-point number using the
        /// specified culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
        public float ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);

        /// <summary>Converts the value of this instance to an <see cref="object"/> of the specified<see cref="Type"/> that has an equivalent value, using the specified culture-specific formatting information.</summary>
        /// <param name="conversionType">The <see cref="Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An <see cref="object"/> instance of type <paramref name="conversionType"/> whose value is equivalent
        /// to the value of this instance.</returns>
        public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(this, conversionType, provider);

        /// <summary>Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
        [CLSCompliant(false)]
        public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
        [CLSCompliant(false)]
        public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);

        /// <summary>Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified
        /// culture-specific formatting information.</summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
        [CLSCompliant(false)]
        public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);
    }
}
