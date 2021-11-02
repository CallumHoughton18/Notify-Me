using Moq;
using notifyme.shared.Models;
using notifyme.shared.Service_Interfaces;

namespace notifyme.server.tests.Mocks
{
    public class AuthServiceMock : Mock<IAuthService>
    {
        public AuthServiceMock MockGetCurrentUser(User user)
        {
            Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);
            return this;
        }
    }
}