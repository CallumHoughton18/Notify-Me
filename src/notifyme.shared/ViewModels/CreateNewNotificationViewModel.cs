using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using System.Linq;
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
        
        [Required]
        public string NotificationBody { get; set; }
        [Required]
        public string NotificationTitle { get; set; }

        public async Task SetAndSaveNotificationSubscription()
        {
             await _pushNotificationSubscriberService.RegisterSubscription();
             var currentSub = await _pushNotificationSubscriberService.GetCurrentUserAndDeviceSubscription();
             if (currentSub is null) return;
             
             var newNotificationSubscription = new SavedNotificationSubscription()
             {
                 UserName = _currentUser.UserName,
                 AuthKey = currentSub.AuthKey,
                 EndPoint = currentSub.EndPoint,
                 P256HKey = currentSub.P256hKey,
             };
             
             SavedNotificationSubscription = await _savedNotificationSubscriptionRepository.AddOrUpdateAsync(newNotificationSubscription);
        }

        public async Task SaveNotification()
        {
            Notification newNotification = new Notification()
            {
                NotificationTitle = NotificationTitle,
                NotificationBody = NotificationBody,
                UserName = _currentUser.UserName,
                CronJobString = ""
            };
            Console.WriteLine("Saving notification...");
            await _notificationRepository.AddOrUpdateAsync(newNotification);
            await _notificationScheduler.ScheduleNotificationAsync(newNotification);
        }
        
        public override async Task InitializeAsync()
        {
            await _pushNotificationSubscriberService.Initialize();
            await _notificationScheduler.InitializeAsync();
            await _userRepository.AddOrUpdateAsync(new() { UserName = "Admin" });
            var users = await _userRepository.ListAllAsync();
            _currentUser = users[0];
            var savedSubs = await _savedNotificationSubscriptionRepository.ListAllAsync();
            
            SavedNotificationSubscription = savedSubs.First();
            await base.InitializeAsync();
        }
    }
}
