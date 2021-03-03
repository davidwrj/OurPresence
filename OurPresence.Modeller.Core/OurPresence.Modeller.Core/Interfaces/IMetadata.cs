using System;
using System.Collections.Generic;

namespace OurPresence.Modeller.Interfaces
{
    public interface IMetadata
    {
        IGeneratorVersion Version { get; }

        string Name { get; }

        string Description { get; }

        Type EntryPoint { get; }

        IEnumerable<Type> SubGenerators { get; }
    }

}
