using Domain.Documents.Entities;
using Domain.Partners.Entities;

namespace Domain.Documents.Interfaces;

public interface IProcedureSpecificationRecordRepository
{
    Task AddAsync(ProcedureSpecificationRecord file);
    void Update(ProcedureSpecificationRecord file);
    void Delete(ProcedureSpecificationRecord file);
    Task<ProcedureSpecificationRecord?> GetByIdAsync(DocumentFileId id);
    Task<(List<ProcedureSpecificationRecord> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId);
}