using FluentValidation;

namespace DevJobsterAPI.Models.RequestModels.Vacancy.Validators;

public class AddVacancyValidator : AbstractValidator<AddVacancy>
{
    public AddVacancyValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(10, 250).WithMessage("Title must be between 10 and 250 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(30, 700).WithMessage("Description must be between 30 and 700 characters");
        RuleFor(x => x.Salary)
            .NotEmpty().WithMessage("Salary is required")
            .GreaterThan(0).WithMessage("Salary must be greater than 0");
        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("Requirements cannot be empty")
            .Length(30, 250).WithMessage("Requirements must be between 30 and 250 characters");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("Company's website link is required")
            .Length(8, 250).WithMessage("Company's website link must be between 8 and 250 characters");
        RuleFor(x => x.TypeOfJob)
            .NotEmpty()
            .Length(1);
        RuleFor(x => x.Location)
            .NotEmpty()
            .Length(1);
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country's name is required")
            .Length(3, 250).WithMessage("Country's name must be between 3 and 250 characters");
        RuleFor(x => x.Benefits)
            .Length(15, 250).WithMessage("Benefits must be between 15 and 250 characters");
    }
}