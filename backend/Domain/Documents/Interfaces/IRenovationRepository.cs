using Domain.Documents.Entities;
using Domain.Partners.Entities;

namespace Domain.Documents.Interfaces;
public interface IRenovationRepository
{
    Task AddAsync(Renovation file);
    void Update(Renovation file);
    void Delete(Renovation file);
    Task<Renovation?> GetByIdAsync(DocumentFileId id);
    Task<(List<Renovation> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId);
}
