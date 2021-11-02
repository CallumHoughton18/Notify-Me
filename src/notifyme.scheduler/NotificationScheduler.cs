using System.Threading.Tasks;
using notifyme.scheduler.Jobs;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Service_Interfaces;
using Quartz;

namespace notifyme.scheduler
{
    public class NotificationScheduler : INotificationSchedulerInterface
    {
        public const string Jobgroupname = "jobs";
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
                .WithIdentity(notification.NotificationId.ToString(), Jobgroupname)
                .Build();

            var cronExpression = new CronExpression(notification.CronJobString);
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{notification.NotificationId}-trigger", Jobgroupname)
                .WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression))
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task DeScheduleNotificationAsync(Notification notification)
        {
            var jobKey = new JobKey(notification.NotificationId.ToString(), Jobgroupname);
            await _scheduler.DeleteJob(jobKey);
        }
    }
}