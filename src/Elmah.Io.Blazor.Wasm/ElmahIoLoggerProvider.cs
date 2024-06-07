using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// An ILoggerProvider for registering the elmah.io logger.
    /// </summary>
    /// <remarks>
    /// Create a new instance using the provided HTTP client and options.
    /// </remarks>
    public sealed class ElmahIoLoggerProvider(HttpClient httpClient, IOptions<ElmahIoBlazorOptions> options) : ILoggerProvider
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly ElmahIoBlazorOptions options = options.Value;

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
