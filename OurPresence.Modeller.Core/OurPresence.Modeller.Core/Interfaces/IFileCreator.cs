using System;

namespace OurPresence.Modeller.Interfaces
{
    public interface IFileCreator
    {
        Type SupportedType { get; }

        void Create(IOutput output, string path, bool overwrite);
    }
}
