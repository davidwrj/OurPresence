using OurPresence.Modeller.Interfaces;
using FluentValidation;

namespace OurPresence.Modeller.Generator.Validators
{
    public class ContextValidator : AbstractValidator<IContext>
    {
        public ContextValidator()
        {
            RuleFor(x => x.Generator).NotNull();
            RuleFor(x => x.Module).NotNull();
        }
    }
}
