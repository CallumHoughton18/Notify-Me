using System;
using System.Text.Json;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Service_Interfaces;
using WebPush;
using VapidDetails = notifyme.shared.Models.VapidDetails;

namespace notifyme.server.Services
{
    public class PushNotificationPusherPusherService : IPushNotificationPusherService
    {
        private readonly WebPushClient _webPushClient = new();

        public bool SendPushNotification(SavedNotificationSubscription subscription, Notification notification, VapidDetails details)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    notification.NotificationTitle,
                    notification.NotificationBody
                });
                _webPushClient.SendNotification(subscription.ToPushSubscription(), payload,
                    details.ToWebPushVapidDetails());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}