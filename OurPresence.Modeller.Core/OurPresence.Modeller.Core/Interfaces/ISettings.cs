// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator;

namespace OurPresence.Modeller.Interfaces
{
    public interface ISettings
    {
        bool SupportRegen { get; set; }

        string LocalFolder { get; set; }

        string OutputPath { get; set; }

        string GeneratorName { get; set; }

        string? ModelName { get; set; }

        string ServerFolder { get; set; }

        string SourceModel { get; set; }

        string Target { get; set; }

        string Version { get; set; }

        Packages Packages { get; }
    }
}
