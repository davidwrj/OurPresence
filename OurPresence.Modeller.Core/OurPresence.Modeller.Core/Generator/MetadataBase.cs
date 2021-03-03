using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OurPresence.Modeller.Generator
{
    public abstract class MetadataBase : IMetadata
    {
        protected MetadataBase()
        {
            var vers = GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            Version = new GeneratorVersion(vers);
        }

        protected MetadataBase(string vers)
        {
            Version = new GeneratorVersion(vers);
        }

        public IGeneratorVersion Version { get; private set; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract Type EntryPoint { get; }

        public abstract IEnumerable<Type> SubGenerators { get; }
    }
}
