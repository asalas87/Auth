using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Partners.Companies.Delete;

public sealed class DeleteCompanyHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCompanyCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(new CompanyId(request.Id), cancellationToken);

        if (company is null)
            return Error.NotFound("Company.NotFound", "La empresa no existe.");

        companyRepository.Delete(company);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
