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
