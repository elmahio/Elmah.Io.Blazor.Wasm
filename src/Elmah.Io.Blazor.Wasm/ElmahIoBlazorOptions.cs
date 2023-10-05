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

        /// <summary>
        /// An application name to put on all messages.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Register an action to be called before logging an error. Use the OnMessage action to
        /// decorate error messages with additional information.
        /// </summary>
        public Action<CreateMessage> OnMessage { get; set; }

        /// <summary>
        /// Register an action to filter log messages. Use this to add client-side ignore
        /// of some error messages. If the filter action returns true, the error is ignored.
        /// </summary>
        public Func<CreateMessage, bool> OnFilter { get; set; }
    }
}
