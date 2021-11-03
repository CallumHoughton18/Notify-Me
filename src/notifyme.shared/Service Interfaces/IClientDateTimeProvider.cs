using System.Threading.Tasks;

namespace notifyme.shared.Service_Interfaces
{
    public interface IClientDateTimeProvider
    {
        Task InitializeAsync();
        Task<long> GetClientTimeZoneOffsetInMinutes();
    }
}