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
    public class CreateQuickNotificationViewModel : BaseCreateNotificationViewModel
    {
        public CreateQuickNotificationViewModel(
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

        private QuickNotification _quickNotification = new();
        public QuickNotification QuickNotification
        {
            get => _quickNotification;
            set => SetValue(ref _quickNotification, value);
        }

        protected override async Task<Notification> NotificationGenerator()
        {
            var isValid = ValidationHelpers.ValidateModel(QuickNotification);
            if (!isValid.ReturnBool) throw new ValidationException(isValid.ReturnString);

            var currentUser = await _authService.GetCurrentUserAsync();
            var currentDateTime = _dateTimeProvider.Now;
            var shouldFireAt = QuickNotification.TimeFormat switch
            {
                NotifyMeEnums.QuickNotificationTimeFormat.Minutes => currentDateTime.AddMinutes(QuickNotification.RequestedTime),
                NotifyMeEnums.QuickNotificationTimeFormat.Hours => currentDateTime.AddHours(QuickNotification.RequestedTime),
                NotifyMeEnums.QuickNotificationTimeFormat.Days => currentDateTime.AddDays(QuickNotification.RequestedTime),
                _ => throw new ArgumentOutOfRangeException()
            };

            return new Notification()
            {
                NotificationTitle = QuickNotification.Title,
                NotificationBody = QuickNotification.Body,
                UserName = currentUser.UserName,
                CronJobString = _cronExpressionBuilder.DateTimeToCronExpression(shouldFireAt),
                Repeat = false
            };
        }

        protected override void ResetState()
        {
            QuickNotification = new();
        }
    }
}
