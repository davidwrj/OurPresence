// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterNotFoundException : LiquidException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FilterNotFoundException(string message, FilterNotFoundException innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public FilterNotFoundException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FilterNotFoundException(string message)
            : base(message)
        {
        }
    }
}
