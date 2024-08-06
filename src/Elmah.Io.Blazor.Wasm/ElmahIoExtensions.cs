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
        /// Add Elmah.Io.Blazor.Wasm with the specified options.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "The URL will never be different")]
        public static ILoggingBuilder AddElmahIo(this ILoggingBuilder loggingBuilder, Action<ElmahIoBlazorOptions> configure)
        {
            loggingBuilder.AddElmahIo();
            loggingBuilder.Services.Configure(configure);
            return loggingBuilder;
        }

        /// <summary>
        /// Add Elmah.Io.Blazor.Wasm without any options. Calling this method requires you to configure elmah.io options manually like this:
        /// <code>builder.Services.Configure&lt;ElmahIoBlazorOptions&gt;(builder.Configuration.GetSection("ElmahIo"));</code>
        /// Blazor WebAssembly doesn't have an appsettings.json file as a default. You will need to create one in the wwwroot directory.
        /// Make sure to set Build Action to Content and Copy to Output Directory to Copy if Newer.
        /// </summary>
        public static ILoggingBuilder AddElmahIo(this ILoggingBuilder loggingBuilder)
        {
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
