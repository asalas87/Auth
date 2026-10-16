using Application.Controls.Interfaces;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Partners.Repositories;

public class CompanyRepository(ApplicationDbContext context) : ICompanyRepository, IControlCompanyRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Company company, CancellationToken cancellationToken = default) => await _context.Companies.AddAsync(company, cancellationToken);

    public async Task<List<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Companies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }
    public async Task<Company?> GetByCuitAsync(Cuit cuit, CancellationToken cancellationToken = default) => await _context.Companies.SingleOrDefaultAsync(c => c.CuitCuil == cuit, cancellationToken);

    public async Task<Company?> GetByIdReadOnlyAsync(CompanyId id, CancellationToken cancellationToken = default) => await _context.Companies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    public async Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default) => await _context.Companies.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    public async Task<Company?> GetByIdWithUsersAsync(CompanyId id, CancellationToken cancellationToken = default) =>
        await _context.Companies.Include(u => u.Users).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Company?> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default) =>
        await _context.Companies.SingleOrDefaultAsync(c =>
        c.Name.ToLower().StartsWith(normalizedName),
        cancellationToken);

    public async Task<(List<Company> Companies, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Companies.AsNoTracking().Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(c => c.Name.Contains(filter) || (c.CuitCuil != null && c.CuitCuil.ToString().Contains(filter)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var companies = await query.OrderBy(c => c.Name).ToListAsync(cancellationToken);

        return (companies, totalCount);
    }

    public void Update(Company company) => _context.Companies.Update(company);

    public void Delete(Company company) => _context.Companies.Remove(company);
}
