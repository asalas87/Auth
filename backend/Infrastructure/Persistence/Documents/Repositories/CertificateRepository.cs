using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Security.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Documents.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Certificate file) => await _context.Certificates.AddAsync(file);

        public void Delete(Certificate file) => _context.Certificates.Remove(file);

        public void Update(Certificate file) => _context.Certificates.Update(file);

        public async Task<(List<Certificate> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId)
        {
            var query = _context.Certificates
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

            var certificates = await query
                .OrderByDescending(u => u.ExpirationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (certificates, totalCount);
        }

        public async Task<Certificate?> GetByIdAsync(DocumentFileId id)
        {
            return await _context.Certificates
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
