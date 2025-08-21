using Domain.Partners.Entities;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.GetById;
public class GetCompanyByCuitHandler : IRequestHandler<GetCompanyByCuitQuery, ErrorOr<Company>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByCuitHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ErrorOr<Company>> Handle(GetCompanyByCuitQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByCuitAsync(request.Cuit);

        if (company is null)
            return Error.NotFound("Company.NotFound", "No se encontró empresa con ese CUIT.");

        return company;
    }
}
