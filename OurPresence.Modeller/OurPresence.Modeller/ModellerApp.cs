namespace OurPresence.Modeller
{
    [Subcommand(typeof(Create))]
    [Subcommand(typeof(Cli.Settings))]
    [Subcommand(typeof(Build))]
    [Subcommand(typeof(Generators))]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    public class ModellerApp
    {
        private readonly ILogger<ModellerApp> _logger;
        private readonly IConfiguration _configuration;

        public ModellerApp(ILogger<ModellerApp> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        internal static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0";

        [Option(Description = "Settings file to use when generating code. Settings in the file will override arguments on the command line")]
        [FileExists]
        public string? Settings { get; }

        [Option(Description = "Target framework. Defaults to net5.0")]
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
