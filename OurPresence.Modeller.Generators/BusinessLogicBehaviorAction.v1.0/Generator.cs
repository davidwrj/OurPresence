using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace BusinessLogicBehaviourAction
{
    public class Generator : IGenerator
    {
        private readonly Module _module;
        private readonly Model? _model;
        private readonly Behaviour _behaviour;

        public Generator(ISettings settings, Module module, Model? model, Behaviour behaviour)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model;
            _behaviour = behaviour;
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var files = new FileGroup(_behaviour.Name.ToString());



            return files;
        }
    }
}
