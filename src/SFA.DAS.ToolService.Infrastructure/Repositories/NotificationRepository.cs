using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolsNotifications.Client;
using SFA.DAS.ToolsNotifications.Types.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly INotificationClient _notificationClient;

        public NotificationRepository(INotificationClient notificationClient)
        {
            _notificationClient = notificationClient;
        }

        public async Task<Notification> GetNotification()
        {
            return await _notificationClient.GetNotification();
        }

        public async Task SetNotification(Notification notificaiton)
        {
            await _notificationClient.SetNotification(notificaiton);
        }
    }
}
