﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elmah.Io.Blazor.Wasm
{
    /// <summary>
    /// Extension methods to help install Elmah.Io.Blazor.Wasm.
    /// </summary>
    public static class LoggingBilderElmahIoExtensions
    {
        /// <summary>
        /// Add elmah.io with the specified options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        public static ILoggingBuilder AddElmahIo(this ILoggingBuilder loggingBuilder, Action<ElmahIoBlazorOptions> configure)
        {
            loggingBuilder.Services.Configure(configure);
            loggingBuilder.Services.AddSingleton<ILoggerProvider, ElmahIoLoggerProvider>(services =>
            {
                var httpClient = new HttpClient { BaseAddress = new Uri("https://api.elmah.io") };
                var options = services.GetService<IOptions<ElmahIoBlazorOptions>>();
                return new ElmahIoLoggerProvider(httpClient, options);
            });
            return loggingBuilder;
        }
    }

    /// <summary>
    /// Extension methods provided by elmah.io for the Exception class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Pulls as much information as possible from an Exception to a list of elmah.io Items.
        /// </summary>
        public static List<Item> ToDataList(this Exception exception)
        {
            if (exception == null) return null;

            var result = new List<Item>();
            var e = exception;
            while (e != null)
            {
                var data = e
                    .Data
                    .Keys
                    .Cast<object>()
                    .Where(k => !string.IsNullOrWhiteSpace(k.ToString()))
                    .Select(k => new Item { Key = k.ToString(), Value = Value(e.Data, k) })
                    .ToList();
                if (data != null && data.Count() > 0)
                {
                    result.AddRange(data);
                }

                if (!string.IsNullOrWhiteSpace(e.HelpLink))
                {
                    result.Add(new Item { Key = "Exception.HelpLink", Value = e.HelpLink });
                }

                var exceptionSpecificItems = ExceptionSpecificItems(e);
                if (exceptionSpecificItems.Count > 0)
                {
                    result.AddRange(exceptionSpecificItems);
                }

                e = e.InnerException;
            }

            return result;
        }

        /// <summary>
        /// Helper method to extract items from different known exception types and their properties.
        /// </summary>
        private static List<Item> ExceptionSpecificItems(Exception e)
        {
            var result = new List<Item>();
            if (e is ArgumentException ae)
            {
                if (!string.IsNullOrWhiteSpace(ae.ParamName))
                    result.Add(new Item { Key = ae.ItemName(nameof(ae.ParamName)), Value = ae.ParamName });
            }

            if (e is BadImageFormatException bife)
            {
                if (!string.IsNullOrWhiteSpace(bife.FileName))
                    result.Add(new Item { Key = bife.ItemName(nameof(bife.FileName)), Value = bife.FileName });
#if NETSTANDARD2_0 || NET45 || NET46 || NET461
                if (!string.IsNullOrWhiteSpace(bife.FusionLog))
                    result.Add(new Item { Key = bife.ItemName(nameof(bife.FusionLog)), Value = bife.FusionLog });
#endif
            }

            if (e is FileNotFoundException fnfe)
            {
                if (!string.IsNullOrWhiteSpace(fnfe.FileName))
                    result.Add(new Item { Key = fnfe.ItemName(nameof(fnfe.FileName)), Value = fnfe.FileName });
#if NETSTANDARD2_0 || NET45 || NET46 || NET461
                if (!string.IsNullOrWhiteSpace(fnfe.FusionLog))
                    result.Add(new Item { Key = fnfe.ItemName(nameof(fnfe.FusionLog)), Value = fnfe.FusionLog });
#endif
            }

#if NETSTANDARD1_4 || NETSTANDARD2_0 || NET45 || NET46 || NET461
            if (e is System.Net.Sockets.SocketException se)
            {
                result.Add(new Item { Key = se.ItemName(nameof(se.SocketErrorCode)), Value = se.SocketErrorCode.ToString() });
#if NETSTANDARD2_0 || NET45 || NET46 || NET461
                result.Add(new Item { Key = se.ItemName(nameof(se.ErrorCode)), Value = se.ErrorCode.ToString() });
#endif
            }
#endif

#if NETSTANDARD2_0 || NET45 || NET46 || NET461
            if (e is System.Net.WebException we)
            {
                string ss = ItemName(we, nameof(we.Status));
                result.Add(new Item { Key = we.ItemName(nameof(we.Status)), Value = we.Status.ToString() });
            }
#endif

            return result;
        }

        /// <summary>
        /// Generate an item name of a property on an exception.
        /// </summary>
        public static string ItemName(this Exception exception, string propertyName)
        {
            return $"{exception.GetType().Name}.{propertyName}";
        }

        private static string Value(IDictionary data, object key)
        {
            var value = data[key];
            if (value == null) return string.Empty;
            return value.ToString();
        }


        /// <summary>
        /// Represents a key value pair.
        /// </summary>
        public partial class Item
        {
            /// <summary>
            /// Initializes a new instance of the Item class.
            /// </summary>
            public Item()
            {
                CustomInit();
            }

            /// <summary>
            /// Initializes a new instance of the Item class.
            /// </summary>
            /// <param name="key">The key of the item.</param>
            /// <param name="value">The value of the item.</param>
            public Item(string key = default(string), string value = default(string))
            {
                Key = key;
                Value = value;
                CustomInit();
            }

            /// <summary>
            /// An initialization method that performs custom operations like setting defaults
            /// </summary>
            partial void CustomInit();

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
}
