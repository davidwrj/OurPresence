// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace OurPresence.Modeller.Liquid.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IEnumerable Flatten(this IEnumerable array)
        {
            foreach (var item in array)
            {
                if (item is string || !(item is IEnumerable))
                {
                    yield return item;
                }
                else
                {
                    foreach (var subitem in Flatten((IEnumerable)item))
                    {
                        yield return subitem;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        public static void ForEach(this IEnumerable<object> array, Action<object> callback)
        {
            foreach (var item in array)
            {
                callback(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        public static void EachWithIndex(this IEnumerable<object> array, Action<object, int> callback)
        {
            var index = 0;
            foreach (var item in array)
            {
                callback(item, index);
                ++index;
            }
        }
    }
}
