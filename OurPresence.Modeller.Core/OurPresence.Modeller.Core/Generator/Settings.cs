// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public sealed class Settings : ISettings
    {
        public Settings(IGeneratorConfiguration configuration, IPackageService packageService)
        {
            SourceModel = configuration.SourceModel;
            Version = configuration.Version.Version.ToString();
            LocalFolder = configuration.LocalFolder;
            OutputPath = configuration.OutputPath;
            ServerFolder = configuration.ServerFolder;
            Target = configuration.Target;
            GeneratorName = configuration.GeneratorName;
            ModelName = configuration.ModelName;
            SupportRegen = true;
            
            Packages = new Packages(packageService, this);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Generator:          {GeneratorName}");
            sb.AppendLine($"Local Folder:       {LocalFolder}");
            sb.AppendLine($"Model Name:         {ModelName}");
            sb.AppendLine($"Output Folder:      {OutputPath}");
            sb.AppendLine($"Server Folder:      {ServerFolder}");
            sb.AppendLine($"Source Model:       {SourceModel}");
            sb.AppendLine($"Target:             {Target}");
            sb.AppendLine($"Version:            {Version}");
            return sb.ToString();
        }

        public bool SupportRegen { get; set; } 

        public string LocalFolder { get; set; } 

        public string OutputPath { get; set; } 

        public string ServerFolder { get; set; } 

        public string GeneratorName { get; set; } 

        public string? ModelName { get; set; }

        public string SourceModel { get; set; } 

        public string Target { get; set; } 

        public string Version { get; set; } 

        public Packages Packages { get; } 
    }
}