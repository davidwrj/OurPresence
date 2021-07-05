// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Generator.Exceptions
{
    public class MissingTargetException : ApplicationException
    {
        public string Target { get; }

        public MissingTargetException(string target)
        {
            Target = target;
        }

        public MissingTargetException(string target, string message) : base(message)
        {
            Target = target;
        }

        public MissingTargetException(string target, string message, Exception innerException) : base(message, innerException)
        {
            Target = target;
        }
    }
}
