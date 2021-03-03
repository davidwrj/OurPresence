using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class RelationshipExtensions
    {
        public static Field? GetForeignBusinessKey(this Relationship relate, Module module)
        {
            if (module is null)
                throw new System.ArgumentNullException(nameof(module));

            return module.Models.SingleOrDefault(m => m.Name == relate.DependantModel)?.Fields.FirstOrDefault(f => f.BusinessKey);
        }

        public static Relationship Align(this Relationship relationship, Model model)
        {
            if (relationship is null)
                throw new System.ArgumentNullException(nameof(relationship));
            if (model is null)
                throw new System.ArgumentNullException(nameof(model));

            return relationship.PrincipalModel == model.Name
                ? relationship
                : new Relationship()
            {
                PrincipalModel = relationship.DependantModel,
                PrincipalFields = relationship.DependantFields,
                PrincipalType = relationship.DependantType,
                DependantModel = relationship.PrincipalModel,
                DependantFields = relationship.PrincipalFields,
                DependantType = relationship.PrincipalType,
                LinkTable = relationship.LinkTable
            };
        }

        public static IEnumerable<string> GetPropertyName(this Relationship relate)
        {
            if (relate is null)
                throw new System.ArgumentNullException(nameof(relate));
            var name = relate.PrincipalFields.Select(f => f.Plural.Value);
            if (relate.PrincipalType == RelationshipTypes.One && relate.DependantType == RelationshipTypes.Many)
            {
                name = relate.DependantFields.Select(f =>
                {
                    return f.Value != "Id" && f.Value.EndsWith("Id", System.StringComparison.InvariantCultureIgnoreCase)
                       ? f.TrimEnd("Id") + relate.DependantModel.Plural.Value
                       : f.Value + relate.DependantModel.Plural.Value;
                });
            }
            else if (relate.PrincipalType == RelationshipTypes.Many && relate.DependantType == RelationshipTypes.One)
            {
                var field = relate.PrincipalFields.First();
                name = field.Value.EndsWith("Id", System.StringComparison.InvariantCultureIgnoreCase)
                    ? (new[] { field.Singular.Value.Substring(0, field.Value.Length - 2) })
                    : (new[] { field.Plural.Value });
            }
            else if (relate.PrincipalType == RelationshipTypes.Many && relate.DependantType == RelationshipTypes.Many)
            {
                name = relate.DependantFields.Select(f =>
                {
                    var temp = f.Value + "Id";
                    if (string.IsNullOrWhiteSpace(temp))
                        temp = relate.DependantModel.Value;
                    return temp.Pluralize();
                });
            }
            else if (relate.PrincipalType == RelationshipTypes.One && relate.DependantType == RelationshipTypes.One)
            {
                name = relate.DependantFields.Select(f => f.Value == "Id" ? relate.DependantModel.Value : f.Value + "Id" + relate.DependantModel.Value);
            }
            return name.Select(f => f.CheckKeyword());
        }
    }

}
