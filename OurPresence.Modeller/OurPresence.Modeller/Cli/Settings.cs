using OurPresence.Modeller.Interfaces;
using OurPresence.Modeller.Properties;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace OurPresence.Modeller.Cli
{
    [Command(Description = "Model generator settings")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via reflection")]
    internal class Settings
    {
        private readonly ILogger<Program> _logger;
        private readonly ILoader<ISettings> _settingsLoader;
        private readonly IGeneratorConfiguration _configuration;

        public Settings(ILogger<Program> logger, ILoader<ISettings> settingsLoader, IGeneratorConfiguration configuration)
        {
            _logger = logger;
            _settingsLoader = settingsLoader;
            _configuration = configuration;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Top level unexpected catch all")]
        internal int OnExecute()
        {
            try
            {
                _logger.LogTrace(Resources.SettingsOnExecute);

                if(!_settingsLoader.TryLoad(_configuration.SettingsFile, out var instance))
                {
                    instance = new Generator.Settings(_configuration);
                }

                _logger.LogInformation(instance.ToString());

                return 0;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(LoggingEvents.ListError, ex, Resources.SettingsFailed);
                return 1;
            }
        }
    }
}
