// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Liquid.Tests.Util
{
    public class ExpressionUtilityTest
    {
        private Dictionary<Type, (double, double)> typeLimits;

        public ExpressionUtilityTest()
        {
            typeLimits = new Dictionary<Type, (double, double)>()
            {
                { typeof(decimal), (Convert.ToDouble(decimal.MaxValue), Convert.ToDouble(decimal.MinValue) ) },
                { typeof(double), (double.MaxValue, double.MinValue ) },
                { typeof(float), (Convert.ToDouble(float.MaxValue), Convert.ToDouble(float.MinValue) ) },
                { typeof(int), (Convert.ToDouble(int.MaxValue), Convert.ToDouble(int.MinValue) ) },
                { typeof(uint), (Convert.ToDouble(uint.MaxValue), Convert.ToDouble(uint.MinValue) ) },
                { typeof(long), (Convert.ToDouble(long.MaxValue), Convert.ToDouble(long.MinValue) ) },
                { typeof(ulong), (Convert.ToDouble(ulong.MaxValue), Convert.ToDouble(ulong.MinValue) ) },
                { typeof(short), (Convert.ToDouble(short.MaxValue), Convert.ToDouble(short.MinValue) ) },
                { typeof(ushort), (Convert.ToDouble(ushort.MaxValue), Convert.ToDouble(ushort.MinValue) ) },
                { typeof(byte), (Convert.ToDouble(byte.MaxValue), Convert.ToDouble(byte.MinValue) ) },
                { typeof(sbyte), (Convert.ToDouble(sbyte.MaxValue), Convert.ToDouble(sbyte.MinValue) ) }
            };
        }

        public static IEnumerable<object[]> GetNumericCombinations()
        {
            var testTypes = new HashSet<Type> { typeof(decimal), typeof(double), typeof(float), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte) };
            var testAgainst = new HashSet<Type>(testTypes.ToArray());

            foreach (var t1 in testTypes)
            {
                foreach (var t2 in testAgainst)
                {
                    yield return (new[] { t1, t2 });
                }
                testAgainst.Remove(t1); // All combinations are tested, no need to test other objects against it.
            }
        }

        //[Theory, MemberData("GetNumericCombinations")]
        //public void TestNumericCombinationsResultInUpgrade(IEnumerable<object[]> types)
        //{
        //    var t1 = types.First();
        //    var t2 = types.ElementAt(1);
        //    var result = Modeller.Liquid.Util.ExpressionUtility.BinaryNumericResultType(t1, t2);
        //    Assert.NotNull(result);
        //    Assert.Equal(result, Modeller.Liquid.Util.ExpressionUtility.BinaryNumericResultType(t2, t1));
        //    Assert.True(typeLimits[result].Item1 >= typeLimits[t1].Item1);
        //    Assert.True(typeLimits[result].Item1 >= typeLimits[t2].Item1);
        //    Assert.True(typeLimits[result].Item2 <= typeLimits[t1].Item2);
        //    Assert.True(typeLimits[result].Item2 <= typeLimits[t1].Item2);
        //}
    }
}
