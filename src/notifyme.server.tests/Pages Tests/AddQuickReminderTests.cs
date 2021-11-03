using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.Pages;
using notifyme.server.tests.Extensions;
using notifyme.shared.Repository_Interfaces;
using notifyme.shared.Service_Interfaces;
using notifyme.shared.ViewModels;
using notifyme.shared.ViewModels.Create_Notification;
using Xunit;

namespace notifyme.server.tests.Pages_Tests
{
    public class AddQuickReminderTests : TestContext
    {
        [Fact]
        public void Should_Render_Page_With_Form()
        {
            var pushSubMock = new Mock<IPushNotificationSubscriberService>(MockBehavior.Loose);
            var schedulerMock = new Mock<INotificationSchedulerInterface>(MockBehavior.Loose);
            var notifRepoMock = new Mock<INotificationRepository>(MockBehavior.Loose);
            var savedNotifSubMock = new Mock<ISavedNotificationSubscriptionRepository>(MockBehavior.Loose);
            var authServiceMock = new Mock<IAuthService>(MockBehavior.Loose);
            var dateTimeMock = new Mock<IServerDateTimeProvider>(MockBehavior.Loose);

            var vm = new CreateQuickNotificationViewModel(pushSubMock.Object,
                schedulerMock.Object, new CronExpressionBuilder(), notifRepoMock.Object, authServiceMock.Object,
                dateTimeMock.Object);

            var subVm = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifSubMock.Object,
                authServiceMock.Object);

            this.AddTestServices();
            Services.AddScoped(_ => vm);
            Services.AddScoped(_ => subVm);

            var cut = RenderComponent<AddQuickReminder>();

            var formEle = cut.Find("form");
            Assert.NotNull(formEle);
        }
    }
}