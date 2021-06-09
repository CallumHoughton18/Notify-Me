using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using notifyme.shared.Models;
using notifyme.shared.ServiceInterfaces;

namespace notifyme.server.Services
{
    public class PushNotificationSubscriberService : IPushNotificationSubscriberService
    {
        private readonly IJSRuntime _jsRunTime;
        private readonly VapidDetails _details;
        private IJSObjectReference _module;

        public PushNotificationSubscriberService(IJSRuntime jSRuntime, VapidDetails details)
        {
            _jsRunTime = jSRuntime;
            _details = details;
        }

        public async Task Initialize()
        {
            _module = await _jsRunTime.InvokeAsync<IJSObjectReference>("import", "./js/PushNotifications.js");
        }

        public async Task<bool> CheckAndRequestNotificationPermission()
        {
            return await _module.InvokeAsync<bool>("checkAndRequestNotificationPermission");
        }

        public async Task<NotificationSubscription> GetCurrentUserAndDeviceSubscription()
        {
            var hasPermission = await CheckAndRequestNotificationPermission();
            if (hasPermission)
            {
                var subscription = await _module.InvokeAsync<NotificationSubscription>("getCurrentSubscriptionDetails", _details.PublicKey );
                return subscription;
            }

            return null;
        }

        public async Task RegisterSubscription()
        {
            var hasPermission = await CheckAndRequestNotificationPermission();
            if (hasPermission)
            {
                await _module.InvokeVoidAsync("registerServiceWorker", _details.PublicKey);
            }
        }
    }
}
