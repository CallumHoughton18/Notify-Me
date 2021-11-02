using System;

namespace notifyme.shared.Service_Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}