using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{

    internal class ProjectFile : IGenerator
    {
        private readonly Module _module;

        public ProjectFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var projectName = _module.Namespace;
            var project = (IProject)new Project(projectName) { Path = System.IO.Path.Combine("src", projectName) };

            var files = new FileGroup();
            project.AddFileGroup(files);

            var sb = new StringBuilder();
            sb.al("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.b();
            sb.i(1).al("<PropertyGroup>");
            sb.i(2).al("<TargetFramework>net5.0</TargetFramework>");
            sb.i(2).al("<Configurations>Debug;Release;DebugOnPremise;DebugAzure</Configurations>");
            sb.i(2).al($"<RootNamespace>{project.Name}</RootNamespace>");
            sb.i(2).al($"<Nullable>enable</Nullable>");
            sb.i(1).al("</PropertyGroup>");
            sb.b();
            sb.i(1).al("<ItemGroup>");
            sb.i(2).al($"<PackageReference Include=\"OurPresence.Core\" Version=\"{Settings.GetPackageVersion("OurPresence.Core", "1.0.0")}\" />");
            sb.i(2).al($"<PackageReference Include=\"MediatR\" Version=\"{Settings.GetPackageVersion("MediatR", "9.0.0")}\" />");
            sb.i(1).al("</ItemGroup>");
            sb.b();
            sb.al("</Project>");

            var projectFile = new File(project.Name + ".csproj", sb.ToString());
            files.AddFile(projectFile);

            return project;
        }
    }
}
