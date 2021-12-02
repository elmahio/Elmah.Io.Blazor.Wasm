using Elmah.Io.Blazor.Wasm;
using Elmah.Io.Blazor.Wasm.Example60;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// IMPORTANT: this is where the magic happens. Insert your api key found on the profile as well as the log id of the log to log to.
builder.Logging.AddElmahIo(o =>
{
    o.ApiKey = "API_KEY";
    o.LogId = new Guid("LOG_ID");

    // Optional application name to set on all messages
    //o.Application = "Blazor WASM 6.0 elmah.io sample";
});

await builder.Build().RunAsync();
