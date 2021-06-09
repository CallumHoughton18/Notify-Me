using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.infrastructure.Data
{
    public class NotificationRepository : BaseEfRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbContextFactory<NotifyMeContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<Notification> GetByNotificationId(Guid guid)
        {
            await using var ctx = _dbContextFactory.CreateDbContext();
            return await ctx.Set<Notification>().FindAsync(guid);
        }
    }
}
