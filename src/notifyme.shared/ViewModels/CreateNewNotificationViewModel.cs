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
        private readonly IUserRepository _userRepository;
        private readonly ISavedNotificationSubscriptionRepository _savedNotificationSubscriptionRepository;
        private readonly INotificationSchedulerInterface _notificationScheduler;
        private readonly INotificationRepository _notificationRepository;

        private User _currentUser;
        public CreateNewNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService, 
            IUserRepository userRepository,
            ISavedNotificationSubscriptionRepository savedNotificationSubscriptionRepository, 
            INotificationSchedulerInterface notificationScheduler, 
            INotificationRepository notificationRepository)
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _userRepository = userRepository;
            _savedNotificationSubscriptionRepository = savedNotificationSubscriptionRepository;
            _notificationScheduler = notificationScheduler;
            _notificationRepository = notificationRepository;
        }

        private SavedNotificationSubscription _savedNotificationSubscription;
        public SavedNotificationSubscription SavedNotificationSubscription
        {
            get => _savedNotificationSubscription;
            set => SetValue(ref _savedNotificationSubscription, value);
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
            Notification newNotification = new Notification()
            {
                NotificationTitle = QuickNotification.Title,
                NotificationBody = QuickNotification.Body,
                UserName = _currentUser.UserName,
                CronJobString = ""
            };
            Console.WriteLine("Saving notification...");
            await _notificationRepository.AddOrUpdateAsync(newNotification);
            await _notificationScheduler.ScheduleNotificationAsync(newNotification);
            IsLoading.SetNewValues(false);

        }
        
        public override async Task InitializeAsync()
        {
            await _pushNotificationSubscriberService.Initialize();
            await _notificationScheduler.InitializeAsync();
            await _userRepository.AddOrUpdateAsync(new() { UserName = "Admin" });
            var users = await _userRepository.ListAllAsync();
            _currentUser = users[0];
            var savedSubs = await _savedNotificationSubscriptionRepository.ListAllAsync();
            
            //SavedNotificationSubscription = savedSubs.FirstOrDefault();
            await base.InitializeAsync();
        }
    }
}
