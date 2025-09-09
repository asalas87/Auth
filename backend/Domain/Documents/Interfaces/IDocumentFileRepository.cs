using System.Reflection.Metadata;
using Domain.Documents.Entities;

namespace Domain.Documents.Interfaces
{
    public interface IDocumentFileRepository
    {
        Task AddAsync(DocumentFile file);
        void Update(DocumentFile file);
        void Delete(DocumentFile file);
        Task<(List<DocumentFile> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, Guid? assignedToId);
    }
}
