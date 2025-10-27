using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS;
public class ActivateAccountDTO
{
    public string Token { get; set; } = string.Empty;
    [Required(ErrorMessage = "El password es obligatorio.")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Debes confirmar tu contraseña."), Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
};
