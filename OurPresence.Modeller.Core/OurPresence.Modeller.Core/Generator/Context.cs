using FluentValidation.Results;
using OurPresence.Modeller.Generator.Validators;
using System;
using System.Linq;
using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace OurPresence.Modeller.Generator
{
    public class Context : IContext
    {
        private readonly ILoader<ISettings> _settingsLoader;
        private readonly ILoader<IEnumerable<INamedElement>> _moduleLoader;
        private readonly ILoader<IEnumerable<IGeneratorItem>> _generatorLoader;
        private readonly IPackageService _packageService;
        private readonly ILogger<IContext> _logger;
        private ISettings _settings = null!;
        private IGeneratorItem? _generator;
        private INamedElement? _model;
        private INamedElement? _module;
        private ValidationResult? _result;

        public Context(ILoader<ISettings> settingsLoader, ILoader<IEnumerable<INamedElement>> moduleLoader, ILoader<IEnumerable<IGeneratorItem>> generatorLoader, IPackageService packageService, ILogger<IContext> logger)
        {
            _settingsLoader = settingsLoader;
            _moduleLoader = moduleLoader;
            _generatorLoader = generatorLoader;
            _packageService = packageService;
            _logger = logger;
        }

        IGeneratorItem? IContext.Generator => _result == null
                    ? throw new InvalidOperationException("Call ValidateConfiguration before returning the generator")
                    : _generator;

        ISettings IContext.Settings => _result == null ? throw new InvalidOperationException("Call ValidateConfiguration before returning the settings") : _settings;

        INamedElement? IContext.Module => _result == null ? throw new InvalidOperationException("Call ValidateConfiguration before returning the module") : _module;

        INamedElement? IContext.Model
        {
            get
            {
                return _result == null ? throw new InvalidOperationException("Call ValidateConfiguration before returning the model") : _model;
            }
        }

        private ISettings GetSettings(IGeneratorConfiguration config)
        {
            if (!_settingsLoader.TryLoad(config.SettingsFile, out var settings))
                settings = new Settings(config);

            if (!settings.PackagesInitialised())
            {
                _packageService.Refresh(System.IO.Path.Combine(settings.LocalFolder, settings.Target, settings.Target + ".json"));
                settings.RegisterPackages(_packageService.Items);
                _logger.LogInformation($"Registered {settings.Packages.Count()} packages");
            }
            return settings;
        }

        private IGeneratorItem? GetGenerator(string localFolder, string name, string version)
        {
            if (!string.IsNullOrWhiteSpace(name) && _generatorLoader.TryLoad(localFolder, out var generators))
            {
                IGeneratorVersion v = new GeneratorVersion(version);
                name = name.ToLowerInvariant();
                var matches = generators.Where(g => g.Metadata.Name.ToLowerInvariant() == name || g.AbbreviatedFileName.ToLowerInvariant() == name);
                var exact = matches.SingleOrDefault(m => m.Metadata.Version == v);
                return exact ?? matches.OrderByDescending(k => k.Metadata.Version).FirstOrDefault();
            }
            return null;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            if (_settings != null)
                sb.AppendLine(_settings.ToString());

            return sb.ToString();
        }

        bool IContext.IsValid() => _result == null ? false : _result.IsValid;

        public ValidationResult ValidateConfiguration(IGeneratorConfiguration config)
        {
            _result = new ValidationResult();
            _settings = GetSettings(config);
            _generator = GetGenerator(_settings.LocalFolder, _settings.GeneratorName, _settings.Version);

            if (!string.IsNullOrEmpty(_settings.SourceModel))
            {
                var items = _moduleLoader.Load(_settings.SourceModel);
                if (!items.Any())
                    _result.Errors.Add(new ValidationFailure("SourceModel", "No module found in the source model."));
                else if (items.Count() == 1)
                    _module = items.First();
                else
                    _result.Errors.Add(new ValidationFailure("SourceModel", "More than one module was found."));
            }

            if (_module != null && string.IsNullOrEmpty(_settings.ModelName))
                _model = ((Domain.Module)_module).Models.FirstOrDefault(m => m.Name.Value == config.ModelName);

            var configValidator = new ContextValidator();
            var result = configValidator.Validate(this);
            if (result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    _logger.LogError(item.ErrorMessage);
                    _result.Errors.Add(item);
                }
            }
            return _result;
        }
    }
}
