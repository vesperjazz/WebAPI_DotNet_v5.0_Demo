using FluentValidation;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;

namespace WebAPI_DotNetCore_Demo.Application.Validators.Users
{
    public sealed class LoginUserValidator : ValidatorBase<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty();
            RuleFor(u => u.Password)
                .NotEmpty();
        }
    }
}
