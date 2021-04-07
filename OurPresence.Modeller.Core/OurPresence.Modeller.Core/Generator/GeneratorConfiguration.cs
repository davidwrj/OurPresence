using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class GeneratorConfiguration : IGeneratorConfiguration
    {        
        public string SourceModel { get; set; }

        public string OutputPath { get; set; } 

        public string Target { get; set; } 

        public string GeneratorName { get; set; } 

        public IGeneratorVersion Version { get; set; } = GeneratorVersion.Empty;

        public string LocalFolder { get; set; } 

        public string ServerFolder { get; set; }

        public bool Verbose { get; set; }

        public string ModelName { get; set; }

        public string SettingsFile { get; set; }

        public bool Overwrite { get; set; }
    }
}
