using DevJobsterAPI.DatabaseModels.Security;
using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Recruiter.Validators;

public class RecruiterRegistrationValidator : AbstractValidator<RecruiterRegistration>
{
    public RecruiterRegistrationValidator(
        IValidator<DatabaseModels.Recruiter.Recruiter> recruiterValidator)
    {
        RuleFor(x => x.Recruiter)
            .SetValidator(recruiterValidator);
    }
}