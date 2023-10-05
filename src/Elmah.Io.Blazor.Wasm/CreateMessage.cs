using System.Text.Json.Serialization;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Represent a log message that is being sent to the elmah.io API.
    /// </summary>
    public class CreateMessage
    {
        /// <summary>
        /// Used to identify which application logged this message. You can use this if you have multiple applications and services logging to the same log
        /// </summary>
        [JsonPropertyName("application")]
        public string Application { get; set; }

        /// <summary>
        /// A longer description of the message. For errors this could be a stacktrace, but it's really up to you what to log in there.
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// The hostname of the server logging the message.
        /// </summary>
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// The textual title or headline of the message to log.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The title template of the message to log. This property can be used from logging frameworks that supports
        /// <br/>structured logging like: "{user} says {quote}". In the example, titleTemplate will be this string and title
        /// <br/>will be "Gilfoyle says It's not magic. It's talent and sweat".
        /// </summary>
        [JsonPropertyName("titleTemplate")]
        public string TitleTemplate { get; set; }

        /// <summary>
        /// The source of the code logging the message. This could be the assembly name.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// If the message logged relates to a HTTP status code, you can put the code in this property. This would probably only be relevant for errors,
        /// <br/>but could be used for logging successful status codes as well.
        /// </summary>
        [JsonPropertyName("statusCode")]
        public int? StatusCode { get; set; }

        /// <summary>
        /// The date and time in UTC of the message. If you don't provide us with a value in dateTime, we will set the current date and time in UTC.
        /// </summary>
        [JsonPropertyName("dateTime")]
        public System.DateTimeOffset? DateTime { get; set; }

        /// <summary>
        /// The type of message. If logging an error, the type of the exception would go into type but you can put anything in there, that makes sense for your domain.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// An identification of the user triggering this message. You can put the users email address or your user key into this property.
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }

        /// <summary>
        /// An enum value representing the severity of this message. The following values are allowed: Verbose, Debug, Information, Warning, Error, Fatal
        /// </summary>
        [JsonPropertyName("severity")]
        public string Severity { get; set; }

        /// <summary>
        /// If message relates to a HTTP request, you may send the URL of that request. If you don't provide us with an URL, we will try to find a key named URL in serverVariables.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// If message relates to a HTTP request, you may send the HTTP method of that request. If you don't provide us with a method, we will try to find a key named REQUEST_METHOD in serverVariables.
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }

        /// <summary>
        /// Versions can be used to distinguish messages from different versions of your software. The value of version can be a SemVer compliant string or any other
        /// <br/>syntax that you are using as your version numbering scheme.
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// CorrelationId can be used to group similar log messages together into a single discoverable batch. A correlation ID could be a session ID from ASP.NET Core,
        /// <br/>a unique string spanning multiple microsservices handling the same request, or similar.
        /// </summary>
        [JsonPropertyName("correlationId")]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Code can be used to include source code related to the log message. The code will typically span from a few lines before the line causing the log message
        /// <br/>to a few lines after. For now, all lines above 21 will be removed. This makes room for showing 10 lines before the logging line, the logging line, and
        /// <br/>10 lines after the logging line. Don't include a very large string in this property since that will quickly make the entire messages exceed the max limit
        /// <br/>of 256 kb.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// The log message category. Category can be a string of choice but typically contain a logging category set by a logging framework like NLog or Serilog.
        /// <br/>When logging through a logging framework, this field will be provided by the framework and not something that needs to be set manually.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; }

        /// <summary>
        /// A key/value pair of cookies. This property only makes sense for logging messages related to web requests.
        /// </summary>
        [JsonPropertyName("cookies")]
        public System.Collections.Generic.ICollection<Item> Cookies { get; set; }

        /// <summary>
        /// A key/value pair of form fields and their values. This property makes sense if logging message related to users inputting data in a form.
        /// </summary>
        [JsonPropertyName("form")]
        public System.Collections.Generic.ICollection<Item> Form { get; set; }

        /// <summary>
        /// A key/value pair of query string parameters. This property makes sense if logging message related to a HTTP request.
        /// </summary>
        [JsonPropertyName("queryString")]
        public System.Collections.Generic.ICollection<Item> QueryString { get; set; }

        /// <summary>
        /// A key/value pair of server values. Server variables are typically related to handling requests in a webserver but could be used for other types of information as well.
        /// </summary>
        [JsonPropertyName("serverVariables")]
        public System.Collections.Generic.ICollection<Item> ServerVariables { get; set; }

        /// <summary>
        /// A key/value pair of user-defined fields and their values. When logging an exception, the Data dictionary of
        /// <br/>the exception is copied to this property. You can add additional key/value pairs, by modifying the Data
        /// <br/>dictionary on the exception or by supplying additional key/values to this API.
        /// </summary>
        [JsonPropertyName("data")]
        public System.Collections.Generic.ICollection<Item> Data { get; set; }

        /// <summary>
        /// A list of breadcrumbs preceding this log message.
        /// </summary>
        [JsonPropertyName("breadcrumbs")]
        public System.Collections.Generic.ICollection<Breadcrumb> Breadcrumbs { get; set; }
    }
}
