using System;
using Quartz;

namespace notifyme.scheduler.Converters
{
    public static class ToCronExpression
    {
        public static string FromDateTime(DateTime dateTime)
        {
            var expression =
                new CronExpression(
                    $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ?");

            return expression.CronExpressionString;
        }

        public static string DailyAtHourAndMinute(int hour, int minute)
        {
            DateBuilder.ValidateHour(hour);
            DateBuilder.ValidateMinute(minute);

            string cronExpression = $"0 {minute} {hour} ? * *";
            return cronExpression;
        }

        public static string WeeklyOnDayAndHourAndMinute(DayOfWeek dayOfWeek, int hour, int minute)
        {
            DateBuilder.ValidateHour(hour);
            DateBuilder.ValidateMinute(minute);

            string cronExpression = $"0 {minute} {hour} ? * {dayOfWeek.ToQuartzString()}";
            return cronExpression;
        }

        private static string ToQuartzString(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "SUN";
                case DayOfWeek.Monday:
                    return "MON";
                case DayOfWeek.Tuesday:
                    return "TUE";
                case DayOfWeek.Wednesday:
                    return "WED";
                case DayOfWeek.Thursday:
                    return "THU";
                case DayOfWeek.Friday:
                    return "FRI";
                case DayOfWeek.Saturday:
                    return "SAT";
                default:
                    return "";
            }
        }

        public static string MonthlyOnDayAndHourAndMinute(int dayOfMonth, int hour, int minute)
        {
            DateBuilder.ValidateDayOfMonth(dayOfMonth);
            DateBuilder.ValidateHour(hour);
            DateBuilder.ValidateMinute(minute);

            string cronExpression = $"0 {minute} {hour} {dayOfMonth} * ?";
            return cronExpression;
        }
    }
}