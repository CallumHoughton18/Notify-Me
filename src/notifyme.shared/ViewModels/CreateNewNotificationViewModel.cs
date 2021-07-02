using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using System.Linq;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.ViewModels
{
    public class CreateNewNotificationViewModel : BaseViewModel
    {
        private readonly IPushNotificationSubscriberService _pushNotificationSubscriberService;
        private readonly INotificationSchedulerInterface _notificationScheduler;
        private readonly ICronExpressionBuilder _cronExpressionBuilder;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthService _authService;
        
        public CreateNewNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService, 
            INotificationSchedulerInterface notificationScheduler, 
            ICronExpressionBuilder cronExpressionBuilder,
            INotificationRepository notificationRepository, 
            IAuthService authService)
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _notificationScheduler = notificationScheduler;
            _cronExpressionBuilder = cronExpressionBuilder;
            _notificationRepository = notificationRepository;
            _authService = authService;
        }

        private QuickNotification _quickNotification = new();
        public QuickNotification QuickNotification
        {
            get => _quickNotification;
            set => SetValue(ref _quickNotification, value);
        }
        
        public async Task SaveNotification()
        {
            IsLoading.SetNewValues(true, "Saving Notification...");
            var currentUser = await _authService.GetCurrentUserAsync();
            var currentDateTime = DateTime.Now;
            var shouldFireAt = QuickNotification.TimeFormat switch  
            {
                NotifyMeEnums.QuickNotificationTimeFormat.Minutes => currentDateTime.AddMinutes(QuickNotification.RequestedTime),
                NotifyMeEnums.QuickNotificationTimeFormat.Hours => currentDateTime.AddHours(QuickNotification.RequestedTime),
                NotifyMeEnums.QuickNotificationTimeFormat.Days => currentDateTime.AddDays(QuickNotification.RequestedTime),
                _ => throw new ArgumentOutOfRangeException()
            }; 
            
            var newNotification = new Notification()
            {
                NotificationTitle = QuickNotification.Title,
                NotificationBody = QuickNotification.Body,
                UserName = currentUser.UserName,
                CronJobString = _cronExpressionBuilder.DateTimeToCronExpression(shouldFireAt),
                Repeat = false
            };
            
            await _notificationRepository.AddOrUpdateAsync(newNotification);
            await _notificationScheduler.ScheduleNotificationAsync(newNotification);
            IsLoading.SetNewValues(false);
        }
        
        public override async Task InitializeAsync()
        {
            await _pushNotificationSubscriberService.Initialize();
            await _notificationScheduler.InitializeAsync();
            await base.InitializeAsync();
        }
    }
}
