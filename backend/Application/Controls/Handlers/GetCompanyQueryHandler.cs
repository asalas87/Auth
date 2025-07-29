using Application.Controls.Dtos;
using Application.Controls.Queries;
using Domain.Partners.Entities;
using ErrorOr;
using MediatR;

namespace Application.Controls.Handlers;

public class GetCompanyByCuitQueryHandler : IRequestHandler<GetCompanyByCuitQuery, ErrorOr<Company>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByCuitQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ErrorOr<Company>> Handle(GetCompanyByCuitQuery request, CancellationToken cancellationToken)
    {
        if (await _companyRepository.GetByCuitAsync(request.cuit, cancellationToken) is not Company company)
        {
            return Error.NotFound("Company.NotFound", "The company with the provide CUIT was not found.");
        }
        return company;
    }
}
