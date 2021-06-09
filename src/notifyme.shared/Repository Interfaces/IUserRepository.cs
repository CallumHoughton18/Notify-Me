using System;
using System.Threading.Tasks;
using notifyme.shared.Models;
using notifyme.shared.Models.DataStore_Models;

namespace notifyme.shared.RepositoryInterfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByUserName(string username);
    }
}
