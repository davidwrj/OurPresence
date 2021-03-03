using FluentValidation;
using FluentValidation.Results;
using OurPresence.Modeller.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Domain.Validators
{
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Key).SetValidator(new KeyValidator());

            RuleFor(x => x.Fields)
                .Custom((list, context) =>
                {
                    if (list.Count == 0)
                        context.AddFailure($"Model {context.DisplayName} must have at least one field");
                    if (list.Count(f => f.BusinessKey == true) > 1)
                        context.AddFailure($"Model {context.DisplayName} can only have at most, one business key");
                    if (HasAuditFields(list))
                        context.AddFailure(new ValidationFailure("HasAudit", $"Model {context.DisplayName} audit fields shouldn't be added, change HasAudit to true"));
                    var duplicates = list.GroupBy(f => f.Name).Where(g => g.Count() > 1).Select(f => f.Key);
                    if (duplicates.Any())
                        context.AddFailure($"Model {context.DisplayName} shouldn't have duplicate field names ({string.Join(", ", duplicates)})");
                });

            RuleFor(x => x.Indexes)
                .Custom((list, context) =>
                {
                    if (!(context.InstanceToValidate is Model model))
                    {
                        context.AddFailure("model does not exist.");
                        return;
                    }
                    foreach (var field in from index in list
                                          from field in index.Fields
                                          where model.Fields.Count(f => f.Name == field.Name) == 0
                                          select field)
                    {
                        context.AddFailure($"Index field '{field.Name}' does not exist in model '{model.Name}'");
                    }
                });

            RuleFor(x => x.Relationships)
                .Custom((list, context) =>
                {
                    if (!(context.InstanceToValidate is Model model))
                    {
                        context.AddFailure("model does not exist.");
                        return;
                    }
                    foreach (var rel in list)
                    {
                        if (rel.PrincipalModel == model.Name)
                        {
                            foreach (var rf in rel.PrincipalFields)
                            {
                                if (!model.AllFields.Any(f => f.Name == rf))
                                    context.AddFailure($"Model '{rel.PrincipalModel}' does not contain relationship field '{rf}'");
                            }
                        }
                        else if (rel.DependantModel == model.Name)
                        {
                            foreach (var rf in rel.DependantFields)
                            {
                                if (!model.AllFields.Any(f => f.Name == rf))
                                    context.AddFailure($"Model '{rel.DependantModel}' does not contain relationship field '{rf}'");
                            }
                        }
                    }
                });
            RuleForEach(x => x.Fields).SetValidator(new FieldValidator());
            RuleForEach(x => x.Indexes).SetValidator(new IndexValidator());
            RuleForEach(x => x.Relationships).SetValidator(new RelationshipValidator());
        }

        private static bool HasAuditFields(IList<Field> fields) => fields.Count(f => f.Name.Singular.Value == "Created" || f.Name.Singular.Value == "CreatedBy" || f.Name.Singular.Value == "Modified" || f.Name.Singular.Value == "ModifiedBy") == 4;
    }
}
