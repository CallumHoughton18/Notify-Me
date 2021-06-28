using System.Linq;
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
        private readonly IAuthService _authService;

        public RegisterNotificationSubscriptionViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService, 
            ISavedNotificationSubscriptionRepository subscriptionRepository,
            IAuthService authService
            )
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _subscriptionRepository = subscriptionRepository;
            _authService = authService;
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
            await SetIsDeviceRegistered();
            await base.InitializeAsync();
        }

        private async Task<bool> HasSubscriptionEnabled()
        {
            await _pushNotificationSubscriberService.RegisterSubscription();
            var currentSub = await _pushNotificationSubscriberService.GetCurrentUserAndDeviceSubscription();
            return currentSub != null;
        }

        private async Task<bool> HasSubscriptionSaved(string userName)
        {
            var currentSub = await _pushNotificationSubscriberService.GetCurrentUserAndDeviceSubscription();
            if (currentSub is null) return false;
            var userSubs = await _subscriptionRepository.GetByUserName(userName);
            return userSubs.FirstOrDefault(x => x.P256HKey == currentSub.P256hKey) != null;
        }

        public async Task SetIsDeviceRegistered()
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            IsDeviceRegistered = await HasSubscriptionEnabled() &&
                                 await HasSubscriptionSaved(currentUser.UserName);
        }

        public async Task SaveNotificationSubscription(string friendlySubscriptionName)
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            await _pushNotificationSubscriberService.RegisterSubscription();
            var currentSub = await _pushNotificationSubscriberService.GetCurrentUserAndDeviceSubscription();

            var newNotificationSubscription = new SavedNotificationSubscription()
            {
                UserName = currentUser.UserName,
                AuthKey = currentSub.AuthKey,
                EndPoint = currentSub.EndPoint,
                P256HKey = currentSub.P256hKey,
                DeviceName =  friendlySubscriptionName
            };
            
            await _subscriptionRepository.AddOrUpdateAsync(newNotificationSubscription);
            IsDeviceRegistered = true;
        }
    }
}