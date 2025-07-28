using Application.Controls.Dtos;
using Application.Controls.Queries;
using MediatR;

namespace Application.Controls.Handlers;

public class GetAllCompaniesHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyLookupDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<CompanyLookupDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync(cancellationToken);

        return companies
            .Select(c => new CompanyLookupDto(c.Id.Value, c.Name))
            .ToList();
    }
}
