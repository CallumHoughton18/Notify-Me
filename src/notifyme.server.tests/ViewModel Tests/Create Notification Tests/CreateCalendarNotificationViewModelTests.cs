using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.tests.TestData;
using notifyme.shared;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.ViewModels.CreateNotification;
using Xunit;

namespace notifyme.server.tests.ViewModelTests.CreateNotificationTests
{
    public class CreateCalendarNotificationViewModelTests : BaseCreateNotificationTests
    {
        [Theory]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.None, false, "16 15 14 2 1 ?")]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Weekly, true, "0 15 14 ? * FRI")]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Monthly, true, "0 15 14 1 * ?")]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Yearly, true, "16 15 14 2 1 ? *")]
        public async Task Should_Save_New_Correct_Notification(
            NotifyMeEnums.CalendarNotificationRepeatFormat format,
            bool expectedRepeatable,
            string expectedCron)
        {
            Notification savedNotification = null;

            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock) =
                CreateSutDependencies(x => savedNotification = x);

            var sut = new CreateCalendarNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object);

            sut.CalendarNotification.Title = MockNotificationTitle;
            sut.CalendarNotification.Body = MockNotificationBody;
            sut.CalendarNotification.FirstDateOccurance = new DateTime(1970, 1, 2);
            sut.CalendarNotification.FirstTimeOccurance = new TimeSpan(14, 15, 16);
            sut.CalendarNotification.RepeatFormat = format;

            await sut.SaveNotification();

            Assert.True(savedNotification.NotificationTitle == MockNotificationTitle);
            Assert.True(savedNotification.NotificationBody == MockNotificationBody);
            Assert.True(savedNotification.CronJobString == expectedCron);
            Assert.True(savedNotification.Repeat == expectedRepeatable);
        }

        [Theory]
        [ClassData(typeof(InvalidCalendarNotificationData))]
        public async Task Should_Not_Save_Notification_With_Invalid_Values(CalendarNotification calendarNotification)
        {
            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock) =
                CreateSutDependencies(x => { });

            var sut = new CreateCalendarNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object)
            {
                CalendarNotification = calendarNotification
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
