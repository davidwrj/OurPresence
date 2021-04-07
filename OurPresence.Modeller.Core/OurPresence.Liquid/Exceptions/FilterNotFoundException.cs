// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Liquid.Exceptions
{
    public class FilterNotFoundException : LiquidException
    {
        public FilterNotFoundException(string message, FilterNotFoundException innerException)
            : base(message, innerException)
        {
        }

        public FilterNotFoundException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }

        public FilterNotFoundException(string message)
            : base(message)
        {
        }
    }
}
