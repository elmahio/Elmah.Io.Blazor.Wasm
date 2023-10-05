using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Implementation of Microsoft.Extensions.Logging's ILogger interface that log messages to elmah.io.
    /// </summary>
    public class ElmahIoLogger : ILogger
    {
        private readonly HttpClient httpClient;
        private readonly ElmahIoBlazorOptions options;

        /// <summary>
        /// Create a new instance of the logger. You typically don't want to call this constructor but rather call the AddElmahIo method.
        /// </summary>
        public ElmahIoLogger(HttpClient httpClient, ElmahIoBlazorOptions options)
        {
            this.httpClient = httpClient;
            this.options = options;
        }

        /// <summary>
        /// Scopes are currently not supported for this logger.
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Tell the logger to store a message in elmah.io.
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var baseException = exception?.GetBaseException();

            var createMessage = new CreateMessage
            {
                DateTime = DateTime.UtcNow,
                Detail = exception?.ToString(),
                Type = baseException?.GetType().FullName,
                Title = formatter(state, exception),
                Data = Data(exception),
                Severity = LogLevelToSeverity(logLevel),
                Source = baseException?.Source,
                Hostname = Environment.MachineName,
                Application = options.Application,
            };

            if (options.OnFilter != null && options.OnFilter(createMessage))
            {
                return;
            }

            options.OnMessage?.Invoke(createMessage);

            httpClient.PostAsJsonAsync($"https://api.elmah.io/v3/messages/{options.LogId}?api_key={options.ApiKey}", createMessage);
        }

        private List<Item> Data(Exception exception)
        {
            var res = exception?.ToDataList() ?? new List<Item>();
            res.Add(new Item("X-ELMAHIO-isBlazor", $"true"));
            return res;
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
