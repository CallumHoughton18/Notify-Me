using System.ComponentModel.DataAnnotations;

namespace notifyme.shared.Models.DataStore_Models
{
    public class SavedNotificationSubscription : BaseEntity
    {
        [Key]
        public string P256HKey { get; set; }
        [Required]
        public string AuthKey { get; set; }
        [Required]
        public string EndPoint { get; set; }
        public User User { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
