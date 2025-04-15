using System.Text.RegularExpressions;
using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.User.Validators;

public class UserValidator : AbstractValidator<DatabaseModels.User.User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email address");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 250).WithMessage("FirstName must be between 2 and 250 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 250).WithMessage("Last name must be between 2 and 250 characters");
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Length(2, 250).WithMessage("Role must be between 2 and 250 characters");
        RuleFor(x => x.Skills)
            .Length(2, 250).WithMessage("Skills must be between 2 and 250 characters");
        RuleFor(x => x.YearsOfExperience)
            .NotEmpty().WithMessage("Years of experience is required")
            .Length(1, 3).WithMessage("Years of experience must be between 1 and 3 characters long")
            .Matches(YearsOfExperienceRegex()).WithMessage("Years of experience must be a number between 0 and 999");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .Length(3, 250).WithMessage("Location must be between 3 and 250 characters");
        RuleFor(x => x.EnglishLevel)
            .NotEmpty().WithMessage("English level is required")
            .Length(1);
    }

    private static Regex YearsOfExperienceRegex()
    {
        Regex regex = new(@"^(?:[0-9]|[1-9][0-9]|[1-9][0-9]{2})$");
        return regex;
    }
}