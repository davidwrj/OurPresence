using FluentValidation;
using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.Domain.Validators
{
    public class FieldValidator : AbstractValidator<Field>
    {
        public FieldValidator()
        {
            RuleFor(x => x.Scale).GreaterThanOrEqualTo(0).When(x => x.Scale.HasValue && x.DataType == DataTypes.Decimal);
            RuleFor(x => x.Scale).Must(x => !x.HasValue).When(x => x.DataType != DataTypes.Decimal).WithMessage("'{PropertyName}' can't be set when DataType is not a Number.");

            RuleFor(x => x.MaxLength).InclusiveBetween(0, 4096).When(x => x.DataType == DataTypes.String);
            RuleFor(x => x.MaxLength).Must(x => !x.HasValue).When(x => x.DataType != DataTypes.String);

            RuleFor(x => x.MinLength).InclusiveBetween(0, 4096).When(x => x.MinLength.HasValue && x.DataType == DataTypes.String);
            RuleFor(x => x.MinLength).LessThanOrEqualTo(x => x.MaxLength).When(x => x.MaxLength.HasValue);
            RuleFor(x => x.MinLength).Must(x => !x.HasValue).When(x => x.DataType != DataTypes.String);
        }
    }
}
