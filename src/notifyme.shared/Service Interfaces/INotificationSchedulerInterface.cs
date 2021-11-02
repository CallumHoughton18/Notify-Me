using System.Threading.Tasks;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.Service_Interfaces
{
    public interface INotificationSchedulerInterface
    {
        Task InitializeAsync();
        Task ScheduleNotificationAsync(Notification notification);
        Task DeScheduleNotificationAsync(Notification notification);
    }
}
