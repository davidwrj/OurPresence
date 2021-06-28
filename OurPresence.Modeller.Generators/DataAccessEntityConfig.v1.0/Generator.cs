using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace EntityFrameworkClass
{
    public class Generator : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public Generator(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model;
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var files = new FileGroup("Data");
            var configs = new FileGroup("Configurations");
            var models = new FileGroup("Models");

            files.AddFileGroup(configs);
            files.AddFileGroup(models);

            if (_model == null)
            {
                _module.Models.ForEach(a => configs.AddFile((IFile)new ConfigGenerated(Settings, _module, a).Create()));
                _module.Models.ForEach(a => models.AddFile((IFile)new ModelGenerated(Settings, _module, a).Create()));
            }
            else
            {
                configs.AddFile((IFile)new ConfigGenerated(Settings, _module, _model).Create());
                models.AddFile((IFile)new ModelGenerated(Settings, _module, _model).Create());
            }
            return files;
        }
    }
}
