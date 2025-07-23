namespace Application.Security.Common.DTOS;

public class UserDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public int RoleId { get; set; } = default!;
}
