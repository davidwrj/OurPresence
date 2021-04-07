using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable MemberCanBePrivate.Global

namespace OurPresence.Modeller.Cli
{
    [Command(Name = "build", Description = "Use DLL components to generate code")]
    internal class Build
    {
        private readonly IBuilder _builder;
        private readonly ILogger<Build> _logger;

        public Build(IBuilder builder, IConfiguration configuration, ILogger<Build> logger)
        {
            _builder = builder;
            _logger = logger;
            
            Target = configuration.GetValue<string>("Target");
            Output = configuration.GetValue<string>("Output");
            LocalFolder = configuration.GetValue<string>("LocalFolder");
            Settings = Environment.CurrentDirectory;
        }

        [Argument(0, Description = "The generator to use.")]
        public string Generator { get; } = string.Empty;

        [Argument(1, Description = "The filename for the source model to use during code generation.")]
        [FileExists]
        public string SourceModel { get; } = string.Empty;

        [Option(Description = "Path to the locally cached generators")]
        [DirectoryExists]
        public string LocalFolder { get; } 

        [Option(Description = "Model name. If included then the output will be limited to the specified model")]
        public string? Model { get; }

        [Option(Description = "Output folder")]
        public string Output { get; } 

        [Option(Inherited = true, ShortName = "")]
        public bool Overwrite { get; }

        [Option(Description = "Target framework. Defaults to netstandard2.0", Inherited = true)]
        public string Target { get; } 

        [Option(Description = "Settings file to use when generating code. Settings in the file will override arguments on the command line", ShortName = "s", Inherited = true)]
        [FileExists]
        public string? Settings { get; }

        [Option(Description = "Specific version to use for the generator", ShortName = "")]
        public string Version { get; } = "1.0.0";

        [Option(ShortName = "", Inherited = true)]
        public bool Verbose { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Top level unexpected catch all")]
        internal int OnExecute()
        {
            try
            {
                IGeneratorConfiguration config = new GeneratorConfiguration()
                {
                    Verbose = Verbose,
                    Overwrite = Overwrite
                };

                if (!string.IsNullOrWhiteSpace(Generator))
                    config.GeneratorName = Generator;
                if (!string.IsNullOrWhiteSpace(LocalFolder))
                    config.LocalFolder = LocalFolder;
                if (!string.IsNullOrWhiteSpace(Output))
                    config.OutputPath = Output;
                if (!string.IsNullOrWhiteSpace(Settings))
                    config.SettingsFile = Settings;
                if (!string.IsNullOrWhiteSpace(Target))
                    config.Target = Target;
                if (!string.IsNullOrWhiteSpace(Version))
                    config.Version = new GeneratorVersion(Version);

                if (!string.IsNullOrWhiteSpace(Model))
                    config.ModelName = Model;
                if (!string.IsNullOrWhiteSpace(SourceModel))
                    config.SourceModel = SourceModel;

                _logger.LogTrace("Generator Build Command - OnExecute");

                _builder.Create();

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.BuildError, ex, "Build command failed");
                return 1;
            }
            finally
            {
                _logger.LogTrace("Generator Build Command - complete");
            }
        }
    }
}
