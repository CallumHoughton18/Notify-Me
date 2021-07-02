using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.Pages;
using notifyme.server.tests.Extensions;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.RepositoryInterfaces;
using notifyme.shared.ServiceInterfaces;
using notifyme.shared.ViewModels;
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

            var vm = new CreateNewNotificationViewModel(pushSubMock.Object,
                schedulerMock.Object,new CronExpressionBuilder(), notifRepoMock.Object, authServiceMock.Object);

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