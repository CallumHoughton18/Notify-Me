using System.Collections.Generic;
using System.Threading.Tasks;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.Repository_Interfaces
{
    public interface ISavedNotificationSubscriptionRepository : IAsyncRepository<SavedNotificationSubscription>
    {
        public Task<IList<SavedNotificationSubscription>> GetByUserName(string username);
    }
}
