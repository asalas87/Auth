using Domain.Documents.Entities;
using Domain.Partners.Entities;

namespace Domain.Documents.Interfaces;

public interface IProcedureSpecificationRepository
{
    Task AddAsync(ProcedureSpecification file);
    void Update(ProcedureSpecification file);
    void Delete(ProcedureSpecification file);
    Task<ProcedureSpecification?> GetByIdAsync(DocumentFileId id);
    Task<(List<ProcedureSpecification> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId);
}