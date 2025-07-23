using System.ComponentModel.DataAnnotations;

namespace Application.Security.Common.DTOs;

public class EditUserRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Role { get; set; } = default!;
}
