using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS;

public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get; set; } = string.Empty;
}
