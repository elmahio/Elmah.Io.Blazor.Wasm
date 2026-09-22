using Elmah.Io.BlazorWebApp.Example.Net10.Components;
using Elmah.Io.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// IMPORTANT: this is where the magic happens. Insert your api key found on the profile as well as the log id of the log to log to.
// This handles logging for the server-rendered (static SSR and Interactive Server) parts of the app.
builder.Logging.AddElmahIo(options =>
{
    options.ApiKey = "API_KEY";
    options.LogId = new Guid("LOG_ID");
});

// The elmah.io provider can log any log level, but we recommend only to log warning and up
builder.Logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Adds contextual information (like URL and status code) to log messages coming from the server-rendered parts of the app.
app.UseElmahIoExtensionsLogging();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Elmah.Io.BlazorWebApp.Example.Net10.Client._Imports).Assembly);

app.Run();
