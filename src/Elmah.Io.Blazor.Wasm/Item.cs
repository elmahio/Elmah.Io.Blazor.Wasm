using System.Text.Json.Serialization;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Represents a key value pair.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Initializes a new instance of the Item class.
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Item class.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <param name="value">The value of the item.</param>
        public Item(string key = default, string value = default)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the key of the item.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

    }
}
