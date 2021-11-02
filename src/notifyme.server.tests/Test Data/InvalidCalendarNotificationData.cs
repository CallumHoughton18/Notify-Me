using System;
using System.Collections.Generic;
using notifyme.shared.Models;
using Xunit;

namespace notifyme.server.tests.Test_Data
{
    public class InvalidCalendarNotificationData : TheoryData<CalendarNotification>
    {
        private readonly List<CalendarNotification> _data = new()
        {
            new CalendarNotification()
            {
                Body = "test",
                FirstDateOccurance = new DateTime(1970, 1, 1),
                FirstTimeOccurance = new TimeSpan(1, 1, 1),
                RepeatFormat = shared.NotifyMeEnums.CalendarNotificationRepeatFormat.None
            },
            new CalendarNotification()
            {
                Title = "testTitle",
                FirstDateOccurance = new DateTime(1970, 1, 1),
                FirstTimeOccurance = new TimeSpan(1, 1, 1),
                RepeatFormat = shared.NotifyMeEnums.CalendarNotificationRepeatFormat.None
            },
            new CalendarNotification()
            {
                Title = "testTitle",
                Body = "test",
                FirstTimeOccurance = new TimeSpan(1, 1, 1),
                RepeatFormat = shared.NotifyMeEnums.CalendarNotificationRepeatFormat.None
            },
            new CalendarNotification()
            {
                Title = "testTitle",
                Body = "test",
                FirstDateOccurance = new DateTime(1970, 1, 1),
                RepeatFormat = shared.NotifyMeEnums.CalendarNotificationRepeatFormat.None
            },
        };

        public InvalidCalendarNotificationData()
        {
            _data.ForEach(Add);
        }
    }
}
