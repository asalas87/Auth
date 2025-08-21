using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Create;
public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, ErrorOr<Guid>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var existingCompany = await _companyRepository.GetByCuitAsync(request.Cuit);
        if (existingCompany != null)
        {
            return Error.Failure("Company.CuitRegistered", "Ya existe una empresa con ese CUIT.");
        }

        var company = new Company(
            request.Name,
            Cuit.Create(request.Cuit)!
        );

        await _companyRepository.AddAsync(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return company.Id.Value;
    }
}
