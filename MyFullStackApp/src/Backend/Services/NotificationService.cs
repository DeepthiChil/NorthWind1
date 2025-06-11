using Microsoft.AspNetCore.SignalR;
using MyFullStackApp.Backend;

namespace MyFullStackApp.Backend.Services
{
    public interface INotificationService
    {
        Task NotifyAllClientsAsync(string type);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IFirebasePushService _firebasePushService;

        public NotificationService(
            IHubContext<NotifyHub> hubContext,
            IFirebasePushService firebasePushService)
        {
            _hubContext = hubContext;
            _firebasePushService = firebasePushService;
        }

        public async Task NotifyAllClientsAsync(string type)
        {
            // Notify all SignalR-connected clients
            await _hubContext.Clients.All.SendAsync("DataChanged", type);

            // Send Firebase Push Notification
            await _firebasePushService.SendNotificationAsync(type);
        }
    }
}
