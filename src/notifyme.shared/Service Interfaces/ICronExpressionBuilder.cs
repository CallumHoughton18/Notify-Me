using System;
using static notifyme.shared.NotifyMeEnums;

namespace notifyme.shared.ServiceInterfaces
{
    public interface ICronExpressionBuilder
    {
        string DateTimeToCronExpression(DateTime dateTime);

        string RepeatableDateTimeToCronExpression(DateTime dateTime, CalendarNotificationRepeatFormat repeatFormat);
    }
}