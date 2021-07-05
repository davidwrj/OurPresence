// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
