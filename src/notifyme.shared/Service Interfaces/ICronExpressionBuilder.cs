using System;
using static notifyme.shared.NotifyMeEnums;

namespace notifyme.shared.Service_Interfaces
{
    public interface ICronExpressionBuilder
    {
        string DateTimeToCronExpression(DateTime dateTime);

        string RepeatableDateTimeToCronExpression(DateTime dateTime, CalendarNotificationRepeatFormat repeatFormat);
    }
}