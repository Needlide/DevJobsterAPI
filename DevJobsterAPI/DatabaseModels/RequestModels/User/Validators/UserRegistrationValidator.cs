using DevJobsterAPI.DatabaseModels.Security;
using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.User.Validators;

public class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    public UserRegistrationValidator(IValidator<DatabaseModels.User.User> userValidator)
    {
        RuleFor(x => x.User)
            .SetValidator(userValidator);
    }
}