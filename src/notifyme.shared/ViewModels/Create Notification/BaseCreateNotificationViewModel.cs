using System.Threading.Tasks;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Repository_Interfaces;
using notifyme.shared.Service_Interfaces;

namespace notifyme.shared.ViewModels.Create_Notification
{
    public abstract class BaseCreateNotificationViewModel : BaseViewModel
    {
        protected readonly IPushNotificationSubscriberService _pushNotificationSubscriberService;
        protected readonly INotificationSchedulerInterface _notificationScheduler;
        protected readonly ICronExpressionBuilder _cronExpressionBuilder;
        protected readonly INotificationRepository _notificationRepository;
        protected readonly IAuthService _authService;
        protected readonly IServerDateTimeProvider _serverServerDateTimeProvider;

        public BaseCreateNotificationViewModel(
            IPushNotificationSubscriberService pushNotificationSubscriberService,
            INotificationSchedulerInterface notificationScheduler,
            ICronExpressionBuilder cronExpressionBuilder,
            INotificationRepository notificationRepository,
            IAuthService authService,
            IServerDateTimeProvider serverServerDateTimeProvider)
        {
            _pushNotificationSubscriberService = pushNotificationSubscriberService;
            _notificationScheduler = notificationScheduler;
            _cronExpressionBuilder = cronExpressionBuilder;
            _notificationRepository = notificationRepository;
            _authService = authService;
            _serverServerDateTimeProvider = serverServerDateTimeProvider;
        }

        protected abstract Task<Notification> NotificationGenerator();
        protected abstract void ResetState();

        public async Task<bool> SaveNotification()
        {
            var notificationToSave = await NotificationGenerator();
            var notif = await _notificationRepository.AddOrUpdateAsync(notificationToSave);
            if (notif == null) return false;

            await _notificationScheduler.ScheduleNotificationAsync(notificationToSave);
            ResetState();
            return true;
        }

        public override async Task InitializeAsync()
        {
            await _pushNotificationSubscriberService.Initialize();
            await _notificationScheduler.InitializeAsync();
            await base.InitializeAsync();
        }
    }
}