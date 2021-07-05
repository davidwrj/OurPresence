// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Interfaces
{
    public interface IFileCreator
    {
        Type SupportedType { get; }

        void Create(IOutput output, string path, bool overwrite);
    }
}
