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
        public static class User
        {
            public static Error UserOrPasswordIncorrect => Error.Validation("User.UserOrPasswordIncorrect", "Usuario o contraseña incorrectos");
        }
    }
}
