// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace OurPresence.Modeller
{
    public class InvalidSettingsFileException:Exception
    {
        const string Msg = "{0} is not a valid settings file.";

        public InvalidSettingsFileException(string filename)
            : base(string.Format(Msg, filename))
        { }

        public InvalidSettingsFileException(string filename, string? message)
            : base(string.Format(Msg, filename) + " " + message)
        { }

        public InvalidSettingsFileException(string filename, string? message, Exception? innerException)
            : base(string.Format(Msg, filename) + " " + message, innerException)
        {
        }

        protected InvalidSettingsFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }

    public class InvalidModuleFileException : Exception
    {
        const string Msg = "{0} is not a valid module file.";

        public InvalidModuleFileException(string filename)
            : base(string.Format(Msg, filename))
        { }

        public InvalidModuleFileException(string filename, string? message)
            : base(string.Format(Msg, filename) + " " + message)
        { }

        public InvalidModuleFileException(string filename, string? message, Exception? innerException)
            : base(string.Format(Msg, filename) + " " + message, innerException)
        {
        }

        protected InvalidModuleFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
