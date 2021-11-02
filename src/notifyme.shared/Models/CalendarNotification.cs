using System;
using System.ComponentModel.DataAnnotations;

namespace notifyme.shared.Models
{
    public class CalendarNotification
    {
        [Required(ErrorMessage = "Notification title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Notification body is required")]
        public string Body { get; set; }

        [Required(ErrorMessage = "First date occurance is required")]
        public DateTime? FirstDateOccurance { get; set; }

        [Required(ErrorMessage = "First time occurancr is required")]
        public TimeSpan? FirstTimeOccurance { get; set; }

        [Required(ErrorMessage = "Time format is required")]
        public NotifyMeEnums.CalendarNotificationRepeatFormat RepeatFormat { get; set; } =
            NotifyMeEnums.CalendarNotificationRepeatFormat.None;
    }
}