using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Interfaces;
using System;

namespace DataAccessProject
{
    public class Generator : IGenerator
    {
        private readonly Module _module;

        public Generator(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var project = (IProject)new ProjectFile(Settings, _module).Create();
            _module.Models.ForEach(m =>
                project.AddFileGroup((IFileGroup)new DataAccessEntityConfig.Generator(Settings, _module, m).Create()));
            return project;
        }
    }
}
