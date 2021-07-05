// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentValidation;

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
