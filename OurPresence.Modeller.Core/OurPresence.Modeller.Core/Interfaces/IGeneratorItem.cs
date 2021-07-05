// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Interfaces
{
    public interface IGeneratorItem
    {
        string AbbreviatedFileName { get; }
        string FilePath { get; }
        IMetadata Metadata { get; }
        Type Type { get; }

        IGenerator Instance(params object[] args);
    }
}
