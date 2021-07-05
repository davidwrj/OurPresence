// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace OurPresence.Modeller.Interfaces
{
    public interface IPackageService
    {
        IEnumerable<IPackage> Items { get; }

        void Refresh(string targetFile);
    }
}
