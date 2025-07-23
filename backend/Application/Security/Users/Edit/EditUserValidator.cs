using FluentValidation;

namespace Application.Security.Users.Edit;
public class EditUserValidator : AbstractValidator<EditUserCommand>
{
    public EditUserValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.RoleId).NotEmpty();
    }
}
