using System;
using System.Linq;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace DomainClass
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
            var files = new FileGroup();
            if (_model == null)
            {
                foreach (var model in _module.Models)
                {
                    AddModelFiles(files, model);
                }
                foreach (var request in _module.Requests)
                {
                    files.AddFile((IFile)new DomainCommand(Settings, _module, request));
                }
            }
            else
            {
                AddModelFiles(files, _model);
                if (_model.Behaviours.Any())
                {
                    foreach (var request in _module.Requests)
                    {
                        if (_model.Behaviours.Any(b => b.Name == request.Name))
                        {
                            files.AddFile((IFile)new DomainCommand(Settings, _module, request).Create());
                        }
                    }
                }
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