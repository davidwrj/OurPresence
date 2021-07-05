// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Interfaces
{
    public interface IGeneratorConfiguration
    {
        string GeneratorName { get; set; }
        string LocalFolder { get; set; }
        string ModelName { get; set; }
        string OutputPath { get; set; }
        string ServerFolder { get; set; }
        string SettingsFile { get; set; }
        string SourceModel { get; set; }
        string Target { get; set; }
        bool Verbose { get; set; }
        IGeneratorVersion Version { get; set; }
        bool Overwrite { get; set; }
    }
}
