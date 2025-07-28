using Domain.Partners.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Partners.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;
    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }
}
