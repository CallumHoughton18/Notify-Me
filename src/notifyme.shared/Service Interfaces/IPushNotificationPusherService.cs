using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.ServiceInterfaces
{
    public interface IPushNotificationPusherService
    {
        public bool SendPushNotification(SavedNotificationSubscription subscription, Notification notification,
            VapidDetails details);
    }
}