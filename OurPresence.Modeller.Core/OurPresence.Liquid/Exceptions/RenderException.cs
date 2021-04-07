// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Liquid.Exceptions
{
    public abstract class RenderException : Exception
    {
        protected RenderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RenderException(string message)
            : base(message)
        {
        }

        protected RenderException()
        {
        }
    }
}
