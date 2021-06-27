using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;

namespace ApplicationProject
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
            project.FileGroups.First().AddFile((IFile)new ProgramFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new StartupFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new ExceptionHandlerFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new EnvelopeFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new EnvelopeResultFile(Settings, _module).Create());

            project.AddFileGroup((IFileGroup)new ControllerClass.Generator(Settings, _module, null).Create());

            return project;
        }
    }
}
