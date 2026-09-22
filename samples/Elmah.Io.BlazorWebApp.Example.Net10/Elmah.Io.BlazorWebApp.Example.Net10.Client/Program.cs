using Elmah.Io.Blazor.Wasm;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// IMPORTANT: this is where the magic happens. Insert your api key found on the profile as well as the log id of the log to log to.
// This handles logging for the parts of the app rendered with Interactive WebAssembly (and Interactive Auto, once it falls back to WebAssembly).
builder.Logging.AddElmahIo(o =>
{
    o.ApiKey = "API_KEY";
    o.LogId = new Guid("LOG_ID");
});

await builder.Build().RunAsync();
