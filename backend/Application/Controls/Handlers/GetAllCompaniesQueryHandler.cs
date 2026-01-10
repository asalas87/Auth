using Application.Controls.Interfaces;
using Application.Controls.Queries;
using Domain.Partners.Entities;
using ErrorOr;
using MediatR;

namespace Application.Controls.Handlers;

public class GetAllCompaniesQueryHandler(IControlCompanyRepository companyRepository) : IRequestHandler<GetAllCompaniesQuery, ErrorOr<List<Company>>>
{
    private readonly IControlCompanyRepository _companyRepository = companyRepository;

    public async Task<ErrorOr<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await _companyRepository.GetAllAsync(cancellationToken);
    }
}
