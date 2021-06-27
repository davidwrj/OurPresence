// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeList
    {
        private readonly List<object> _nodes = new();

        /// <summary>
        /// 
        /// </summary>
        public NodeList()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public NodeList(IEnumerable<object> items)
        {
            _nodes.AddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => _nodes.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            if (item is null)
            {
                return;
            }

            _nodes.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> GetItems() => _nodes.ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int Count(Func<object, bool> p)
        {
            return _nodes.Count(p);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _nodes.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> OfType<T>() => _nodes.OfType<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<object> FindAll(Predicate<object> p)
        {
            return _nodes.FindAll(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal bool Any(Func<object, bool> p)
        {
            return _nodes.Any(p);
        }
    }
}
