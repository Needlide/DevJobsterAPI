using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Security.Validators;

public class LoginRegisterModelValidator : AbstractValidator<LoginRegisterModel>
{
    public LoginRegisterModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email address");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(8, 250).WithMessage("Password must be between 8 and 250 characters");
    }
}