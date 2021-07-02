using System;
using notifyme.scheduler.Converters;
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
    }
}