using FluentValidation;

namespace Application.Security.Users.Delete;
public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(r => r.UserId).NotEmpty();
    }
}

