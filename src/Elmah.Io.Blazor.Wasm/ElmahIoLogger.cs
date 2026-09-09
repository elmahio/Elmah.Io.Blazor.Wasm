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
    /// <remarks>
    /// Create a new instance of the logger. You typically don't want to call this constructor but rather call the AddElmahIo method.
    /// </remarks>
    public class ElmahIoLogger(HttpClient httpClient, ElmahIoBlazorOptions options) : ILogger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "The URL will never be different")]
        internal const string DefaultHost = "https://v4.api.elmah.io";

        private readonly HttpClient httpClient = httpClient;
        private readonly ElmahIoBlazorOptions options = options;

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
                Title = state.Title(formatter, exception),
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

            httpClient.PostAsJsonAsync($"{DefaultHost}/messages/{options.LogId}?api_key={options.ApiKey}", createMessage)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine($"elmah.io: failed to log message: {task.Exception?.GetBaseException().Message}");
                    }
                    else if (!task.Result.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"elmah.io: failed to log message: {(int)task.Result.StatusCode} {task.Result.StatusCode}");
                    }
                });
        }

        private List<Item> Data(Exception exception)
        {
            var res = exception?.ToDataList() ?? [];
            res.Add(new Item("X-ELMAHIO-isBlazor", $"true"));
            return res;
        }

        private string LogLevelToSeverity(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Critical => "Fatal",
                LogLevel.Debug => "Debug",
                LogLevel.Error => "Error",
                LogLevel.Information => "Information",
                LogLevel.Trace => "Verbose",
                LogLevel.Warning => "Warning",
                _ => "Information",
            };
        }
    }
}
