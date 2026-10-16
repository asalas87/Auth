using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using Domain.Security.Interfaces;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Update;

public sealed class UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCompanyCommand, ErrorOr<Guid>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ICompanyRepository _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    public async Task<ErrorOr<Guid>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(new CompanyId(request.Id), cancellationToken);

        if (company is null)
            return Error.NotFound("Company.NotFound", "La empresa no existe.");

        var existingCompany = await _companyRepository.GetByCuitAsync(request.Cuit, cancellationToken);
        if (existingCompany != null && existingCompany.Id.Value != company.Id.Value)
        {
            return Error.Failure("Company.CuitRegistered", "Ya existe una empresa con ese CUIT.");
        }

        company.UpdateCompany(request.Name, request.Cuit, company.IsActive);

        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return company.Id.Value;
    }
}
