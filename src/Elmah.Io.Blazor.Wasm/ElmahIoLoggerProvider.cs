using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Elmah.Io.Blazor.Wasm
{
    public class ElmahIoLoggerProvider : ILoggerProvider
    {
        private readonly HttpClient httpClient;
        private readonly ElmahIoBlazorOptions options;

        public ElmahIoLoggerProvider(HttpClient httpClient, IOptions<ElmahIoBlazorOptions> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ElmahIoLogger(httpClient, options);
        }

        public void Dispose()
        {
        }
    }

    public class ElmahIoLogger : ILogger
    {
        private readonly HttpClient httpClient;
        private readonly ElmahIoBlazorOptions options;

        public ElmahIoLogger(HttpClient httpClient, ElmahIoBlazorOptions options)
        {
            this.httpClient = httpClient;
            this.options = options;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var baseException = exception?.GetBaseException();

            httpClient.PostAsJsonAsync(
                $"https://api.elmah.io/v3/messages/{options.LogId}?api_key={options.ApiKey}",
                new
                {
                    dateTime = DateTime.UtcNow,
                    detail = exception?.StackTrace,
                    type = baseException?.GetType().FullName,
                    title = formatter(state, exception),
                    data = exception?.ToDataList(),
                    severity = LogLevelToSeverity(logLevel),
                    source = baseException?.Source,
                    hostname = Environment.MachineName
                });
        }

        private string LogLevelToSeverity(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return "Fatal";
                case LogLevel.Debug:
                    return "Debug";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Trace:
                    return "Verbose";
                case LogLevel.Warning:
                    return "Warning";
                default:
                    return "Information";
            }
        }
    }
}
