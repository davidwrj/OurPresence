using FluentValidation;
using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.Domain.Validators
{
    public class ModuleValidator : AbstractValidator<Module>
    {
        public ModuleValidator()
        {
            RuleFor(x => x.Company).NotNull().NotEmpty();
            RuleFor(x => x.Project).Must(x => !string.IsNullOrWhiteSpace(x.ToString()));
            RuleFor(x => x.Models).NotNull();
            RuleForEach(x => x.Models).SetValidator(new ModelValidator());
        }
    }
}
