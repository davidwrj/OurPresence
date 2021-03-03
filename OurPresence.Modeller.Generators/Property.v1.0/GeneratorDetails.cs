using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OurPresence.Modeller.Generator;

namespace Property
{
    public class GeneratorDetails : MetadataBase
    {
        public GeneratorDetails() : base("1.0.0")
        { }

        public override string Name => "C# Property";

        public override string Description => "Build a C# property";

        public override Type EntryPoint => null;

        public override IEnumerable<Type> SubGenerators => new Collection<Type>();
    }
}