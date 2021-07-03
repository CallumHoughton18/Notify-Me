using System.Threading.Tasks;
using Moq;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using notifyme.shared.ViewModels;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests
{
    public class RegisterNotificationSubscriptionViewModelTests
    {
        [Fact]
        public async Task Should_Register_Notification()
        {
            var pushSubMock = new Mock<IPushNotificationSubscriberService>(MockBehavior.Loose);
            var savedNotifSubMock = new Mock<ISavedNotificationSubscriptionRepository>(MockBehavior.Loose);
            var authServiceMock = new Mock<IAuthService>(MockBehavior.Loose);
            
            var sut = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifSubMock.Object,
                authServiceMock.Object);

            await sut.InitializeAsync();
            //sut.SetIsDeviceRegistered();
        }
    }
}