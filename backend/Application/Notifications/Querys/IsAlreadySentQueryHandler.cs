using Application.Common.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using SharedKernel.Interfaces;

namespace Application.Notifications.Querys;
public class IsAlreadySentQueryHandler : IRequestHandler<IsAlreadySentQuery, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _notificationRepository;

    public IsAlreadySentQueryHandler(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
    {
        _unitOfWork = unitOfWork;
        _notificationRepository = notificationRepository;
    }

    public async Task<ErrorOr<bool>> Handle(IsAlreadySentQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.IsAlreadySentAsync(request.DocumentId, cancellationToken);
    }
}

