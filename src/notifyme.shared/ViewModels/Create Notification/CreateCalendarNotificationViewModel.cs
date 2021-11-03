using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using notifyme.shared.Helpers;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Repository_Interfaces;
using notifyme.shared.Service_Interfaces;

namespace notifyme.shared.ViewModels.Create_Notification
{
    public class CreateCalendarNotificationViewModel : BaseCreateNotificationViewModel
    {
        private readonly IClientDateTimeProvider _clientDateTimeProvider;

        public CreateCalendarNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService,
            INotificationSchedulerInterface notificationScheduler,
            ICronExpressionBuilder cronExpressionBuilder,
            INotificationRepository notificationRepository,
            IAuthService authService,
            IServerDateTimeProvider serverServerDateTimeProvider, 
            IClientDateTimeProvider clientDateTimeProvider) :
            base(pushNotificationSubscriberService,
                notificationScheduler,
                cronExpressionBuilder,
                notificationRepository,
                authService,
                serverServerDateTimeProvider)
        {
            _clientDateTimeProvider = clientDateTimeProvider;
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
            var firstDateOccurance = CalendarNotification.FirstDateOccurance.Value;
            var firstTimeOccurance = CalendarNotification.FirstTimeOccurance.Value;
            var firstOccurance = firstDateOccurance.Date + firstTimeOccurance;
            // Assume here that the user always enters the FirstTimeOccurance using their timezone
            // would have been better to use DateTimeOffsets from the start but this fix should be fine for basic use cases
            var utcClientOffset = await _clientDateTimeProvider.GetClientTimeZoneOffsetInMinutes();
            var firstOccuranceInUTC = firstOccurance.AddMinutes(utcClientOffset);
            var convertedFirstOccurance =
                TimeZoneInfo.ConvertTimeFromUtc(firstOccuranceInUTC, _serverServerDateTimeProvider.CurrentTimeZone);
            
            var isRepeatable = CalendarNotification.RepeatFormat != NotifyMeEnums.CalendarNotificationRepeatFormat.None;

            return new Notification()
            {
                NotificationTitle = CalendarNotification.Title,
                NotificationBody = CalendarNotification.Body,
                UserName = currentUser.UserName,
                CronJobString = _cronExpressionBuilder.RepeatableDateTimeToCronExpression(convertedFirstOccurance, CalendarNotification.RepeatFormat),
                Repeat = isRepeatable
            };
        }

        protected override void ResetState()
        {
            CalendarNotification = new();
        }

        public override async Task InitializeAsync()
        {
            await _clientDateTimeProvider.InitializeAsync();
            await base.InitializeAsync();
        }
    }
}
