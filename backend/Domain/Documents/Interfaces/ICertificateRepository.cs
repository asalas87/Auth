using Domain.Documents.Entities;
using Domain.Partners.Entities;

namespace Domain.Documents.Interfaces
{
    public interface ICertificateRepository
    {
        Task AddAsync(Certificate file);
        void Update(Certificate file);
        void Delete(Certificate file);
        Task<Certificate?> GetByIdAsync(DocumentFileId id);
        Task<(List<Certificate> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId);
    }
}
