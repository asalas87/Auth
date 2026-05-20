using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SharedKernel.Enums;

namespace Infrastructure.Persistence.Documents.Repositories;

public class DocumentFileRepository(ApplicationDbContext context) : IDocumentFileRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(DocumentFile file) => await _context.DocumentFiles.AddAsync(file);

    public void Delete(DocumentFile file) => _context.DocumentFiles.Remove(file);

    public void Update(DocumentFile file) => _context.DocumentFiles.Update(file);
    public async Task<DocumentFile?> GetByIdAsync(DocumentFileId id) => await _context.DocumentFiles.FindAsync(id);

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

    public async Task<List<DocumentFile>> GetExpiringDocumentsNotSendAsync(int batchDays)
    {
        var limitDate = DateTime.UtcNow.AddDays(batchDays);

        // 🔥 1. IDs ya notificados
        var notifiedIds = await _context.Notifications
            .Where(n =>
                n.Type == NotificationType.DocumentExpiring &&
                NotificationStatus.Sent == n.Status &&
                n.DocumentId != null
            )
            .Select(n => n.DocumentId!.Value)
            .Distinct()
            .ToListAsync();

        // 🔥 2. Traigo candidatos (DB)
        var candidates = await _context.DocumentFiles
            .Where(d =>
                d.ExpirationDate != null &&
                d.ExpirationDate < limitDate &&
                d.AssignedTo != null &&
                d.AssignedTo.Users.Any()
            )
            .Include(d => d.AssignedTo!)
                .ThenInclude(c => c.Users)
            .OrderBy(d => d.ExpirationDate)
            .ToListAsync(); // 👈 🔥 CORTE A MEMORIA

        // 🔥 3. Filtro en memoria (LINQ to Objects)
        var result = candidates
            .Where(d => !notifiedIds.Contains(d.Id.Value))
            .ToList();

        return result;
    }

    public IQueryable<DocumentFile> GetUserDocuments(UserId assignedToUserId)
    {
        return _context.DocumentFiles
            .Include(d => d.AssignedTo!)
                .ThenInclude(c => c.Users)
            .Where(f => f.AssignedTo != null &&
                        f.AssignedTo.Users.Any(u => u.Id == assignedToUserId));
    }

    public async Task<List<DocumentFile>> GetListByIdsAsync(List<DocumentFileId> ids) => await _context.DocumentFiles.Where(d => ids.Contains(d.Id))
    .ToListAsync();
}
