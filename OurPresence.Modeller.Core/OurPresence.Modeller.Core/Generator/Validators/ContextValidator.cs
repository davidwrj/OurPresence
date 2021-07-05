// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
