using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Persons
{
    public class CreatePersonValidator : ValidatorBase<CreatePersonDto>
    {
        public CreatePersonValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName)
                .NotEmpty().MaximumLength(100);
        }
    }
}
