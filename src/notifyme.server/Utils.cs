using notifyme.shared.Models.DataStore_Models;
using WebPush;
using SharedVapidDetails = notifyme.shared.Models.VapidDetails;
using VapidDetails = WebPush.VapidDetails;

namespace notifyme.server
{
    public static class Utils
    {
        public static PushSubscription ToPushSubscription(this SavedNotificationSubscription subscription)
        {
            return new(subscription.EndPoint, subscription.P256HKey, subscription.AuthKey);
        }

        public static VapidDetails ToWebPushVapidDetails(this SharedVapidDetails details)
        {
            return new(details.MailTo, details.PublicKey, details.PrivateKey);
        }
    }
}