using System;
using System.Threading.Tasks;
using notifyme.scheduler.Jobs;
using notifyme.scheduler.Services;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ServiceInterfaces;
using Quartz;

namespace notifyme.scheduler
{
    public class NotificationScheduler : INotificationSchedulerInterface
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public NotificationScheduler(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task InitializeAsync()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
        }

        public async Task ScheduleNotificationAsync(Notification notification)
        {
            var job = JobBuilder.Create<SendPushNotificationJob>()
                .WithIdentity(notification.NotificationId.ToString(), "test")
                .Build();

            var cronExpression = new CronExpression(notification.CronJobString);
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{notification.NotificationId}-trigger", "test")
                .WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression))
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task DeScheduleNotificationAsync(Notification notification)
        {
            var jobKey = new JobKey(notification.NotificationId.ToString(), "test");
            await _scheduler.DeleteJob(jobKey);
        }
    }
}