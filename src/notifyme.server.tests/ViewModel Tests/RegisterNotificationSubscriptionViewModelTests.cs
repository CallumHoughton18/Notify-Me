using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using notifyme.server.tests.Mocks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ViewModels;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests
{
    public class RegisterNotificationSubscriptionViewModelTests
    {
        private readonly NotificationSubscription _notificationSubscriptionStub = new()
            {EndPoint = "testEndPoint", AuthKey = "testAuthKey", P256HKey = "testKey"};

        private readonly string MockUserName = "Admin";

        private (PushNotificationSubscriberServiceMock, SavedNotificationSubscriptionRepositoryMock,
            AuthServiceMock) CreateDependencyMocks(Action<SavedNotificationSubscription> savedSubscriptionCallback)
        {
            var pushSubMock =
                new PushNotificationSubscriberServiceMock().MockGetCurrentUserAndDeviceSubscription(
                    (_notificationSubscriptionStub));
            var savedNotifRepoMock =
                new SavedNotificationSubscriptionRepositoryMock().MockAddOrUpdate(savedSubscriptionCallback);
            var authServiceMock = new AuthServiceMock().MockGetCurrentUser(new User(MockUserName));

            return (pushSubMock, savedNotifRepoMock, authServiceMock);
        }

        [Fact]
        public async Task Should_Save_Device_Notification_Subscription()
        {
            SavedNotificationSubscription savedSubscription = null;
            var (pushSubMock, savedNotifRepoMock, authServiceMock) =
                CreateDependencyMocks(x => savedSubscription = x);

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
            var (pushSubMock, savedNotifRepoMock, authServiceMock) =
                CreateDependencyMocks(x => savedSubscription = x);
                pushSubMock.MockGetCurrentUserAndDeviceSubscription(null);

                var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.SaveNotificationSubscription("Test Subscription");

            savedNotifRepoMock.Verify(x =>
                    x.AddOrUpdateAsync(It.IsAny<SavedNotificationSubscription>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Assert.Null(savedSubscription);
            Assert.False(sut.IsDeviceRegistered);
        }

        [Fact]
        public async Task Should_Set_To_Registered_On_Initialization()
        {
            var (pushSubMock, savedNotifRepoMock, authServiceMock) =
                CreateDependencyMocks(_ => { });

            savedNotifRepoMock.MockGetByUserName(MockUserName, new List<SavedNotificationSubscription>()
            {
                new()
                {
                    AuthKey = "TestKey",
                    DeviceName = "TestDevice",
                    EndPoint = "TestEndPoint",
                    P256HKey = _notificationSubscriptionStub.P256HKey,
                    UserName = MockUserName
                }
            });
            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.InitializeAsync();
            
            Assert.True(sut.IsDeviceRegistered);
        }
        
        [Fact]
        public async Task Should_Set_To_Not_Registered_On_Initialization_With_Other_Subscriptions()
        {
            var (pushSubMock, savedNotifRepoMock, authServiceMock) =
                CreateDependencyMocks(_ => { });

            savedNotifRepoMock.MockGetByUserName(MockUserName, new List<SavedNotificationSubscription>()
            {
                new()
                {
                    AuthKey = "TestKey",
                    DeviceName = "TestDevice",
                    EndPoint = "TestEndPoint",
                    P256HKey = "AnotherP256HKey",
                    UserName = MockUserName
                }
            });
            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.InitializeAsync();
            
            Assert.False(sut.IsDeviceRegistered);
        }
        
        [Fact]
        public async Task Should_Set_To_Not_Registered_On_Initialization_With_Empty_List()
        {
            var (pushSubMock, savedNotifRepoMock, authServiceMock) =
                CreateDependencyMocks(_ => { });

            savedNotifRepoMock.MockGetByUserName(MockUserName, new List<SavedNotificationSubscription>());
            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifRepoMock.Object,
                authServiceMock.Object);

            await sut.InitializeAsync();
            
            Assert.False(sut.IsDeviceRegistered);
        }
    }
}