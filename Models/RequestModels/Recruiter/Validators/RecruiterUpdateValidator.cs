using System.Text.RegularExpressions;
using FluentValidation;

namespace DevJobsterAPI.Models.RequestModels.Recruiter.Validators;

public partial class RecruiterUpdateValidator : AbstractValidator<RecruiterUpdate>
{
    public RecruiterUpdateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 250).WithMessage("First name must be between 2 and 250 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 250).WithMessage("Last name must be between 2 and 250 characters");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(12).WithMessage("Phone number cannot exceed 12 characters")
            .Matches(PhoneNumberRegex())
            .WithMessage(
                "Invalid phone number format. Must start with '+' and contain only digits, up to 11 digits after the +");
        RuleFor(x => x.Notes)
            .MaximumLength(250).WithMessage("Notes cannot exceed 250 characters");
    }

    [GeneratedRegex(@"^\+?(\d[\s-()]*){1,11}$")]
    private static partial Regex PhoneNumberRegex();
}