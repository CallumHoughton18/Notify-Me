using System;
namespace notifyme.shared.Models
{
    public class NotificationSubscription
    {
        public string P256hKey { get; set; }
        public string AuthKey { get; set; }
        public string EndPoint { get; set; }
    }
}
