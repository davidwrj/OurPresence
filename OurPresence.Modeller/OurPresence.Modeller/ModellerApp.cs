using OurPresence.Modeller.Cli;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;

// ReSharper disable MemberCanBePrivate.Global

namespace OurPresence.Modeller
{
    [Subcommand(typeof(Create))]
    [Subcommand(typeof(Settings))]
    [Subcommand(typeof(Build))]
    [Subcommand(typeof(Generators))]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModellerApp
    {
        private readonly ILogger<ModellerApp> _logger;

        public ModellerApp(ILogger<ModellerApp> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Option(Description = "Settings file to use when generating code. Settings in the file will override arguments on the command line")]
        [FileExists]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string? Settings { get; }

        [Option(Description = "Target framework. Defaults to net5.0")]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string? Target { get; }

        [Option(ShortName = "")]
        public bool Overwrite { get; } = false;

        [Option(ShortName = "")]
        public bool Verbose { get; } = false;

        public bool SaveOptions { get; }

        internal int OnExecute(IConsole console, CommandLineApplication app)
        {
            _logger.LogInformation(LoggingEvents.ParameterEvent, "Settings: {Settings}, Target: {Target}, Overwrite: {Overwrite}, Verbose: {Verbose}", Settings, Target, Overwrite.ToString(), Verbose.ToString());

            console.WriteLine();
            console.WriteLine("You need to specify a command.");
            console.WriteLine();

            app.ShowHelp();

            _logger.LogInformation(LoggingEvents.CompleteEvent, "Modeller Complete");
            return 1;
        }
    }
}
