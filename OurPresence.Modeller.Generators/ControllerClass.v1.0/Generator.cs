// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace ControllerClass
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
            var files = new FileGroup("Controllers");
            if(_model == null)
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
            files.AddFile((IFile)new ControllerUser(Settings, _module, model).Create());
            files.AddFile((IFile)new ControllerGenerated(Settings, _module, model).Create());
        }
    }
}
