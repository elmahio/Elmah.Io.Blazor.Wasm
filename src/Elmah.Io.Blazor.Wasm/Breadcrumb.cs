using System.Text.Json.Serialization;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// A breadcrumb represent a step preceding a log message.
    /// </summary>
    public partial class Breadcrumb
    {
        /// <summary>
        /// The date and time in UTC of the breadcrumb. If no date and time is provided, we will use the current date and time in UTC.
        /// </summary>
        [JsonPropertyName("dateTime")]
        public System.DateTimeOffset? DateTime { get; set; }

        /// <summary>
        /// An enum value representing the severity of this breadcrumb. The following values are allowed: Verbose, Debug, Information, Warning, Error, Fatal.
        /// </summary>
        [JsonPropertyName("severity")]
        public string Severity { get; set; }

        /// <summary>
        /// An action representing the breadcrumb. You can set a custom action or use one of the built-in: click, submit, navigation, request, error.
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; }

        /// <summary>
        /// A message representing the breadcrumb. This should elaborate on the action.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
