using System;
using System.Threading.Tasks;
using notifyme.scheduler.Jobs;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ServiceInterfaces;
using Quartz;
using Quartz.Impl;

namespace notifyme.scheduler
{
    public class NotificationScheduler : INotificationSchedulerInterface
    {
        ISchedulerFactory schedulerFactory;
        IScheduler scheduler;

        public NotificationScheduler(ISchedulerFactory schedulerFactory)
        {
            this.schedulerFactory = schedulerFactory;
        }

        public async Task InitializeAsync()
        {
            scheduler = await schedulerFactory.GetScheduler();
        }

        public async Task ScheduleNotificationAsync(Notification notification)
        {
            try
            {
                var dateTimeNow = DateTime.Now;

                IJobDetail job = JobBuilder.Create<SendPushNotificationJob>()
                    .WithIdentity(notification.NotificationId.ToString(), "test")
                    .Build();

                // ITrigger trigger = TriggerBuilder.Create()
                //     .WithIdentity($"{notification.NotificationId}-trigger", "test")
                //     .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(dateTimeNow.Hour, dateTimeNow.Minute + 1))
                //     .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{notification.NotificationId}-trigger", "test")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .WithRepeatCount(1))                    
                    .Build(); 

                await this.scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                var exception = e.ToString();
                throw;
            }
        }
    }
}