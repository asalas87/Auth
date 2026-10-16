using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Documents.Repositories
{
    public class RenovationRepository(ApplicationDbContext context) : IRenovationRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddAsync(Renovation file) => await _context.Renovations.AddAsync(file);

        public void Delete(Renovation file) => _context.Renovations.Remove(file);

        public void Update(Renovation file) => _context.Renovations.Update(file);

        public async Task<(List<Renovation> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId)
        {
            var query = _context.Renovations
                .Include(d => d.UploadedBy)
                .Include(d => d.AssignedTo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(u => u.Name.Contains(filter) || u.Description.Contains(filter));
            }

            if (assignedToId != null)
                query = query.Where(f => f.AssignedTo != null && f.AssignedTo.Id == assignedToId);

            var totalCount = await query.CountAsync();

            var renovations = await query
                .OrderByDescending(u => u.UploadDate)
                //.Skip((page - 1) * pageSize)
                //.Take(pageSize)
                .ToListAsync();

            return (renovations, totalCount);
        }

        public async Task<Renovation?> GetByIdAsync(DocumentFileId id)
        {
            return await _context.Renovations
                .Include(d => d.UploadedBy)
                .Include(d => d.AssignedTo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
