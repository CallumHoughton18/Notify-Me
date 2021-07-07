using System;

namespace notifyme.shared.ServiceInterfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}