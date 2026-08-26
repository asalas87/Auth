namespace Application.Partners.Dtos;

public class CompanyFilterDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Filter { get; set; }
}
