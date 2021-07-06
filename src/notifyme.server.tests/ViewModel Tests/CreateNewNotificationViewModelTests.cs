using System;
using System.Threading.Tasks;
using notifyme.scheduler.Services;
using notifyme.server.tests.Mocks;
using notifyme.shared;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ViewModels;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests
{
    public class CreateNewNotificationViewModelTests
    {
        private readonly NotificationSubscription _notificationSubscriptionStub = new()
            {EndPoint = "testEndPoint", AuthKey = "testAuthKey", P256hKey = "testKey"};

        private readonly string MockUserName = "Admin";

        private (PushNotificationSubscriberServiceMock, NotificationSchedulerMock, NotificationRepositoryMock,
            AuthServiceMock) CreateSUTDependencies(Action<Notification> savedNotificationCallback)
        {
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (_notificationSubscriptionStub));
            var notificationSchedularMock = new NotificationSchedulerMock();
            var savedNotifRepoMock =
                new NotificationRepositoryMock().MockAddOrUpdate(savedNotificationCallback);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));

            return (pushSubMock, notificationSchedularMock, savedNotifRepoMock, authServiceMock);
        }

        [Fact]
        public async Task Should_Create_New_Notification()
        {
            Notification savedNotification = null;
            
            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock) =
                CreateSUTDependencies(x => savedNotification = x);
            
            var sut = new CreateNewNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object);

            sut.QuickNotification.Title = "Test Notification";
            sut.QuickNotification.Body = "Test Body";
            sut.QuickNotification.RequestedTime = 5;
            sut.QuickNotification.TimeFormat = NotifyMeEnums.QuickNotificationTimeFormat.Minutes;

            await sut.SaveNotification();
            
            Assert.True(savedNotification.NotificationTitle == "Test Notification");
            Assert.True(savedNotification.NotificationBody == "Test Body");
            Assert.False(savedNotification.Repeat);
        }
    }
}