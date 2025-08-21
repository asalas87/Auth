using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS;
public class RegisterDTO : LoginDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Debes confirmar tu contraseña."), Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
    public int RoleId { get; set; } = 2;
}
