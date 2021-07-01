using System;
using System.Threading.Tasks;
using notifyme.shared.Models;

namespace notifyme.shared.ServiceInterfaces
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
