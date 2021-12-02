using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// An ILoggerProvider for registering the elmah.io logger.
    /// </summary>
    public class ElmahIoLoggerProvider : ILoggerProvider
    {
        private readonly HttpClient httpClient;
        private readonly ElmahIoBlazorOptions options;

        /// <summary>
        /// Create a new instance using the provided HTTP client and options.
        /// </summary>
        public ElmahIoLoggerProvider(HttpClient httpClient, IOptions<ElmahIoBlazorOptions> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return new ElmahIoLogger(httpClient, options);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
