using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.infrastructure.Data
{
    public class NotifyMeContext : DbContext
    {
        public const string DB_NAME = "NotifyMeDB";

        public NotifyMeContext(DbContextOptions<NotifyMeContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SavedNotificationSubscription> SavedNotificationSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Notification>()
                .HasOne(p => p.User)
                .WithMany(b => b.Notifications)
                .HasForeignKey(p => p.UserName);

            modelBuilder.Entity<SavedNotificationSubscription>()
                .HasOne(p => p.User)
                .WithMany(b => b.SavedNotificationSubscriptions)
                .HasForeignKey(p => p.UserName);
        }
    }
}
