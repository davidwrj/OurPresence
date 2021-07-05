// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
