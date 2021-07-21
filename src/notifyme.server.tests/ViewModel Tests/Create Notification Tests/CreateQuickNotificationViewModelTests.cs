using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.tests.Mocks;
using notifyme.server.tests.Test_Data;
using notifyme.server.tests.ViewModelTests.CreateNotificationTests;
using notifyme.shared;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ServiceInterfaces;
using notifyme.shared.ViewModels.CreateNotification;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests.CreateNotificationTests
{
    public class CreateQuickNotificationViewModelTests : BaseCreateNotificationTests
    {
        [Theory]
        [InlineData(NotifyMeEnums.QuickNotificationTimeFormat.Minutes, "1 6 12 1 1 ?")]
        [InlineData(NotifyMeEnums.QuickNotificationTimeFormat.Hours, "1 1 17 1 1 ?")]
        [InlineData(NotifyMeEnums.QuickNotificationTimeFormat.Days, "1 1 12 6 1 ?")]
        public async Task Should_Save_New_Correct_Notification(NotifyMeEnums.QuickNotificationTimeFormat format,
            string expectedCron)
        {
            Notification savedNotification = null;

            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock) =
                CreateSutDependencies(x => savedNotification = x);

            var sut = new CreateQuickNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object);

            sut.QuickNotification.Title = MockNotificationTitle;
            sut.QuickNotification.Body = MockNotificationBody;
            sut.QuickNotification.RequestedTime = 5;
            sut.QuickNotification.TimeFormat = format;

            await sut.SaveNotification();

            Assert.True(savedNotification.NotificationTitle == MockNotificationTitle);
            Assert.True(savedNotification.NotificationBody == MockNotificationBody);
            Assert.True(savedNotification.CronJobString == expectedCron);
            Assert.False(savedNotification.Repeat);
        }

        [Theory]
        [ClassData(typeof(InvalidQuickNotificationData))]
        public async Task Should_Not_Save_Notification_With_Invalid_Values(QuickNotification invalidNotification)
        {
            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock) =
                CreateSutDependencies(x => { });

            var sut = new CreateQuickNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object)
            {
                QuickNotification = invalidNotification
            };

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await sut.SaveNotification();
            });
            savedNotifRepoMock.Verify(x =>
                x.AddOrUpdateAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}