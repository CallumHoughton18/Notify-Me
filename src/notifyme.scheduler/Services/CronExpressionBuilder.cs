using System;
using notifyme.scheduler.Converters;
using notifyme.shared;
using notifyme.shared.ServiceInterfaces;
using Quartz;

namespace notifyme.scheduler.Services
{
    public class CronExpressionBuilder : ICronExpressionBuilder
    {

        public string DateTimeToCronExpression(DateTime dateTime)
        {
            return ToCronExpression.FromDateTime(dateTime);
        }

        public string RepeatableDateTimeToCronExpression(DateTime dateTime, NotifyMeEnums.CalendarNotificationRepeatFormat repeatFormat)
        {
            var cronExp = repeatFormat switch
            {
                NotifyMeEnums.CalendarNotificationRepeatFormat.None => ToCronExpression.FromDateTime(dateTime),
                NotifyMeEnums.CalendarNotificationRepeatFormat.Weekly => ToCronExpression.WeeklyOnDayAndHourAndMinute(
                    dateTime.DayOfWeek, dateTime.Hour, dateTime.Minute),
                NotifyMeEnums.CalendarNotificationRepeatFormat.Monthly => ToCronExpression.MonthlyOnDayAndHourAndMinute(
                    dateTime.Day, dateTime.Hour, dateTime.Minute),
                NotifyMeEnums.CalendarNotificationRepeatFormat.Yearly =>
                    $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? *",
                _ => null
            };
            return cronExp;
        }
    }
}