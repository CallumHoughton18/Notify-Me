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
        private readonly IPushNotificationPusherService _pushNotificationPusherService;
        private readonly VapidDetails _vapidDetails;
        private readonly IUserRepository _userRepository;


        public SendPushNotificationJob(INotificationRepository notificationRepository,IPushNotificationPusherService pushNotificationPusherService, VapidDetails vapidDetails, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _pushNotificationPusherService = pushNotificationPusherService;
            _vapidDetails = vapidDetails;
            _userRepository = userRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(context.JobDetail.Key.Name);
            if (Guid.TryParse(context.JobDetail.Key.Name, out var notificationGuid))
            {
                var notification = await _notificationRepository.GetByNotificationId(notificationGuid);
                var user = await _userRepository.GetByUserName(notification.UserName);
                foreach (var sub in user.SavedNotificationSubscriptions)
                {
                    _pushNotificationPusherService.SendPushNotification(sub, notification, _vapidDetails);
                }
                Console.WriteLine(notification.NotificationTitle);
            }
        }
    }
}
