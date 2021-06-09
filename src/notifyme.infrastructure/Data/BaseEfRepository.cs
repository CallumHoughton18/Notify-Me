using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.infrastructure.Data
{
    public class BaseEfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly IDbContextFactory<NotifyMeContext> _dbContextFactory;
        private readonly NotifyMeContext context;

        public BaseEfRepository(IDbContextFactory<NotifyMeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            context = _dbContextFactory.CreateDbContext();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            await ctx.Set<T>().AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<T> AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var hasEntity = await ctx.Set<T>().ContainsAsync(entity, cancellationToken: cancellationToken);
            if (hasEntity) await UpdateAsync(entity, cancellationToken);
            else await AddAsync(entity, cancellationToken);

            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            ctx.Set<T>().Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            var keyValues = new object[] { id };
            return await ctx.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            return await ctx.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            ctx.Entry(entity).State = EntityState.Modified;
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
