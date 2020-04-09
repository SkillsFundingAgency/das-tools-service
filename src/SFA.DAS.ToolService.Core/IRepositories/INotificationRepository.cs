using SFA.DAS.ToolsNotifications.Types.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Core.IRepositories
{
    public interface INotificationRepository
    {
        Task<Notification> GetNotification();

        Task SetNotification(Notification notification);
    }
}
