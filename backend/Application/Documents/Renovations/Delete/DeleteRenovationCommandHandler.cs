using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Renovation.Delete;

public sealed class DeleteRenovationCommandHandler : IRequestHandler<DeleteRenovationCommand, ErrorOr<Guid>>
{
    private readonly IRenovationRepository _renovationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public DeleteRenovationCommandHandler(IRenovationRepository renovationRepository, IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _renovationRepository = renovationRepository ?? throw new ArgumentNullException(nameof(renovationRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteRenovationCommand request, CancellationToken cancellationToken)
    {
        var renovation = await _renovationRepository.GetByIdAsync(new DocumentFileId(request.Id));
        if (renovation is null)
        {
            return Error.NotFound("Renovation.NotFound", "The renovation with the provided Id was not found.");
        }

        renovation.DeletePhysicalFile(_env.WebRootPath);
        _renovationRepository.Delete(renovation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return renovation.Id.Value;
    }
}
