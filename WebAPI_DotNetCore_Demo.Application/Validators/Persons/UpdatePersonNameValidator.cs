using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Persons
{
    public class UpdatePersonNameValidator : ValidatorBase<UpdatePersonNameDto>
    {
        public UpdatePersonNameValidator()
        {
            RuleFor(u => u.ID)
                .NotEmpty();
            RuleFor(u => u.FirstName)
                .NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName)
                .NotEmpty().MaximumLength(100);
        }
    }
}
