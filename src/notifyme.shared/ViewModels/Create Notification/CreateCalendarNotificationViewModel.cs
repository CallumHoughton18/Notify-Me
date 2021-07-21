using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using notifyme.shared.Helpers;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;

namespace notifyme.shared.ViewModels.CreateNotification
{
    public class CreateCalendarNotificationViewModel : BaseCreateNotificationViewModel
    {
        public CreateCalendarNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService,
            INotificationSchedulerInterface notificationScheduler,
            ICronExpressionBuilder cronExpressionBuilder,
            INotificationRepository notificationRepository,
            IAuthService authService,
            IDateTimeProvider dateTimeProvider) :
            base(pushNotificationSubscriberService,
                notificationScheduler,
                cronExpressionBuilder,
                notificationRepository,
                authService,
                dateTimeProvider)
        {
        }

        private CalendarNotification _calendarNotification = new();
        public CalendarNotification CalendarNotification
        {
            get => _calendarNotification;
            set => SetValue(ref _calendarNotification, value);
        }

        protected override async Task<Notification> NotificationGenerator()
        {
            var isValid = ValidationHelpers.ValidateModel(CalendarNotification);
            if (!isValid.ReturnBool) throw new ValidationException(isValid.ReturnString);
            if (!CalendarNotification.FirstDateOccurance.HasValue) throw new ValidationException($"{nameof(CalendarNotification.FirstDateOccurance)} must have a value");
            if (!CalendarNotification.FirstTimeOccurance.HasValue) throw new ValidationException($"{nameof(CalendarNotification.FirstTimeOccurance)} must have a value");

            var currentUser = await _authService.GetCurrentUserAsync();
            if (!CalendarNotification.FirstDateOccurance.HasValue && !CalendarNotification.FirstTimeOccurance.HasValue) return null;
            var firstDateOccurance = CalendarNotification.FirstDateOccurance.Value;
            var firstTimeOccurance = CalendarNotification.FirstTimeOccurance.Value;
            var firstOccurance = firstDateOccurance.Date + firstTimeOccurance;
            var isRepeatable = CalendarNotification.RepeatFormat != NotifyMeEnums.CalendarNotificationRepeatFormat.None;

            return new Notification()
            {
                NotificationTitle = CalendarNotification.Title,
                NotificationBody = CalendarNotification.Body,
                UserName = currentUser.UserName,
                CronJobString = _cronExpressionBuilder.RepeatableDateTimeToCronExpression(firstOccurance, CalendarNotification.RepeatFormat),
                Repeat = isRepeatable
            };
        }

        protected override void ResetState()
        {
            CalendarNotification = new();
        }
    }
}
