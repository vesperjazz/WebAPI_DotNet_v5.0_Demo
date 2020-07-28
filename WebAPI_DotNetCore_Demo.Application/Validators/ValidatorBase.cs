using FluentValidation;

namespace WebAPI_DotNetCore_Demo.Application.Validators
{
    public abstract class ValidatorBase<TDto> : AbstractValidator<TDto> where TDto : class, new()
    {
    }
}
