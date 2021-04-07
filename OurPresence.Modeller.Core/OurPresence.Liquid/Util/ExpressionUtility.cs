using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OurPresence.Liquid.Util
{
    /// <summary>
    /// Some of this code was taken from http://www.yoda.arachsys.com/csharp/miscutil/usage/genericoperators.html.
    /// General purpose Expression utilities
    /// </summary>
    public static class ExpressionUtility
    {
        private static readonly Dictionary<Type, Type[]> s_numericTypePromotions;

        static ExpressionUtility()
        {
            s_numericTypePromotions = new Dictionary<Type, Type[]>();

            void Add(Type key, params Type[] types) => s_numericTypePromotions[key] = types;
            // Using the promotion table at
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/conversion-tables

            Add(typeof(Byte), typeof(UInt16), typeof(Int16), typeof(UInt32), typeof(Int32), typeof(UInt64), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(SByte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(Int16), typeof(Int32), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(UInt16), typeof(UInt32), typeof(Int32), typeof(UInt64), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(Char), typeof(UInt16), typeof(UInt32), typeof(Int32), typeof(UInt64), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(Int32), typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(UInt32), typeof(Int64), typeof(UInt64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(Int64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(UInt64), typeof(Decimal), typeof(Single), typeof(double));
            Add(typeof(Single), typeof(Double));
            Add(typeof(Decimal), typeof(Single), typeof(Double));
            Add(typeof(Double));

        }

        /// <summary>
        /// Perform the implicit conversions as set out in the C# spec docs at
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/conversion-tables
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Type BinaryNumericResultType(Type left, Type right)
        {
            if (left == right)
                return left;

            if (!s_numericTypePromotions.ContainsKey(left))
                throw new System.ArgumentException("Argument is not numeric", nameof(left));
            if (!s_numericTypePromotions.ContainsKey(right))
                throw new System.ArgumentException("Argument is not numeric", nameof(right));

            // Test left to right promotion
            if (s_numericTypePromotions[right].Contains(left))
                return left;
            if (s_numericTypePromotions[left].Contains(right))
                return right;
            return s_numericTypePromotions[right].First(p => s_numericTypePromotions[left].Contains(p));
        }

        private static (Expression left, Expression right) Cast(Expression lhs, Expression rhs,Type leftType, Type rightType, Type resultType)
        {
            var castLhs = leftType == resultType ? lhs : (Expression)Expression.Convert(lhs, resultType);
            var castRhs = rightType == resultType ? rhs : (Expression)Expression.Convert(rhs, resultType);
            return (castLhs, castRhs);
        }

        /// <summary>
        /// Create a function delegate representing a binary operation
        /// </summary>
        /// <param name="body">Body factory</param>
        /// <param name="leftType"></param>
        /// <param name="rightType"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns>Compiled function delegate</returns>
        public static Delegate CreateExpression
            (Func<Expression, Expression, BinaryExpression> body
             , Type leftType
             , Type rightType)
        {
            var lhs = Expression.Parameter(leftType, "lhs");
            var rhs = Expression.Parameter(rightType, "rhs");
            try
            {
                try
                {
                    var resultType = BinaryNumericResultType( leftType, rightType );
                    var (castLhs, castRhs) = Cast( lhs, rhs, leftType, rightType, resultType );
                    return Expression.Lambda(body(castLhs, castRhs), lhs, rhs).Compile();
                }
                catch (InvalidOperationException)
                {
                    try
                    {
                        var resultType = leftType;
                        var (castLhs, castRhs) = Cast( lhs, rhs, leftType, rightType, resultType );
                        return Expression.Lambda( body( castLhs, castRhs ), lhs, rhs ).Compile();
                    }
                    catch (InvalidOperationException)
                    {
                        var resultType = rightType;
                        var (castLhs, castRhs) = Cast( lhs, rhs, leftType, rightType, resultType );
                        return Expression.Lambda( body( castLhs, castRhs ), lhs, rhs ).Compile();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message; // avoid capture of ex itself
                return Expression.Lambda(Expression.Throw(Expression.Constant(new InvalidOperationException(msg))), lhs, rhs).Compile();
            }
        }
    }
}
