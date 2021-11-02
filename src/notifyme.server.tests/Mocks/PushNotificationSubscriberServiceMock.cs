using Moq;
using notifyme.shared.Models;
using notifyme.shared.Service_Interfaces;

namespace notifyme.server.tests.Mocks
{
    public class PushNotificationSubscriberServiceMock : Mock<IPushNotificationSubscriberService>
    {
        public PushNotificationSubscriberServiceMock MockGetCurrentUserAndDeviceSubscription(
            NotificationSubscription subscription)
        {
            Setup(x => x.GetCurrentUserAndDeviceSubscription())
                .ReturnsAsync(() => subscription);
            return this;
        }
    }
}