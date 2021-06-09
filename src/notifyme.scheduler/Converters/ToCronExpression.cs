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
    }
}