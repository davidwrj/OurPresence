using OurPresence.Modeller.Generator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkProject
{
    public class GeneratorDetails : MetadataBase
    {
        public GeneratorDetails() : base("1.0.0")
        { }

        public override string Name => "Entity Framework Project";

        public override string Description => "Build an Entity Framework project";

        public override Type EntryPoint => typeof(Generator);

        public override IEnumerable<Type> SubGenerators => new Collection<Type>()
        {
            typeof(EntityFrameworkClass.Generator)
        };
    }
}
