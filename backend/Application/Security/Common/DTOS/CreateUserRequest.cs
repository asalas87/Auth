using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOS;
public class CreateUserRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public int RoleId { get; set; } = default!;
    public Guid? CompanyId { get; set; } = default!;
}
