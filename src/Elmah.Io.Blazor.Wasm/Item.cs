using System.Text.Json.Serialization;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Represents a key value pair.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the Item class.
    /// </remarks>
    /// <param name="key">The key of the item.</param>
    /// <param name="value">The value of the item.</param>
    public class Item(string key = default, string value = default)
    {

        /// <summary>
        /// Gets or sets the key of the item.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; } = key;

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; } = value;

    }
}
