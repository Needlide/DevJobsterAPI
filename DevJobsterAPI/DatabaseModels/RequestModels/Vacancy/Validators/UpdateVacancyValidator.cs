using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy.Validators;

public class UpdateVacancyValidator : AbstractValidator<UpdateVacancy>
{
    public UpdateVacancyValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(30, 700).WithMessage("Description must be between 30 and 700 characters");
        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("Requirements cannot be empty")
            .Length(30, 250).WithMessage("Requirements must be between 30 and 250 characters");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("Company's website link is required")
            .MaximumLength(250).WithMessage("Company's website link must be less than 250 characters")
            .Must(BeAValidUrl).WithMessage("Company's website link must be a valid URL");
        RuleFor(x => x.TypeOfJob)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Benefits)
            .Length(15, 250).WithMessage("Benefits must be between 15 and 250 characters");
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}