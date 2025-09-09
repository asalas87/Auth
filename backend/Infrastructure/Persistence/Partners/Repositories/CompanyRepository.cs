using Domain.Partners.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Persistence.Partners.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;
    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Company company, CancellationToken cancellationToken = default)
    {
        var result = await _context.Companies.AddAsync(company);
        return result.Entity.Id.Value;
    }

    public async Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }
    public async Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default) => await _context.Companies.SingleOrDefaultAsync(c => c.CuitCuil == cuit, cancellationToken);

    public async Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default) => await _context.Companies.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
}
