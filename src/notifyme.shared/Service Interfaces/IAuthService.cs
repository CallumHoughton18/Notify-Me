using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<User> GetCurrentUserAsync();
    }
}