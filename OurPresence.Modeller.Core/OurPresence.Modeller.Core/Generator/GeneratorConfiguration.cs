using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;

namespace OurPresence.Modeller.Generator
{
    public class GeneratorConfiguration : IGeneratorConfiguration
    {        
        public string SourceModel { get; set; }

        public string OutputPath { get; set; } = Defaults.OutputFolder;

        public string Target { get; set; } = Defaults.Target;

        public string GeneratorName { get; set; } = string.Empty;

        public IGeneratorVersion Version { get; set; } = GeneratorVersion.Empty;

        public string LocalFolder { get; set; } = Defaults.LocalFolder;

        public string ServerFolder { get; set; }

        public bool Verbose { get; set; }

        public string ModelName { get; set; }

        public string SettingsFile { get; set; }

        public bool Overwrite { get; set; }

        public IList<IPackage> Packages { get; set; } = new List<IPackage>();
    }
}
