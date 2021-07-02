using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using notifyme.shared.Helpers;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;

namespace notifyme.shared.ViewModels
{
    public class ManageUserDataViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ISavedNotificationSubscriptionRepository _subRepo;
        private readonly INotificationRepository _notifRepo;
        private readonly IPushNotificationSubscriberService _pushNotificationSubscriberService;
        private readonly INotificationSchedulerInterface _notificationSchedulerInterface;
        private User _currentUser;

        public ManageUserDataViewModel(
            IAuthService authService, 
            ISavedNotificationSubscriptionRepository subRepo,
            INotificationRepository notifRepo, 
            IPushNotificationSubscriberService pushNotificationSubscriberService,
            INotificationSchedulerInterface notificationSchedulerInterface)
        {
            _authService = authService;
            _subRepo = subRepo;
            _notifRepo = notifRepo;
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _notificationSchedulerInterface = notificationSchedulerInterface;
        }

        private RangeObservableCollection<SavedNotificationSubscription> _subscriptions = new();

        public RangeObservableCollection<SavedNotificationSubscription> Subscriptions
        {
            get => _subscriptions;
            set => SetValue(ref _subscriptions, value);
        }
        
        private SavedNotificationSubscription _selectedSubscription;

        public SavedNotificationSubscription SelectedSubscription
        {
            get => _selectedSubscription;
            set => SetValue(ref _selectedSubscription, value);
        }

        private RangeObservableCollection<Notification> _notifications = new();

        public RangeObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set => SetValue(ref _notifications, value);
        }

        private Notification _selectedNotification;

        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set => SetValue(ref _selectedNotification, value);
        }

        public override async Task InitializeAsync()
        {
            _currentUser =  await _authService.GetCurrentUserAsync();
            var notifications = await _notifRepo.GetByUserAsync(_currentUser.UserName);
            Notifications.Clear();
            Notifications.AddRange(notifications.ToList());

            var subscriptions = await _subRepo.GetByUserName(_currentUser.UserName);
            Subscriptions.Clear();
            Subscriptions.AddRange(subscriptions.ToList());

            await _pushNotificationSubscriberService.Initialize();
            
            await base.InitializeAsync();
        }

        public void SaveSelectedNotification()
        {
            IsLoading.SetNewValues(true, "Saving Notification Changes...");
            _notifRepo.AddOrUpdateAsync(SelectedNotification);
            IsLoading.SetNewValues(false);
        }
        
        public async Task DeleteSelectedNotification()
        {
            Notifications.Remove(SelectedNotification);
            await _notificationSchedulerInterface.DeScheduleNotificationAsync(SelectedNotification);
            await _notifRepo.DeleteAsync(SelectedNotification);
            SelectedNotification = null;
        }

        public void SaveSelectedSubscription()
        {
            IsLoading.SetNewValues(true, "Saving Subscription Changes...");
            _subRepo.AddOrUpdateAsync(SelectedSubscription);
            IsLoading.SetNewValues(false);
        }

        public async Task DeleteSelectedSubscription()
        {
            var unsubscribedBrowserFromNotifications =
                await _pushNotificationSubscriberService.UnsubscribeFromNotifications();
            if (unsubscribedBrowserFromNotifications)
            {
                Subscriptions.Remove(SelectedSubscription);
                await _subRepo.DeleteAsync(SelectedSubscription);
            }
            
            SelectedSubscription = null;
        }
    }
}