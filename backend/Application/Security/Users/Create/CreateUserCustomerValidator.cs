using FluentValidation;

namespace Application.Security.Users.Create;
public class CreateUserCustomerValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCustomerValidator()
    {
        RuleFor(r => r.Email).NotEmpty();
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.CompanyId).NotEmpty();
    }
}
