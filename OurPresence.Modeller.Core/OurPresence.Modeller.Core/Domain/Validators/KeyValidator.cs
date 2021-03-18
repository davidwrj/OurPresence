using FluentValidation;

namespace OurPresence.Modeller.Domain.Validators
{
    public class KeyValidator : AbstractValidator<Key>
    {
        public KeyValidator()
        {
            RuleFor(x => x.Fields).Must(l => l.Count > 0).WithMessage("Key must contain at least one field");
            RuleForEach(x => x.Fields).SetValidator(new FieldValidator());
        }
    }
}
