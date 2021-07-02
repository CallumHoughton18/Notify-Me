using System;

namespace notifyme.shared.ServiceInterfaces
{
    public interface ICronExpressionBuilder
    {
        string DateTimeToCronExpression(DateTime dateTime);
    }
}