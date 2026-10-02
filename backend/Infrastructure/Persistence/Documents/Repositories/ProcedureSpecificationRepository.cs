using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Enums;
using Domain.Partners.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Documents.Repositories;

public class ProcedureSpecificationRepository(ApplicationDbContext context) : IProcedureSpecificationRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(ProcedureSpecification file) => await _context.ProcedureSpecifications.AddAsync(file);

    public void Delete(ProcedureSpecification file) => _context.ProcedureSpecifications.Remove(file);

    public void Update(ProcedureSpecification file) => _context.ProcedureSpecifications.Update(file);

    public async Task<(List<ProcedureSpecification> Files, int TotalCount)> GetPaginatedByAssignedToAsync(int page, int pageSize, string? filter, CompanyId? assignedToId)
    {
        var query = _context.ProcedureSpecifications
            .Include(document => document.UploadedBy)
            .Include(document => document.AssignedTo)
            .Where(document => document.DocumentType == DocumentType.WeldingProcedure);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(document => document.Name.Contains(filter) || document.Description.Contains(filter));
        }

        if (assignedToId is not null)
        {
            query = query.Where(document => document.AssignedTo != null && document.AssignedTo.Id.Value == assignedToId.Value);
        }

        var totalCount = await query.CountAsync();
        var documents = await query.OrderByDescending(document => document.UploadDate).ToListAsync();
        return (documents, totalCount);
    }

    public async Task<ProcedureSpecification?> GetByIdAsync(DocumentFileId id)
    {
        return await _context.ProcedureSpecifications
            .Include(document => document.UploadedBy)
            .Include(document => document.AssignedTo)
            .FirstOrDefaultAsync(document => document.Id.Value == id.Value && document.DocumentType == DocumentType.WeldingProcedure);
    }
}