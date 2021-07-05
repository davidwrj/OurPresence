// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentValidation.Results;

namespace OurPresence.Modeller.Interfaces
{
    public interface IContext
    {
        INamedElement? Module { get; }

        INamedElement? Model { get; }

        IGeneratorItem? Generator { get; }

        ISettings Settings { get; }

        ValidationResult ValidateConfiguration(IGeneratorConfiguration configuration);
    }
}
