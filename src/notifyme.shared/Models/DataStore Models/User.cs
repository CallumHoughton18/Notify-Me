using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace notifyme.shared.Models.DataStore_Models
{
    public class User : BaseEntity
    {
        [Key]
        public string UserName { get; set; }
        public ICollection<SavedNotificationSubscription> SavedNotificationSubscriptions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
