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
            string cronExp = null;
            switch (repeatFormat)
            {
                case NotifyMeEnums.CalendarNotificationRepeatFormat.None:
                    cronExp = ToCronExpression.FromDateTime(dateTime);
                    break;
                case NotifyMeEnums.CalendarNotificationRepeatFormat.Weekly:
                    cronExp = ToCronExpression.WeeklyOnDayAndHourAndMinute(dateTime.DayOfWeek, dateTime.Hour, dateTime.Minute).ToString();
                    break;
                case NotifyMeEnums.CalendarNotificationRepeatFormat.Monthly:
                    cronExp = ToCronExpression.MonthlyOnDayAndHourAndMinute(dateTime.Month, dateTime.Hour, dateTime.Minute).ToString();
                    break;
                case NotifyMeEnums.CalendarNotificationRepeatFormat.Yearly:
                    cronExp = $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? *";
                    break;
                default:
                    break;
            }

            return cronExp;
        }
    }
}