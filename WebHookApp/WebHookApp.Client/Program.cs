using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebHookApp.Client;
using Microsoft.AspNetCore.Components.Web;
using WebHookApp.Client.Services;

namespace WebHookApp.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp=>new HttpClient { BaseAddress=new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<WebHookService>();


            await builder.Build().RunAsync();
        }
    }
}
