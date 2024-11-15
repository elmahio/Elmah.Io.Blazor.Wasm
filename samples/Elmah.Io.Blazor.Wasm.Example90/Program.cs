#pragma warning disable S125 // Sections of code should not be commented out
using Elmah.Io.Blazor.Wasm;
using Elmah.Io.Blazor.Wasm.Example90;
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

    // Optional application name to set on all messages.
    //o.Application = "Blazor WASM 9.0 elmah.io sample";

    // Optional OnMessage callback that can be used to decorate messages before sent to elmah.io.
    //o.OnMessage = msg =>
    //{
    //    msg.Version = "9.0";
    //};

    // Optional OnFilter callback that can be used to ignore log messages when matching specific conditions.
    //o.OnFilter = msg =>
    //{
    //    return msg.Detail != null && msg.Detail.Contains("Attempted");
    //};
}
);

// Elmah.Io.Blazor.Wasm can also be configured from appsettings.json like this:
//builder.Services.Configure<ElmahIoBlazorOptions>(builder.Configuration.GetSection("ElmahIo"));
//builder.Logging.AddElmahIo();

await builder.Build().RunAsync();
#pragma warning restore S125 // Sections of code should not be commented out
