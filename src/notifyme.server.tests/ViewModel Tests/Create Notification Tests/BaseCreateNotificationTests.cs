using System;
using System.Threading.Tasks;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.Data;
using notifyme.server.tests.Mocks;
using notifyme.shared;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ServiceInterfaces;
using Xunit;

namespace notifyme.server.tests.ViewModelTests.CreateNotificationTests
{
    public abstract class BaseCreateNotificationTests
    {
       protected readonly shared.Models.NotificationSubscription _notificationSubscriptionStub = new()
        { EndPoint = "testEndPoint", AuthKey = "testAuthKey", P256hKey = "testKey" };

        protected const string MockUserName = "Admin";
        protected const string MockNotificationTitle = "NotificationTitle";
        protected const string MockNotificationBody = "Notificationbody";
        protected readonly DateTime _mockCurrentDateTime = new(2021, 1, 1, 12, 1, 1);

        protected (PushNotificationSubscriberServiceMock, NotificationSchedulerMock, NotificationRepositoryMock,
            AuthServiceMock, Mock<IDateTimeProvider>) CreateSutDependencies(
                Action<Notification> savedNotificationCallback)
        {
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (_notificationSubscriptionStub));
            var notificationSchedularMock = new NotificationSchedulerMock();
            var savedNotifRepoMock =
                new NotificationRepositoryMock().MockAddOrUpdate(savedNotificationCallback);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.SetupGet(x => x.Now).Returns(_mockCurrentDateTime);

            return (pushSubMock, notificationSchedularMock, savedNotifRepoMock, authServiceMock, mockDateTimeProvider);
        }
    }
}
