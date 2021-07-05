// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Tests.TestFiles
{
    public class SimpleTestGenerator : IGenerator
    {
        private readonly Module _module;

        public SimpleTestGenerator(ISettings settings, Module module)
        {
            Settings = settings;
            _module = module;
        }

        public ISettings Settings { get; }

        IOutput IGenerator.Create() => new Snippet($"Snippet Content for {_module.Project}");
    }
}
