using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using notifyme.scheduler.Services;
using notifyme.server.tests.Mocks;
using notifyme.server.tests.Test_Data;
using notifyme.shared;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;
using notifyme.shared.Service_Interfaces;
using notifyme.shared.ViewModels.Create_Notification;
using Xunit;

namespace notifyme.server.tests.ViewModel_Tests.Create_Notification_Tests
{
    public class CreateCalendarNotificationViewModelTests : BaseCreateNotificationTests
    {
        private (PushNotificationSubscriberServiceMock, NotificationSchedulerMock, NotificationRepositoryMock,
            AuthServiceMock, Mock<IServerDateTimeProvider>, Mock<IClientDateTimeProvider>) 
            CreateCalendarSutDependencies(Action<Notification> savedNotificationCallback, 
            long clientUtcOffsetInMinutes,
            TimeZoneInfo serverTimeZone)
        {
            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock) =
                CreateBaseSutDependencies(savedNotificationCallback);
            dateTimeMock.SetupGet(x => x.CurrentTimeZone).Returns(serverTimeZone);
            var mockClientDateTimeProvider = new Mock<IClientDateTimeProvider>();
            mockClientDateTimeProvider.Setup(x => x.GetClientTimeZoneOffsetInMinutes())
                .ReturnsAsync(clientUtcOffsetInMinutes);

            return (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock,
                mockClientDateTimeProvider);
        }
        
        [Theory]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.None, false, "16 15 14 2 1 ?", 0)]
        // test data for when client timezone has a UTC offset
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.None, false, "16 15 16 2 1 ?", 120)]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Weekly, true, "0 15 14 ? * FRI", 0)]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Monthly, true, "0 15 14 2 * ?", 0)]
        [InlineData(NotifyMeEnums.CalendarNotificationRepeatFormat.Yearly, true, "16 15 14 2 1 ? *", 0)]
        public async Task Should_Save_New_Correct_Notification(
            NotifyMeEnums.CalendarNotificationRepeatFormat format,
            bool expectedRepeatable,
            string expectedCron,
            long clientUtcOffsetInMinutes)
        {
            
            Notification savedNotification = null;

            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock, clientDateTimeMock) =
                CreateCalendarSutDependencies(x => savedNotification = x, clientUtcOffsetInMinutes, TimeZoneInfo.Utc);

            var sut = new CreateCalendarNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object, clientDateTimeMock.Object);

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
            var (pushSubMock, notificationSchedulerMock, savedNotifRepoMock, authServiceMock, dateTimeMock, clientDateTimeMock) =
                CreateCalendarSutDependencies(x => { }, 0, TimeZoneInfo.Utc);

            var sut = new CreateCalendarNotificationViewModel(pushSubMock.Object, notificationSchedulerMock.Object,
                new CronExpressionBuilder(), savedNotifRepoMock.Object, authServiceMock.Object, dateTimeMock.Object, clientDateTimeMock.Object)
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
