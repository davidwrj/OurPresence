using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace BusinessLogicBehaviour
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
            if(_model == null)
            {
                var files = new FileGroup();
                _module.Models.ForEach(m => files.AddFileGroup(AddModelFiles(m)));
                return files;
            }
            else
            {
                return AddModelFiles(_model);
            }
        }

        private IFileGroup AddModelFiles(Model model)
        {
            var files = new FileGroup(model.Name.ToString());

            model.Behaviours.ForEach(b =>  new BusinessLogicBehaviourAction.Generator(Settings, _module, _model, b));

            return files;
        }
    }
}
