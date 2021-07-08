using System.ComponentModel.DataAnnotations;

namespace notifyme.shared.Models
{
    public class QuickNotification
    {
        [Required(ErrorMessage = "Notification title is required")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Notification body is required")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Requested time is required")] 
        [Range(1, 1000, ErrorMessage = "Requested time must be between 1 and 1000")] 
        public int RequestedTime { get; set; } = 1;

        [Required(ErrorMessage = "Time format is required")]
        public NotifyMeEnums.QuickNotificationTimeFormat TimeFormat { get; set; } =
            NotifyMeEnums.QuickNotificationTimeFormat.Minutes;
    }
}