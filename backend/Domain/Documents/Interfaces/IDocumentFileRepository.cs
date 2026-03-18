using Domain.Documents.Entities;
using Domain.Security.Entities;

namespace Domain.Documents.Interfaces;
public interface IDocumentFileRepository
{
    Task AddAsync(DocumentFile file);
    void Update(DocumentFile file);
    void Delete(DocumentFile file);
    Task<DocumentFile?> GetByIdAsync(DocumentFileId id);
    Task<List<DocumentFile>> GetListByIdsAsync(List<DocumentFileId> ids);
    Task<(List<DocumentFile> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, UserId? assignedToUserId);
    IQueryable<DocumentFile> GetUserDocuments(UserId assignedToUserId);
    Task<List<DocumentFile>> GetExpiringAsync(int batchSize);
}
