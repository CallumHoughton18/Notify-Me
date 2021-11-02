using System.Threading.Tasks;
using notifyme.shared.Models;

namespace notifyme.shared.Service_Interfaces
{
    public interface IPushNotificationSubscriberService
    {
        Task Initialize();
        Task<bool> CheckAndRequestNotificationPermission();
        Task RegisterSubscription();
        Task<NotificationSubscription> GetCurrentUserAndDeviceSubscription();
        Task<bool> UnsubscribeFromNotifications();
    }
}
