using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Persons;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Persons
{
    public class UpdatePersonValidator : ValidatorBase<UpdatePersonDto>
    {
        public UpdatePersonValidator()
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
