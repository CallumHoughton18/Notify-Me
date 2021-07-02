using System;
using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.ServiceInterfaces
{
    public interface INotificationSchedulerInterface
    {
        Task InitializeAsync();
        Task ScheduleNotificationAsync(Notification notification);
        Task DeScheduleNotificationAsync(Notification notification);
    }
}
