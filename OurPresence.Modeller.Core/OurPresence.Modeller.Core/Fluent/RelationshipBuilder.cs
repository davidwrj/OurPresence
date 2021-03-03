using OurPresence.Modeller.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RelationshipBuilder : FluentBase
    {
        public RelationshipBuilder(ModuleBuilder module, ModelBuilder model, Domain.Relationship relationship)
        {
            ModuleBuild = module ?? throw new ArgumentNullException(nameof(module));
            Build = model ?? throw new ArgumentNullException(nameof(model));
            Instance = relationship;
        }

        public ModuleBuilder ModuleBuild { get; }

        public ModelBuilder Build { get; }

        public Domain.Relationship Instance { get; }

        public RelationshipBuilder Relate(string principal, IEnumerable<string> principalFields, string dependant, IEnumerable<string> dependantFields, RelationshipTypes principalType = RelationshipTypes.One, RelationshipTypes dependantType = RelationshipTypes.Many)
        {
            if (principalType == RelationshipTypes.Many && dependantType == RelationshipTypes.Many)
                return Relate(principal, principalFields, dependant, dependantFields, principal + dependant);

            Instance.SetRelationship(new Name(principal), principalFields.Select(s => new Name(s)), new Name(dependant), dependantFields.Select(s => new Name(s)), principalType, dependantType);
            return this;
        }

        public RelationshipBuilder Relate(string principal, IEnumerable<string> principalFields, string dependant, IEnumerable<string> dependantFields, string linkTable)
        {
            Instance.SetRelationship(new Name(principal), principalFields.Select(s => new Name(s)), new Name(dependant), dependantFields.Select(s => new Name(s)), RelationshipTypes.Many, RelationshipTypes.Many, linkTable);
            return this;
        }
    }
}
