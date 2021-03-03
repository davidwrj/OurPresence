using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OurPresence.Modeller.Generator;

namespace OverwriteHeader
{
    public class GeneratorDetails : MetadataBase
    {
        public GeneratorDetails() : base("1.0.0")
        { }

        public override string Name => "Overwrite Header";

        public override string Description => "Auto-generate header applied to files that can be overwritten";

        public override Type EntryPoint => typeof(Generator);

        public override IEnumerable<Type> SubGenerators => new Collection<Type>();
    }
}
