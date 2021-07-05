// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Interfaces
{
    public interface IGeneratorVersion
    {
        bool IsAlphaRelease { get; set; }
        bool IsBetaRelease { get; set; }
        bool IsRelease { get; set; }
        Version Version { get; set; }
    }
}
