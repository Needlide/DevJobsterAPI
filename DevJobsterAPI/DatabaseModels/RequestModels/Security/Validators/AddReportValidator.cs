using DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;
using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Security.Validators;

public class AddReportValidator : AbstractValidator<AddReport>
{
    public AddReportValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(5, 250).WithMessage("Title must be between 5 and 250 characters");
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Report message is required")
            .Length(15, 1000).WithMessage("Report message must be between 15 and 1000 characters");
    }
}