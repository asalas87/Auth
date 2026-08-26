namespace Application.Partners.Dtos;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CuitCuil { get; set; }
}
