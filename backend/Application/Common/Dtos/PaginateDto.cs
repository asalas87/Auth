namespace Application.Common.Dtos;
public class PaginateDTO
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Filter { get; set; }
}
