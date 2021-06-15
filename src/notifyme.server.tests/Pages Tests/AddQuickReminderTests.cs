using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
              var userRepoMock = new Mock<IUserRepository>(MockBehavior.Loose);
              userRepoMock.Setup(x => x.ListAllAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult<IReadOnlyList<User>>(new List<User>(){new User(){UserName = "Admin"}}));
              var savedNotifsMock = new Mock<ISavedNotificationSubscriptionRepository>(MockBehavior.Loose);
              var schedulerMock = new Mock<INotificationSchedulerInterface>(MockBehavior.Loose);
              var notifRepo = new Mock<INotificationRepository>(MockBehavior.Loose);
             
              var vm = new CreateNewNotificationViewModel(pushSubMock.Object, userRepoMock.Object, savedNotifsMock.Object,
                  schedulerMock.Object, notifRepo.Object);
             
              var subVm = new RegisterNotificationSubscriptionViewModel(pushSubMock.Object, savedNotifsMock.Object);
             
              this.AddTestServices();
              Services.AddScoped(_ => vm);
              Services.AddScoped(_ => subVm);
             
              var cut = RenderComponent<AddQuickReminder>();
            
             var formEle = cut.Find("form");
            Assert.NotNull(formEle);
        } 
    }
}