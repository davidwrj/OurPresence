// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            var files = new FileGroup();

            if (_model == null)
            {
                _module.Models.ForEach(a => files.AddFile((IFile)new ConfigGenerated(Settings, _module, a).Create()));
                _module.Models.ForEach(a => files.AddFile((IFile)new ModelGenerated(Settings, _module, a).Create()));
            }
            else
            {
                files.AddFile((IFile)new ConfigGenerated(Settings, _module, _model).Create());
                files.AddFile((IFile)new ModelGenerated(Settings, _module, _model).Create());
            }
            return files;
        }
    }
}
