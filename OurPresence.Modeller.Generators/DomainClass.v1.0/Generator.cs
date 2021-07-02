using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace DomainClass
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

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var files = new FileGroup("Domain");
            if (_model == null)
            {
                _module.Models.ForEach(m => AddModelFiles(files, m));
            }
            else
            {
                AddModelFiles(files, _model);
            }
            return files;
        }

        private void AddModelFiles(FileGroup files, Model model)
        {
            files.AddFile((IFile)new DomainUser(Settings, _module, model).Create());
            files.AddFile((IFile)new DomainGenerated(Settings, _module, model).Create());
            var fg = (IFileGroup)new DomainEvent(Settings, _module, model).Create();
            foreach (var file in fg.Files)
            {
                files.AddFile(file);
            }
        }
    }
}
