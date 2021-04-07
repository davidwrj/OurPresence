using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class ModuleExtensions
    {
        public static Model AddModel(this Module module, string name)
        {
            return AddModel(module, name, true, null);
        }


        public static Model AddModel(this Module module, string name, bool hasAudit, string? schema)
        {
            var model = new Model(name)
            {
                HasAudit = hasAudit,
                Schema = schema
            };
            module.Models.Add(model);
            return model;
        }

        public static Request AddRequest(this Module module, string name)
        {
            var request = new Request(name);
            module.Requests.Add(request);
            return request;
        }

        public static IEnumerable<Relationship> FindRelationshipsByModel(this Module module, Model model)
        {
            return module.Models.SelectMany(m => m.Relationships).Where(r => r.PrincipalModel == model.Name || r.DependantModel == model.Name).Select(r => r.Align(model));
        }

        public static IEnumerable<Relationship> FindRelationshipsByModelAndField(this Module module, Model model, IEnumerable<string> fields)
        {
            return module.Models.SelectMany(m => m.Relationships).Where(r => (r.PrincipalFields.Select(f => f.Value).SequenceEqual(fields) && r.PrincipalModel == model.Name) || (r.DependantFields.Select(f => f.Value).SequenceEqual(fields) && r.DependantModel == model.Name)).Select(r => r.Align(model));
        }

        public static Module AddForeignKey(this Module module, Model principal, Model dependant)
        {
            if (principal is null)
                throw new ArgumentNullException(nameof(principal));
            if (dependant is null)
                throw new ArgumentNullException(nameof(dependant));

            if (!principal.Key.Fields.Any())
                throw new ArgumentException($"Foreign Key can't be generated since {principal.Name} does not have a primary key.");

            var fk = new List<string>();
            foreach (var pk in principal.Key.Fields)
            {
                var name = principal.Name.Value + pk.Name.Value;
                var found = dependant.Fields.FirstOrDefault(f => f.Name.Value == name);
                if (found == null)
                {
                    found = new Field(name) { DataType = pk.DataType };
                    dependant.Fields.Add(found);
                }
                fk.Add(found.Name.Value);
            }

            principal.AddRelation(principal.Key.Fields.Select(f => f.Name.Value).ToArray(), dependant.Name.Value, fk.ToArray());

            return module;
        }

        public static bool IsForeignKey(this Module module, Model model, IEnumerable<string> fields)
        {
            var relationships = module.FindRelationshipsByModelAndField(model, fields);

            var found = relationships.Where(r => (r.DependantFields.Select(f => f.Value).SequenceEqual(fields)) || (r.PrincipalFields.Select(f => f.Value).SequenceEqual(fields))).ToList();
            if (!found.Any()) return false;

            foreach (var relationship in found)
            {
                IEnumerable<string>? actual = null;
                IEnumerable<string>? expected = null;

                var principal = module.Models.SingleOrDefault(m => m.Name == relationship.PrincipalModel);
                var dependant = module.Models.SingleOrDefault(m => m.Name == relationship.DependantModel);
                if (principal == null || dependant == null) continue;

                if (model.Name == relationship.PrincipalModel)
                {
                    actual = dependant.Key.Fields.Select(f => $"{relationship.DependantModel.Value}{f.Name.Value}");
                    expected = relationship.PrincipalFields.Select(f => f.Value);
                }
                else if (model.Name == relationship.DependantModel)
                {
                    actual = principal.Key.Fields.Select(f => $"{relationship.PrincipalModel.Value}{f.Name.Value}");
                    expected = relationship.DependantFields.Select(f => f.Value);
                }
                else
                    return false;

                if (expected.SequenceEqual(actual))
                    return true;
            }
            return false;
        }
    }

}
