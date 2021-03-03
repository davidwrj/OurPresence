using FluentValidation.Results;

namespace OurPresence.Modeller.Interfaces
{
    public interface IContext
    {
        INamedElement? Module { get; }

        INamedElement? Model { get; }

        IGeneratorItem? Generator { get; }

        ISettings Settings { get; }

        ValidationResult ValidateConfiguration(IGeneratorConfiguration generatorConfiguration);

        bool IsValid();
    }
}
