using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using notifyme.infrastructure.Data;
using notifyme.infrastructure.Identity;
using notifyme.scheduler;
using notifyme.scheduler.Jobs;
using notifyme.scheduler.Services;
using notifyme.server.Areas.Identity;
using notifyme.server.Data;
using notifyme.server.Services;
using notifyme.shared.Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using notifyme.shared.ViewModels;
using Quartz;

namespace notifyme.server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices();
            services.AddRouting();
            services.AddDbContextFactory<NotifyMeContext>(opt => opt.UseSqlite($"Data Source=Data Stores/{NotifyMeContext.DB_NAME}.db"));
            services.AddScoped(p => p.GetRequiredService<IDbContextFactory<NotifyMeContext>>().CreateDbContext());
            services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<NotifyMeContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICronExpressionBuilder, CronExpressionBuilder>();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>();
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<CreateNewNotificationViewModel>();
            services.AddScoped<ManageUserDataViewModel>();
            services.AddScoped<RegisterNotificationSubscriptionViewModel>();
            services.AddTransient<IPushNotificationSubscriberService, PushNotificationSubscriberService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ISavedNotificationSubscriptionRepository, SavedNotificationSubscriptionRepository>();
            ConfigureJobScheduler(services);

            var vapidDetails = Configuration.GetSection("VAPID");
            services.AddScoped<VapidDetails>((_) => new VapidDetails(
                vapidDetails["publicKey"], 
                vapidDetails["privateKey"],
                vapidDetails["subject"]));

            services.AddScoped<IPushNotificationPusherService, PushNotificationPusherPusherService>();
        }

        private void ConfigureJobScheduler(IServiceCollection services)
        {
            services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzServer(q =>
            {
                q.WaitForJobsToComplete = true;
            });

            services.AddTransient<SendPushNotificationJob>();
            services.AddScoped<INotificationSchedulerInterface, NotificationScheduler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
