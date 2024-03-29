using Microsoft.Extensions.Logging;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Core.IServices;
using SFA.DAS.ToolsNotifications.Types.Entities;

using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> GetNotification()
        {
            return await _notificationRepository.GetNotification();
        }

        public async Task SetNotification(Notification notification)
        {
            await _notificationRepository.SetNotification(notification);
        }
    }
}
