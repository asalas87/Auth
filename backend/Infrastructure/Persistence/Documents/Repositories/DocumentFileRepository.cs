using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Security.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Documents.Repositories
{
    public class DocumentFileRepository : IDocumentFileRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentFileRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(DocumentFile file) => await _context.DocumentFiles.AddAsync(file);

        public void Delete(DocumentFile file) => _context.DocumentFiles.Remove(file);

        public void Update(DocumentFile file) => _context.DocumentFiles.Update(file);

        public async Task<(List<DocumentFile> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, UserId? assignedToUserId)
        {
            var query = _context.DocumentFiles
                .Include(d => d.UploadedBy)
                .Include(d => d.AssignedTo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(u => u.Name.Contains(filter) || u.Description.Contains(filter));
            }

            if (assignedToUserId != null)
            {
                query = query.Where(f => f.AssignedTo != null && f.AssignedTo.Users.Any(u => u.Id == assignedToUserId));
            }

            var totalCount = await query.CountAsync();

            var files = await query
                .OrderByDescending(u => u.ExpirationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (files, totalCount);
        }
    }
}
