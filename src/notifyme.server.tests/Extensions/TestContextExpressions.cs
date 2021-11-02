using System.Net.Http;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace notifyme.server.tests.Extensions
{
    public static class TestContextExpressions
    {
        public static void AddTestServices(this TestContext ctx)
        {
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddMudServices(options =>
            {
                options.SnackbarConfiguration.ShowTransitionDuration = 0;
                options.SnackbarConfiguration.HideTransitionDuration = 0;
            });
            ctx.Services.AddScoped(_ => new HttpClient());
            ctx.Services.AddOptions();
        }
    }
}
