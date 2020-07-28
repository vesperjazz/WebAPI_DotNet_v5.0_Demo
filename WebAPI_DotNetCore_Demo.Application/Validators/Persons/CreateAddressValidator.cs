using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Persons
{
    public class CreateAddressValidator : ValidatorBase<CreateAddressDto>
    {
        public CreateAddressValidator()
        {
            RuleFor(a => a.AddressType)
                .MaximumLength(20)
                .When(a => !string.IsNullOrWhiteSpace(a.AddressType));
            RuleFor(a => a.FirstLine)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(a => a.SecondLine)
                .MaximumLength(100)
                .When(a => !string.IsNullOrWhiteSpace(a.SecondLine));
            RuleFor(a => a.ThirdLine)
                .MaximumLength(100)
                .When(a => !string.IsNullOrWhiteSpace(a.ThirdLine));
            RuleFor(a => a.PostCode)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(a => a.City)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(a => a.State)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(a => a.CountryID)
                .NotEmpty();
        }
    }
}
