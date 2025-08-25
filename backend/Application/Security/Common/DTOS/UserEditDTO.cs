namespace Application.Security.Common.DTOS;
public class UserEditDTO : UserDTO
{
    public int RoleId { get; set; } = default!;
    public Guid? CompanyId { get; set; } = default!;
}
