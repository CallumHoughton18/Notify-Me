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
        private User _currentUser;

        public ManageUserDataViewModel(IAuthService authService, ISavedNotificationSubscriptionRepository subRepo,
            INotificationRepository notifRepo)
        {
            _authService = authService;
            _subRepo = subRepo;
            _notifRepo = notifRepo;
        }

        private RangeObservableCollection<SavedNotificationSubscription> _subscriptions = new();

        public RangeObservableCollection<SavedNotificationSubscription> Subscriptions
        {
            get => _subscriptions;
            set => SetValue(ref _subscriptions, value);
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
            set
            {
                if (value is not null) Console.WriteLine("Selected An Item");
                SetValue(ref _selectedNotification, value);
            }
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
            
            await base.InitializeAsync();
        }
    }
}