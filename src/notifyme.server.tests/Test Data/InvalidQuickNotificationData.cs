using System.Collections;
using System.Collections.Generic;

namespace notifyme.server.tests.Test_Data
{
    public class InvalidQuickNotificationData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}