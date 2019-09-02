using FluentValidation;
using UndderControlLib.Dtos;

namespace UndderControl.Validation
{
    
    public class FarmValidator : AbstractValidator<FarmDto>
    {
        public FarmValidator()
        {
            RuleFor(farm => farm.Address).NotEmpty();
            RuleFor(farm => farm.ContactName).NotEmpty();
            RuleFor(farm => farm.HerdSize).GreaterThan(0);
            RuleFor(farm => farm.Name).NotEmpty();
            RuleFor(farm => farm.PhoneNumber).NotEmpty().Matches(@"^(?:0|\+?44)(?:\d\s?){9,10}$");
            RuleFor(farm => farm.Type).NotEmpty();
        }
    }
}
