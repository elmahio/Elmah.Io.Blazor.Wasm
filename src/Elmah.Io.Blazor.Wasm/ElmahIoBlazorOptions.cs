using System;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Contain properties for configuring the elmah.io middleware for ASP.NET Core.
    /// </summary>
    public class ElmahIoBlazorOptions
    {
        /// <summary>
        /// The API key from the elmah.io UI.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The id of the log to send messages to.
        /// </summary>
        public Guid LogId { get; set; }
    }
}
