using System;
using notifyme.shared.Service_Interfaces;

namespace notifyme.shared.Service_Implementations
{
    public class ServerServerDateTimeProvider : IServerDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public TimeZoneInfo CurrentTimeZone => TimeZoneInfo.Local;
    }
}