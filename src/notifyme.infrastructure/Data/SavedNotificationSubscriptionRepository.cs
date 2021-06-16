using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.infrastructure.Data
{
    public class SavedNotificationSubscriptionRepository : BaseEfRepository<SavedNotificationSubscription>,
        ISavedNotificationSubscriptionRepository
    {
        public SavedNotificationSubscriptionRepository(IDbContextFactory<NotifyMeContext> contextFactory) : base(
            contextFactory)
        {
        }

        public async Task<IList<SavedNotificationSubscription>> GetByUserName(string username)
        {
            await using var ctx = _dbContextFactory.CreateDbContext();
            return ctx.Set<SavedNotificationSubscription>().Where(x => x.UserName == username).ToList();
        }
    }
}