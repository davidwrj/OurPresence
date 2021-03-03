using OurPresence.Modeller.Interfaces;
using FluentValidation;
using System.IO;

namespace OurPresence.Modeller.Generator.Validators
{
    public class GeneratorConfigurationValidator : AbstractValidator<IGeneratorConfiguration>
    {
        public GeneratorConfigurationValidator()
        {
            RuleFor(x => x.SourceModel).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();
            RuleFor(x => x.LocalFolder).Must(x => Directory.Exists(x)).WithMessage(m => $"Local folder not found '{m.LocalFolder}'");
            RuleFor(x => x.GeneratorName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty();


        }
    }
}
