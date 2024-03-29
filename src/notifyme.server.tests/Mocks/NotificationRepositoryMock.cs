using System;
using System.Threading;
using Moq;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Repository_Interfaces;

namespace notifyme.server.tests.Mocks
{
    public class NotificationRepositoryMock : Mock<INotificationRepository>
    {
        public NotificationRepositoryMock MockAddOrUpdate(Action<Notification> callback)
        {
            Setup(x =>
                    x.AddOrUpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .Callback<Notification, CancellationToken>((x, _) => callback(x));
            return this;
        }
    }
}