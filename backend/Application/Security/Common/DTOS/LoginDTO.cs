using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Debes ingresar email valido"), DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar tu contraseña."), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
