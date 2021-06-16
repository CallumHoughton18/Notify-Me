using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace notifyme.shared.Models.DataStore_Models
{
    public class Notification : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid NotificationId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string NotificationTitle { get; set; }
        [Required]
        public string NotificationBody { get; set; }
        [Required]
        [RegularExpression(Constants.CustomCronExpressionRegex)]
        public string CronJobString { get; set; }
    }
}
