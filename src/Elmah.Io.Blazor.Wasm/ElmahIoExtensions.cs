using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Extension methods to help install Elmah.Io.Blazor.Wasm.
    /// </summary>
    public static class LoggingBilderElmahIoExtensions
    {
        /// <summary>
        /// Add elmah.io with the specified options.
        /// </summary>
        public static ILoggingBuilder AddElmahIo(this ILoggingBuilder loggingBuilder, Action<ElmahIoBlazorOptions> configure)
        {
            loggingBuilder.Services.Configure(configure);
            loggingBuilder.Services.AddSingleton<ILoggerProvider, ElmahIoLoggerProvider>(services =>
            {
                var httpClient = new HttpClient { BaseAddress = new Uri("https://api.elmah.io") };
                var options = services.GetService<IOptions<ElmahIoBlazorOptions>>();
                return new ElmahIoLoggerProvider(httpClient, options);
            });
            return loggingBuilder;
        }
    }
}
