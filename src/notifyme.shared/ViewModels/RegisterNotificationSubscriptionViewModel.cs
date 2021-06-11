using System.Threading.Tasks;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;

namespace notifyme.shared.ViewModels
{
    public class RegisterNotificationSubscriptionViewModel : BaseViewModel
    {
        private readonly IPushNotificationSubscriberService _pushNotificationSubscriberService;
        private readonly ISavedNotificationSubscriptionRepository _subscriptionRepository;

        public RegisterNotificationSubscriptionViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService, 
            ISavedNotificationSubscriptionRepository subscriptionRepository
            )
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _subscriptionRepository = subscriptionRepository;
        }

        private bool _isDeviceRegistered = false;
        public bool IsDeviceRegistered
        {
            get => _isDeviceRegistered;
            set => SetValue(ref _isDeviceRegistered, value);
        }

        public override async Task InitializeAsync()
        {
            await _pushNotificationSubscriberService.Initialize();
            await SetAndSaveNotificationSubscription();
            await base.InitializeAsync();
        }

        public async Task SetAndSaveNotificationSubscription()
        {
            await _pushNotificationSubscriberService.RegisterSubscription();
            var currentSub = await _pushNotificationSubscriberService.GetCurrentUserAndDeviceSubscription();
            if (currentSub is null)
            {
                IsDeviceRegistered = false;
                return;
            }
             
            var newNotificationSubscription = new SavedNotificationSubscription()
            {
                UserName = "Admin",
                AuthKey = currentSub.AuthKey,
                EndPoint = currentSub.EndPoint,
                P256HKey = currentSub.P256hKey,
            };
            
            var sub = await _subscriptionRepository.AddOrUpdateAsync(newNotificationSubscription);
            IsDeviceRegistered = sub != null;
        }
    }
}