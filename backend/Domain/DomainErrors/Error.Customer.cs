using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Customer
        {
            public static Error  PhoneNumberWithBadFormat => Error.Validation("Customer.PhoneNumber", "Phone number has not valid format");
            public static Error AddressWithBadFormat => Error.Validation("Customer.Address", "Address is not valid");
        }
    }
}
