using System;
using Moq;
using notifyme.server.tests.Mocks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Service_Interfaces;

namespace notifyme.server.tests.ViewModel_Tests.Create_Notification_Tests
{
    public abstract class BaseCreateNotificationTests
    {
       protected readonly NotificationSubscription _notificationSubscriptionStub = new()
        { EndPoint = "testEndPoint", AuthKey = "testAuthKey", P256HKey = "testKey" };

        protected const string MockUserName = "Admin";
        protected const string MockNotificationTitle = "NotificationTitle";
        protected const string MockNotificationBody = "Notificationbody";
        protected readonly DateTime _mockServerCurrentDateTime = new(2021, 1, 1, 12, 1, 1);

        protected (PushNotificationSubscriberServiceMock, NotificationSchedulerMock, NotificationRepositoryMock,
            AuthServiceMock, Mock<IServerDateTimeProvider>) CreateBaseSutDependencies(
                Action<Notification> savedNotificationCallback)
        {
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (_notificationSubscriptionStub));
            var notificationSchedularMock = new NotificationSchedulerMock();
            var savedNotifRepoMock =
                new NotificationRepositoryMock().MockAddOrUpdate(savedNotificationCallback);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));
            var mockServerDateTimeProvider = new Mock<IServerDateTimeProvider>();
            mockServerDateTimeProvider.SetupGet(x => x.Now).Returns(_mockServerCurrentDateTime);
            
            return (pushSubMock, notificationSchedularMock, savedNotifRepoMock, authServiceMock, mockServerDateTimeProvider);
        }
    }
}
