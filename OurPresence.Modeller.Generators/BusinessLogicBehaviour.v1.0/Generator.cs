// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            var files = new FileGroup();
            if(_model is null)
            {
                _module.Models.ForEach(m => m.Behaviours.ForEach(b => files.AddFile((IFile)new BehaviourRequestGenerator(Settings, _module, m, b).Create())));
                _module.Models.ForEach(m => m.Behaviours.ForEach(b => files.AddFile((IFile)new BehaviourRequestUser(Settings, _module, m, b).Create())));
                _module.Models.ForEach(m => m.Behaviours.ForEach(b => files.AddFile((IFile)new ValidatorFile(Settings, _module, m, b).Create())));
            }
            else
            {
                _model.Behaviours.ForEach(b => files.AddFile((IFile)new BehaviourRequestGenerator(Settings, _module, _model, b).Create()));
                _model.Behaviours.ForEach(b => files.AddFile((IFile)new BehaviourRequestUser(Settings, _module, _model, b).Create()));
                _model.Behaviours.ForEach(b => files.AddFile((IFile)new ValidatorFile(Settings, _module, _model, b).Create()));
            }
            return files;
        }
    }
}
