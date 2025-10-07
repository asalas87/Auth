namespace Application.Documents.Services
{
    public interface INotificationService
    {
        Task<int> CreateExpiringDocumentNotificationsAsync(CancellationToken cancellationToken = default);
        Task<int> SendPendingNotificationsAsync(CancellationToken cancellationToken = default);
    }
}
