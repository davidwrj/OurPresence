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
            sb.al($"// Auto-generated using OurPresence.Modeller template '{_metadata.Name}' version {_metadata.Version}");
            sb.b();
            sb.al($"// {new string('-', 80)}");
            sb.al("// WARNING: This file will be overwritten if re-generation is triggered.");
            sb.al($"// {new string('-', 80)}");
            return new Snippet(sb.ToString());
        }
    }
}
