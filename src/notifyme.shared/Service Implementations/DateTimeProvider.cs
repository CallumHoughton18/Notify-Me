using System;
using notifyme.shared.ServiceInterfaces;

namespace notifyme.shared.Service_Implementations
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}