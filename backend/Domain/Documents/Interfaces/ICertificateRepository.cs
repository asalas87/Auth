using Domain.Documents.Entities;

namespace Domain.Documents.Interfaces
{
    public interface ICertificateRepository
    {
        Task AddAsync(Certificate file);
        void Update(Certificate file);
        void Delete(Certificate file);
        Task<Certificate?> GetByIdAsync(Guid id);
        Task<(List<Certificate> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, Guid? assignedToId);
    }
}
