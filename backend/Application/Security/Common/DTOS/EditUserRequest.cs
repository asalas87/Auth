using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOs;

public class EditUserRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public int RoleId { get; set; } = default!;
    [Required]
    public Guid CompanyId { get; set; } = default!;
}
