// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
