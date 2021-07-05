using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using notifyme.server.tests.Mocks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using notifyme.shared.ViewModels;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests
{
    public class RegisterNotificationSubscriptionViewModelTests
    {
        private readonly NotificationSubscription _notificationSubscriptionStub = new()
            {EndPoint = "testEndPoint", AuthKey = "testAuthKey", P256hKey = "testKey"};

        private readonly string MockUserName = "Admin";

        [Fact]
        public async Task Should_Save_Device_Notification_Subscription()
        {
            SavedNotificationSubscription savedSubscription = null;
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (_notificationSubscriptionStub));
            var savedNotifRepoMock =
                new SavedNotificationSubscriptionRepositoryMock().MockAddOrUpdate(x => savedSubscription = x);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));

            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.SaveNotificationSubscription("Test Subscription");

            Assert.NotNull(savedSubscription);
            Assert.True(sut.IsDeviceRegistered);
        }

        [Fact]
        public async Task Should_Not_Save_Device_Notification_Subscription()
        {
            SavedNotificationSubscription savedSubscription = null;
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (null));
            var savedNotifRepoMock =
                new SavedNotificationSubscriptionRepositoryMock().MockAddOrUpdate(x => savedSubscription = x);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));

            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.SaveNotificationSubscription("Test Subscription");

            savedNotifRepoMock.Verify(x => 
                x.AddOrUpdateAsync(It.IsAny<SavedNotificationSubscription>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.Null(savedSubscription);
            Assert.False(sut.IsDeviceRegistered);
        }
    }
}