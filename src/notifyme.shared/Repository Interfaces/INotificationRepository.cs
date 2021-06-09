using System;
using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.RepositoryInterfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification>
    {
        Task<Notification> GetByNotificationId(Guid guid);
    }
}
