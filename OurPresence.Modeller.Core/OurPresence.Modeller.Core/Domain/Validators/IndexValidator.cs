// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentValidation;

namespace OurPresence.Modeller.Domain.Validators
{
    public class IndexValidator : AbstractValidator<Index>
    {
        public IndexValidator()
        {
            RuleForEach(x => x.Fields).SetValidator(new IndexFieldValidator());
            RuleFor(x => x.Fields).NotEmpty().WithMessage(m => $"Index '{m.Name}' must have at least one field");
        }
    }
}
