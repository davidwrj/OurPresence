// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace OverwriteHeader
{
    public class Generator : IGenerator
    {
        private readonly IMetadata _metadata;

        public Generator(ISettings settings, IMetadata metadata)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new System.Text.StringBuilder();
            sb.Al($"// Auto-generated using OurPresence.Modeller template '{_metadata.Name}' version {_metadata.Version}");
            sb.B();
            sb.Al($"// {new string('-', 80)}");
            sb.Al("// WARNING: This file will be overwritten if re-generation is triggered.");
            sb.Al($"// {new string('-', 80)}");
            return new Snippet(sb.ToString());
        }
    }
}
