﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using notifyme.infrastructure.Data;

namespace notifyme.infrastructure.Data.Migrations
{
    [DbContext(typeof(NotifyMeContext))]
    [Migration("20210609160631_cleanedUpDataImp")]
    partial class cleanedUpDataImp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.Notification", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CronJobString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NotificationBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NotificationTitle")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserName");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.SavedNotificationSubscription", b =>
                {
                    b.Property<string>("P256HKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EndPoint")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("P256HKey");

                    b.HasIndex("UserName");

                    b.ToTable("SavedNotificationSubscriptions");
                });

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.Notification", b =>
                {
                    b.HasOne("notifyme.shared.Models.DataStore_Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.SavedNotificationSubscription", b =>
                {
                    b.HasOne("notifyme.shared.Models.DataStore_Models.User", "User")
                        .WithMany("SavedNotificationSubscriptions")
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("notifyme.shared.Models.DataStore_Models.User", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("SavedNotificationSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
