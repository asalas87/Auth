using Application.Common.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using SharedKernel.Entities;
using SharedKernel.Interfaces;

namespace Application.Notifications.Commands
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, ErrorOr<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationHandler(IUnitOfWork unitOfWork, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }

        public async Task<ErrorOr<bool>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification(request.RecipientEmail, request.DocumentId, request.Subject, request.Body, request.Type, request.ExpirationDate);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
