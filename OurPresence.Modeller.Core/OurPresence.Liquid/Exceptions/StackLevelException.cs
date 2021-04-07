// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Liquid.Exceptions
{
    public class StackLevelException : LiquidException
    {
        public StackLevelException(string message)
            : base(string.Format(message))
        {
        }
    }
}
