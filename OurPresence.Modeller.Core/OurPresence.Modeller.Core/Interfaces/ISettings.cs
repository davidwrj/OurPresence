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
