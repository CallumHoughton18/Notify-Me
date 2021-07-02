using System;
using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using Quartz;

namespace notifyme.scheduler.Jobs
{
    public class SendPushNotificationJob : IJob
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ISavedNotificationSubscriptionRepository _subscriptionRepository;
        private readonly INotificationSchedulerInterface _notificationSchedulerInterface;
        private readonly IPushNotificationPusherService _pushNotificationPusherService;
        private readonly VapidDetails _vapidDetails;


        public SendPushNotificationJob(INotificationRepository notificationRepository,
            ISavedNotificationSubscriptionRepository subscriptionRepository,
            INotificationSchedulerInterface notificationSchedulerInterface,
            IPushNotificationPusherService pushNotificationPusherService, VapidDetails vapidDetails)
        {
            _notificationRepository = notificationRepository;
            _subscriptionRepository = subscriptionRepository;
            _notificationSchedulerInterface = notificationSchedulerInterface;
            _pushNotificationPusherService = pushNotificationPusherService;
            _vapidDetails = vapidDetails;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(context.JobDetail.Key.Name);
            if (Guid.TryParse(context.JobDetail.Key.Name, out var notificationGuid))
            {
                var notification = await _notificationRepository.GetByNotificationId(notificationGuid);
                var savedSubscriptions = await _subscriptionRepository.GetByUserName(notification.UserName);
                foreach (var sub in savedSubscriptions)
                {
                    _pushNotificationPusherService.SendPushNotification(sub, notification, _vapidDetails);
                }

                if (!notification.Repeat)
                {
                    await _notificationSchedulerInterface.DeScheduleNotificationAsync(notification);
                    await _notificationRepository.DeleteAsync(notification);
                }
            }
        }
    }
}