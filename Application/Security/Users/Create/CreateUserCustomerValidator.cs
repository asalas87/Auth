using Application.Security.Users.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sales.Customers.Delete
{
    public class CreateUserCustomerValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCustomerValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.ConfirmPassword).NotEmpty().Equal(user => user.Password);
        }
    }
}
