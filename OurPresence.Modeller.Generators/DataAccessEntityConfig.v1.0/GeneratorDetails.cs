using OurPresence.Modeller.Generator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkClass
{
    public class GeneratorDetails : MetadataBase
    {
        public GeneratorDetails() : base("1.0.0")
        { }

        public override string Name => "Entity Framework Entity classes";

        public override string Description => "Build the Entity Framework Entity classes";

        public override Type EntryPoint => typeof(Generator);

        public override IEnumerable<Type> SubGenerators => new Collection<Type>();
    }
}
