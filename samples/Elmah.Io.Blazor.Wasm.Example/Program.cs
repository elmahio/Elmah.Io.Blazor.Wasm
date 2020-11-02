using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Elmah.Io.Blazor.Wasm.Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // IMPORTANT: this is where the magic happens. Insert your api key found on the profile as well as the log id of the log to log to.
            builder.Logging.AddElmahIo(o =>
            {
                o.ApiKey = "API_KEY";
                o.LogId = new Guid("LOG_ID");
            });

            await builder.Build().RunAsync();
        }
    }
}
