﻿// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Liquid.Exceptions
{
    public class SyntaxException : LiquidException
    {
        public SyntaxException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
