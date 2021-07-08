using System.Collections;
using System.Collections.Generic;
using notifyme.shared;
using notifyme.shared.Models;
using Xunit;

namespace notifyme.server.tests.Test_Data
{
    public class InvalidQuickNotificationData : TheoryData<QuickNotification>
    {
        private readonly List<QuickNotification> _data = new()
        {
            new QuickNotification()
            {
                RequestedTime = 1, Body = "test", TimeFormat = NotifyMeEnums.QuickNotificationTimeFormat.Days,
            },
            new QuickNotification()
            {
                Body = "test", RequestedTime = 0, TimeFormat = NotifyMeEnums.QuickNotificationTimeFormat.Days, Title = "testTitle"
            },
            new QuickNotification()
            {
                Body = "test", RequestedTime = 1001, TimeFormat = NotifyMeEnums.QuickNotificationTimeFormat.Days, Title = "testTitle"
            },
            new QuickNotification()
            {
                RequestedTime = 1, TimeFormat = NotifyMeEnums.QuickNotificationTimeFormat.Days, Title = "testTitle"
            },
        };

        public InvalidQuickNotificationData()
        {
            _data.ForEach(Add);
        }
    }
}