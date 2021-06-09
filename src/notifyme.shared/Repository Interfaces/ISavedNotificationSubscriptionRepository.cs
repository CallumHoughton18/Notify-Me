using System;
using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.RepositoryInterfaces
{
    public interface ISavedNotificationSubscriptionRepository : IAsyncRepository<SavedNotificationSubscription>
    {
    }
}
