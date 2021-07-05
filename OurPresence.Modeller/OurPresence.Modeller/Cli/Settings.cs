// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Cli
{
    [Command(Description = "Model generator settings")]
    internal class Settings
    {
        private readonly ILogger<Settings> _logger;
        private readonly ILoader<ISettings> _settingsLoader;
        private readonly IGeneratorConfiguration _configuration;
        private readonly IPackageService _packageService;

        public Settings(ILogger<Settings> logger, ILoader<ISettings> settingsLoader, IGeneratorConfiguration configuration, IPackageService packageService)
        {
            _logger = logger;
            _settingsLoader = settingsLoader;
            _configuration = configuration;
            _packageService = packageService;
        }

        internal int OnExecute()
        {
            try
            {
                _logger.LogTrace("Generator Settings Command - OnExecute");

                if(!_settingsLoader.TryLoad(_configuration.SettingsFile, out var instance))
                {
                    instance = new Generator.Settings(_configuration, _packageService);
                }

                _logger.LogInformation("{Message}",instance.ToString());

                return 0;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(LoggingEvents.ListError, ex, "Settings command failed");
                return 1;
            }
        }
    }
}