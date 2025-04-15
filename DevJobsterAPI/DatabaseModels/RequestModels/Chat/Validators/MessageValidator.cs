using FluentValidation;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat.Validators;

public class MessageValidator : AbstractValidator<AddMessageWithSender>
{
    public MessageValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Message can't be empty")
            .MaximumLength(5000).WithMessage("Message's length can't be more than 5000 characters");
    }
}