using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public sealed class Settings : ISettings
    {
        private readonly List<IPackage> _packages = new();

        public Settings() { }

        public Settings(IGeneratorConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var s = (ISettings)this;

            s.SourceModel = configuration.SourceModel;
            s.Version = configuration.Version.ToString();
            s.LocalFolder = configuration.LocalFolder;
            s.OutputPath = configuration.OutputPath;
            s.ServerFolder = configuration.ServerFolder ?? Defaults.ServerFolder;
            s.Target = configuration.Target;
            s.GeneratorName = configuration.GeneratorName;
            s.ModelName = configuration.ModelName;
        }

        public override string ToString()
        {
            var s = (ISettings)this;
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Server Folder:      {s.ServerFolder}");
            sb.AppendLine($"Local Folder:       {s.LocalFolder}");
            sb.AppendLine($"Output Folder:      {s.OutputPath}");
            sb.AppendLine($"Version:            {s.Version}");
            sb.AppendLine($"Target:             {s.Target}");
            sb.AppendLine($"Generator:          {s.GeneratorName}");
            sb.AppendLine($"Source Model:       {s.SourceModel}");
            sb.AppendLine($"Model Name:         {s.ModelName}");
            return sb.ToString();
        }

        string ISettings.GetPackageVersion(string name, string fallbackVersion)
        {
            var found = _packages.SingleOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
            return found == null ? fallbackVersion : found.Version;
        }

        void ISettings.RegisterPackage(IPackage package)
        {
            Register(package);
        }

        void ISettings.RegisterPackage(string name, string version) => Register(new Package(name, version));

        void ISettings.RegisterPackages(IEnumerable<IPackage> packages)
        {
            foreach (var item in packages)
                Register(item);
        }

        private void Register(IPackage package)
        {
            if (package == null || string.IsNullOrWhiteSpace(package.Name) || string.IsNullOrWhiteSpace(package.Version))
                return;

            var packages = _packages.Where(pa => pa.Name == package.Name);
            if (!packages.Any())
            {
                _packages.Add(package);
                return;
            }

            var p = packages.First();
            if (Version.TryParse(p.Version, out var p1))
            {
                if (Version.TryParse(package.Version, out var p2))
                {
                    if (p1 < p2)
                        p.Version = p2.ToString();
                }
            }
            return;
        }

        bool ISettings.PackagesInitialised() => _packages != null && _packages.Any();

        bool ISettings.SupportRegen { get; set; } = true;

        string ISettings.LocalFolder { get; set; } = Defaults.LocalFolder;

        string ISettings.OutputPath { get; set; } = Defaults.OutputFolder;

        string ISettings.ServerFolder { get; set; } = Defaults.ServerFolder;

        string ISettings.GeneratorName { get; set; } = null!;

        string? ISettings.ModelName { get; set; }

        string ISettings.SourceModel { get; set; } = null!;

        string ISettings.Target { get; set; } = Defaults.Target;

        string ISettings.Version { get; set; } = "0.0";

        IReadOnlyCollection<IPackage> ISettings.Packages => _packages.AsReadOnly();
    }
}
