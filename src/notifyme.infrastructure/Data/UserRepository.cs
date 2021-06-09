using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.infrastructure.Data
{
    public class UserRepository : BaseEfRepository<User>, IUserRepository
    {
        public UserRepository(IDbContextFactory<NotifyMeContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<User> GetByUserName(string username)
        {
            await using var ctx = _dbContextFactory.CreateDbContext();
            return await ctx.Set<User>()
                .Include(p => p.SavedNotificationSubscriptions)
                .Where(p => p.UserName == username).FirstOrDefaultAsync();
        }
    }
}
