using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Create
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(50);
            RuleFor(r => r.LastName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(r => r.PhoneNumber).NotEmpty().MaximumLength(9);
            RuleFor(r => r.Line1).NotEmpty().MaximumLength(20);
            RuleFor(r => r.Line2).MaximumLength(20);
            RuleFor(r=> r.Country).NotEmpty().MaximumLength(3);
            RuleFor(r=>r.City).NotEmpty().MaximumLength(40);
            RuleFor(r => r.State).NotEmpty().MaximumLength(40);
            RuleFor(r => r.ZipCode).NotEmpty().MaximumLength(10);
        }
    }
}
