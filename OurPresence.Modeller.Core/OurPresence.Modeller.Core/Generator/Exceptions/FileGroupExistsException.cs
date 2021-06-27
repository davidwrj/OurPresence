using System;

namespace OurPresence.Modeller.Generator.Exceptions
{
    public class FileGroupExistsException : ApplicationException
    {
        public string Name { get; set; }

        public FileGroupExistsException()
        {
            Name = "unknown";
        }

        public FileGroupExistsException(string name)
        {
            Name = name;
        }

        public FileGroupExistsException(string name, string message) : base(message)
        {
            Name = name;
        }

        public FileGroupExistsException(string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
        }

        public FileGroupExistsException(string message, Exception innerException) : base(message, innerException)
        {
            Name = "unknown";
        }
    }
}
