using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.Repository_Interfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification>
    {
        Task<Notification> GetByNotificationId(Guid guid);
        Task<IList<Notification>> GetByUserAsync(string userName);
    }
}
