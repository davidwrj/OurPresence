// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Core
{
    public class ValidationError
    {
        public ValidationError(string context, string message)
        {
            if (string.IsNullOrWhiteSpace(context))
                throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            this.Message = message;
            this.Context = context;
        }

        public string Context { get; }
        public string Message { get; }
    }
}
