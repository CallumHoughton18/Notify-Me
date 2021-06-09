using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.infrastructure.Data
{
    public class SavedNotificationSubscriptionRepository : BaseEfRepository<SavedNotificationSubscription>, ISavedNotificationSubscriptionRepository
    {
        public SavedNotificationSubscriptionRepository(IDbContextFactory<NotifyMeContext> contextFactory) : base(contextFactory)
        {
        }
    }
}
