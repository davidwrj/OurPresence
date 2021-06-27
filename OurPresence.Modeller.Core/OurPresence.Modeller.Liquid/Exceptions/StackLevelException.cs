// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class StackLevelException : LiquidException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public StackLevelException(string message)
            : base(string.Format(message))
        {
        }
    }
}
