using System;
using System.Runtime.Serialization;

namespace OurPresence.Modeller
{
    public class InvalidSettingsFileException:Exception
    {
        const string msg = "{0} is not a valid settings file.";
        
        public InvalidSettingsFileException(string filename)
            : base(string.Format(msg, filename))
        { }

        public InvalidSettingsFileException(string filename, string? message)
            : base(string.Format(msg, filename) + " " + message)
        { }

        public InvalidSettingsFileException(string filename, string? message, Exception? innerException) 
            : base(string.Format(msg, filename) + " " + message, innerException)
        {
        }

        protected InvalidSettingsFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }

    public class InvalidModuleFileException : Exception
    {
        const string msg = "{0} is not a valid module file.";

        public InvalidModuleFileException(string filename)
            : base(string.Format(msg, filename))
        { }

        public InvalidModuleFileException(string filename, string? message)
            : base(string.Format(msg, filename) + " " + message)
        { }

        public InvalidModuleFileException(string filename, string? message, Exception? innerException)
            : base(string.Format(msg, filename) + " " + message, innerException)
        {
        }

        protected InvalidModuleFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
