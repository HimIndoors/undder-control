using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UndderControlLib.Dtos;

namespace UndderControl.Validation
{
    public class CowStatusValidator : AbstractValidator<CowStatusDto>
    {
        public CowStatusValidator()
        {
            RuleFor(c => c.CowIdentifier).NotEmpty();
        }
    }
}
