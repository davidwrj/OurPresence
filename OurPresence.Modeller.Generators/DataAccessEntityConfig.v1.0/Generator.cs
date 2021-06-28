using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System.Text;
using OurPresence.Modeller.Domain.Extensions;
using System.Linq;

namespace DataAccessEntityConfig
{
    public class Generator : IGenerator
    {
        private readonly Module _module;
        private readonly Model? _model;

        public Generator(ISettings settings, Module module, Model? model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model;
        }

        public IOutput Create()
        {
            if (_model == null)
            {
                var fg = new FileGroup("Configuration");
                foreach (var model in _module.Models.Where(m => m.IsEntity()))
                {
                    fg.AddFile((IFile)new ConfigGenerated(Settings, _module, model).Create());
                }
                return fg;
            }
            return new ConfigGenerated(Settings, _module, _model).Create();
        }

        public ISettings Settings { get; }
    }
}
