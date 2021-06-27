﻿// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Liquid.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RenderException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        protected RenderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected RenderException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        protected RenderException()
        {
        }
    }
}
