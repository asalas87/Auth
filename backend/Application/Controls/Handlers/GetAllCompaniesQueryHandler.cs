using Application.Controls.Dtos;
using Application.Controls.Queries;
using Domain.Partners.Entities;
using ErrorOr;
using MediatR;

namespace Application.Controls.Handlers;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, ErrorOr<List<Company>>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ErrorOr<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await _companyRepository.GetAllAsync(cancellationToken);
    }
}
