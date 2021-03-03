using System.Collections.Generic;

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

        string GetPackageVersion(string name, string fallbackVersion);

        void RegisterPackage(IPackage package);

        void RegisterPackage(string name, string version);

        void RegisterPackages(IEnumerable<IPackage> packages);

        bool PackagesInitialised();

        IReadOnlyCollection<IPackage> Packages { get; }
    }
}
