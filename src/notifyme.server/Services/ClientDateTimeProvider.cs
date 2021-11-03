using System.Threading.Tasks;
using Microsoft.JSInterop;
using notifyme.shared.Service_Interfaces;

namespace notifyme.server.Services
{
    public class ClientDateTimeProvider : IClientDateTimeProvider
    {
        private readonly IJSRuntime _jSRuntime;
        private IJSObjectReference _module;
        
        public ClientDateTimeProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }
        
        public async Task InitializeAsync()
        {
            _module = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/ClientDateTime.js");
        }

        public async Task<long> GetClientTimeZoneOffsetInMinutes()
        {
            return await _module.InvokeAsync<long>("getClientTimeZoneOffsetInMinutes");
        }
    }
}