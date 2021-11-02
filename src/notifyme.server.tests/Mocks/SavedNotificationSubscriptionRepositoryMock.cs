using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Repository_Interfaces;

namespace notifyme.server.tests.Mocks
{
    public class SavedNotificationSubscriptionRepositoryMock : Mock<ISavedNotificationSubscriptionRepository>
    {
        public SavedNotificationSubscriptionRepositoryMock MockAddOrUpdate(Action<SavedNotificationSubscription> callback)
        {
            Setup(x =>
                    x.AddOrUpdateAsync(It.IsAny<SavedNotificationSubscription>(), It.IsAny<CancellationToken>()))
                .Callback<SavedNotificationSubscription, CancellationToken>((x, _) => callback(x));
            return this;
        }

        public SavedNotificationSubscriptionRepositoryMock MockGetByUserName(string userName, List<SavedNotificationSubscription> returnedSubscriptions)
        {
            Setup(x => x.GetByUserName(userName)).ReturnsAsync(returnedSubscriptions);
            return this;
        }
    }
}