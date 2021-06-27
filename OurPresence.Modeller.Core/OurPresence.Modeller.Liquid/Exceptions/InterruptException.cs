// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class InterruptException : LiquidException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InterruptException(string message)
            : base(message)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BreakInterrupt : InterruptException
    {
        /// <summary>
        /// 
        /// </summary>
        public BreakInterrupt()
            : base("Misplaced 'break' statement")
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ContinueInterrupt : InterruptException
    {
        /// <summary>
        /// 
        /// </summary>
        public ContinueInterrupt()
            : base("Misplaced 'continue' statement")
        {
        }
    }
}
