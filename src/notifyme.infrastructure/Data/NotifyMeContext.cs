using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using notifyme.infrastructure.Identity;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.infrastructure.Data
{
    public class NotifyMeContext : IdentityDbContext<AppUser>
    {
        public const string DB_NAME = "NotifyMeDB";

        public NotifyMeContext(DbContextOptions<NotifyMeContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SavedNotificationSubscription> SavedNotificationSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
