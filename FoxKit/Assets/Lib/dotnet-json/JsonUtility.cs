// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.IO;
using System.Text;

namespace Rotorz.Json
{
    /// <summary>
    /// Utility methods for interacting with <see cref="JsonNode"/> instances.
    /// </summary>
    public static class JsonUtility
    {
        /// <inheritdoc cref="JsonNode.ReadFrom(string)"/>
        public static JsonNode ReadFrom(string json)
        {
            return JsonNode.ReadFrom(json);
        }

        /// <inheritdoc cref="JsonNode.ReadFrom(Stream)"/>
        public static JsonNode ReadFrom(Stream stream)
        {
            return JsonNode.ReadFrom(stream);
        }

        /// <inheritdoc cref="JsonNode.ReadFrom(TextReader)"/>
        public static JsonNode ReadFrom(TextReader reader)
        {
            return JsonNode.ReadFrom(reader);
        }

        /// <inheritdoc cref="JsonNode.ConvertFrom(object)"/>
        public static JsonNode ConvertFrom(object value)
        {
            return JsonNode.ConvertFrom(value);
        }


        /// <summary>
        /// Writes a <see cref="JsonNode"/> instance to the provided <see cref="StringBuilder"/>
        /// with the default formatting.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="builder">String builder.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="builder"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, StringBuilder builder)
        {
            WriteTo(node, builder, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Writes a <see cref="JsonNode"/> instance to the provided <see cref="StringBuilder"/>
        /// with custom formatting.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="builder">String builder.</param>
        /// <param name="settings">Custom settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="builder"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, StringBuilder builder, JsonWriterSettings settings)
        {
            var writer = JsonWriter.Create(builder, settings);
            if (node != null) {
                node.Write(writer);
            }
            else {
                writer.WriteNull();
            }
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="Stream"/> with custom settings.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="stream">Stream that data will be written to.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="stream"/> is not writable.
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, Stream stream)
        {
            WriteTo(node, stream, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="Stream"/> with custom settings.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="stream">Stream that data will be written to.</param>
        /// <param name="settings">Custom settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="stream"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="stream"/> is not writable.
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, Stream stream, JsonWriterSettings settings)
        {
            var writer = JsonWriter.Create(stream, settings);
            if (node != null) {
                node.Write(writer);
            }
            else {
                writer.WriteNull();
            }
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with custom settings.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="textWriter">Text writer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="textWriter"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, TextWriter textWriter)
        {
            WriteTo(node, textWriter, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with custom settings.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="textWriter">Text writer.</param>
        /// <param name="settings">Custom settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="textWriter"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="JsonWriter"/>
        /// <seealso cref="JsonNode.Write(IJsonWriter)"/>
        public static void WriteTo(this JsonNode node, TextWriter textWriter, JsonWriterSettings settings)
        {
            var writer = JsonWriter.Create(textWriter, settings);
            if (node != null) {
                node.Write(writer);
            }
            else {
                writer.WriteNull();
            }
        }

        /// <summary>
        /// Converts the specified <see cref="JsonNode"/> into a JSON encoded string with
        /// the default formatting.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <returns>
        /// The JSON encoded string.
        /// </returns>
        /// <seealso cref="ToJsonString(JsonNode, JsonWriterSettings)"/>
        public static string ToJsonString(this JsonNode node)
        {
            return ToJsonString(node, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Converts the specified <see cref="JsonNode"/> into a JSON encoded string with
        /// the default formatting.
        /// </summary>
        /// <param name="node">A <see cref="JsonNode"/> instance or <c>null</c>.</param>
        /// <param name="settings">Custom settings.</param>
        /// <returns>
        /// The JSON encoded string.
        /// </returns>
        /// <seealso cref="ToJsonString(JsonNode)"/>
        public static string ToJsonString(this JsonNode node, JsonWriterSettings settings)
        {
            if (node == null) {
                return "null";
            }

            var writer = JsonWriter.Create(settings);
            node.Write(writer);
            return writer.ToString();
        }
    }
}
