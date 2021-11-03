using System;

namespace notifyme.shared.Service_Interfaces
{
    public interface IServerDateTimeProvider
    {
        DateTime Now { get; }
        TimeZoneInfo CurrentTimeZone { get; }
    }
}