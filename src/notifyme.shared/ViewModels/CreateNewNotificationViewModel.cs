using System;
using System.ComponentModel.DataAnnotations;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthService _authService;
        
        public CreateNewNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService, 
            INotificationSchedulerInterface notificationScheduler, 
            INotificationRepository notificationRepository, IAuthService authService)
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _notificationScheduler = notificationScheduler;
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
            
            var newNotification = new Notification()
            {
                NotificationTitle = QuickNotification.Title,
                NotificationBody = QuickNotification.Body,
                UserName = currentUser.UserName,
                CronJobString = ""
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
