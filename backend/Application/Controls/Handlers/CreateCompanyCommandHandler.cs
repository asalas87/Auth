using Application.Controls.Commands;
using Domain.Partners.Entities;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Controls.Handlers;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, ErrorOr<Guid>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = new Company(new CompanyId(Guid.NewGuid()), request.Name, request.Cuit, true);
        var id = await _companyRepository.AddAsync(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return id;
    }
}
