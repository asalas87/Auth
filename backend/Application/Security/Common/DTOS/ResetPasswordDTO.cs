using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS;

public class ResetPasswordDTO
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El token es obligatorio.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "El password es obligatorio.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes confirmar tu contraseña.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? CaptchaToken { get; set; }
}
