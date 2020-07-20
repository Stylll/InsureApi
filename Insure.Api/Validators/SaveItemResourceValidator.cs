using FluentValidation;
using Insure.Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Validators
{
    public class SaveItemResourceValidator : AbstractValidator<SaveItemResource>
    {
        public SaveItemResourceValidator()
        {
            RuleFor(i => i.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(i => i.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(i => i.CategoryId).NotEmpty().WithMessage("CategoryId cannot be empty");
            RuleFor(i => i.Value).GreaterThan(0).WithMessage("Value must be greater than zero");
        }
    }
}
