using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Persons
{
    public class UpdatePhoneNumberValidator : ValidatorBase<UpdatePhoneNumberDto>
    {
        public UpdatePhoneNumberValidator()
        {
            RuleFor(pn => pn.PhoneNumberType)
                .MaximumLength(20)
                .When(pn => !string.IsNullOrWhiteSpace(pn.PhoneNumberType));
            RuleFor(pn => pn.Number)
                .NotEmpty()
                .MaximumLength(20);
            RuleFor(pn => pn.CountryID)
                .NotEmpty();
        }
    }
}
