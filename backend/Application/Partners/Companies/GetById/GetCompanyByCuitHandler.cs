using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetById;
public class GetCompanyByCuitHandler(ICompanyRepository companyRepository) : IRequestHandler<GetCompanyByCuitQuery, ErrorOr<Company>>
{
    private readonly ICompanyRepository _companyRepository = companyRepository;

    public async Task<ErrorOr<Company>> Handle(GetCompanyByCuitQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByCuitAsync(request.Cuit, cancellationToken);

        if (company is null)
            return Error.NotFound("Company.NotFound", "No se encontró empresa con ese CUIT.");

        return company;
    }
}
