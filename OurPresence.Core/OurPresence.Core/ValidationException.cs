// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Core
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : this(message, null) { }
        public ValidationException(string message, params ValidationError[] errors) : base(message, null)
        {
            this.Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public IEnumerable<ValidationError> Errors { get; private set; }

    }
}
