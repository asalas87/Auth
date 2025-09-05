using Domain.Documents.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Interfaces
{
    public interface IDocumentFileRepository
    {
        Task AddAsync(DocumentFile file);
        void Update(DocumentFile file);
        void Delete(DocumentFile file);
        Task<DocumentFile?> GetById(DocumentFileId id);
        Task<(List<DocumentFile> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, UserId? assignedToUserId);
    }
}
