using System.Threading.Tasks;
using notifyme.shared.Models;

namespace notifyme.shared.Service_Interfaces
{
    public interface IAuthService
    {
        Task<User> GetCurrentUserAsync();
    }
}