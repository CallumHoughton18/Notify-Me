using System;
using notifyme.shared.Service_Interfaces;

namespace notifyme.shared.Service_Implementations
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}