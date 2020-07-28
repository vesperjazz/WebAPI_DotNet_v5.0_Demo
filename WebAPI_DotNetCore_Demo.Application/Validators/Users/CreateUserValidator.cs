using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Users
{
    public sealed class CreateUserValidator : ValidatorBase<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName)
                .NotEmpty().MaximumLength(100);
            RuleFor(u => u.UserName)
                .NotEmpty().MaximumLength(100);
            //@TODO Add complex password validation,
            // e.g. Capital, numeric or special characters requirement.
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(20)
                .Equal(u => u.ConfirmPassword);
            RuleFor(u => u.RoleIDs)
                .NotEmpty();
        }
    }
}
