using System;
using System.Threading;
using Moq;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;

namespace notifyme.server.tests.Mocks
{
    public class SavedNotificationSubscriptionRepositoryMock : Mock<ISavedNotificationSubscriptionRepository>
    {
        public SavedNotificationSubscriptionRepositoryMock MockAddOrUpdate(Action<SavedNotificationSubscription> callback)
        {
            Setup(x =>
                    x.AddOrUpdateAsync(It.IsAny<SavedNotificationSubscription>(), It.IsAny<CancellationToken>()))
                .Callback<SavedNotificationSubscription, CancellationToken>((x, y) => callback(x));
            return this;
        }
    }
}