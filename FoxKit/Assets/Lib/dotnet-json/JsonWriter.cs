// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Json.Internal;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Rotorz.Json
{
    /// <summary>
    /// A <see cref="IJsonWriter"/> that can be used to write JSON encoded data which
    /// accepts formatting settings. This class can be used to manually write JSON
    /// encoded data without the need to instantiate any <see cref="JsonNode"/> instances.
    /// </summary>
    /// <remarks>
    /// <para>Each <see cref="JsonNode"/> has as custom implementation of <see cref="JsonNode.ToString()"/>
    /// which produces a JSON encoded strings:</para>
    /// <code language="csharp"><![CDATA[
    /// var card = new JsonObjectNode();
    /// card["Name"] = "Jessica";
    /// card["Age"] = 24;
    /// string json = card.ToString();
    /// ]]></code>
    /// <para>Alternative the more verbose implementation would be the following:</para>
    /// <code language="csharp"><![CDATA[
    /// var writer = JsonWriter.Create();
    /// card.Write(writer);
    /// var json = writer.ToString();
    /// ]]></code>
    /// </remarks>
    /// <example>
    /// <para>The following code demonstrates how to manually write JSON data:</para>
    /// <code language="csharp"><![CDATA[
    /// var writer = JsonWriter.Create();
    /// writer.WriteStartObject();
    /// writer.WritePropertyKey("Name");
    /// writer.WriteString("Jessica");
    /// writer.WritePropertyKey("Age");
    /// writer.WriteInteger(24);
    /// writer.WriteEndObject();
    /// ]]></code>
    /// </example>
    public sealed class JsonWriter : IJsonWriter
    {
        #region Factory Methods

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance with the default formatting.
        /// </summary>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        public static JsonWriter Create()
        {
            return Create(JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance with custom formatting.
        /// </summary>
        /// <param name="settings">Custom settings.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        public static JsonWriter Create(JsonWriterSettings settings)
        {
            return Create(new StringWriter(), settings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with the default formatting.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="builder"/> is <c>null</c>.
        /// </exception>
        public static JsonWriter Create(StringBuilder builder)
        {
            return Create(builder, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with custom formatting.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="settings">Custom settings.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="builder"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        public static JsonWriter Create(StringBuilder builder, JsonWriterSettings settings)
        {
            if (builder == null) {
                throw new ArgumentNullException("builder");
            }
            if (settings == null) {
                throw new ArgumentNullException("settings");
            }

            return Create(new StringWriter(builder), settings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="Stream"/> with the default formatting.
        /// </summary>
        /// <param name="stream">Stream that data will be written to.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="stream"/> is not writable.
        /// </exception>
        public static JsonWriter Create(Stream stream)
        {
            return Create(stream, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="Stream"/> with custom formatting.
        /// </summary>
        /// <param name="stream">Stream that data will be written to.</param>
        /// <param name="settings">Custom settings.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="stream"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="stream"/> is not writable.
        /// </exception>
        public static JsonWriter Create(Stream stream, JsonWriterSettings settings)
        {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanWrite) {
                throw new ArgumentException("Cannot write to stream.", "stream");
            }
            if (settings == null) {
                throw new ArgumentNullException("settings");
            }

            return new JsonWriter(new StreamWriter(stream), settings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with the default formatting.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="textWriter"/> is <c>null</c>.
        /// </exception>
        public static JsonWriter Create(TextWriter textWriter)
        {
            return Create(textWriter, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Creates a new <see cref="JsonWriter"/> instance and write content to the
        /// provided <see cref="StringBuilder"/> with custom formatting.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        /// <param name="settings">Custom settings.</param>
        /// <returns>
        /// New <see cref="JsonWriter"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="textWriter"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        public static JsonWriter Create(TextWriter textWriter, JsonWriterSettings settings)
        {
            if (textWriter == null) {
                throw new ArgumentNullException("textWriter");
            }
            if (settings == null) {
                throw new ArgumentNullException("settings");
            }

            return new JsonWriter(textWriter, settings);
        }

        #endregion


        private readonly TextWriter writer;


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWriter"/> class.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        /// <param name="settings">Custom settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>If <paramref name="textWriter"/> is <c>null</c>.</item>
        /// <item>If <paramref name="settings"/> is <c>null</c>.</item>
        /// </list>
        /// </exception>
        private JsonWriter(TextWriter textWriter, JsonWriterSettings settings)
        {
            if (textWriter == null) {
                throw new ArgumentNullException("textWriter");
            }
            if (settings == null) {
                throw new ArgumentNullException("settings");
            }

            this.writer = textWriter;
            this.Settings = settings;

            settings.MarkReadOnly();

            this.writeContextStack.Push(WriteContext.Root);
        }


        #region Low Level Writing

        private enum WriteContext
        {
            Root,
            Object,
            Array,
        }


        private SimpleStack<WriteContext> writeContextStack = new SimpleStack<WriteContext>();
        private bool isWriteContextEmpty = true;


        /// <summary>
        /// Gets writer settings which are used to control formatting of output. Setting
        /// properties become read-only once assigned to a <see cref="JsonWriter"/>
        /// instance.
        /// </summary>
        public JsonWriterSettings Settings { get; private set; }


        private void WriteIndent()
        {
            if (this.Settings.Indent == true) {
                int count = this.writeContextStack.Count;
                while (--count > 0) {
                    this.writer.Write(this.Settings.IndentChars);
                }
            }
        }

        private void WriteLine()
        {
            if (this.Settings.Indent == true) {
                this.writer.Write(this.Settings.NewLineChars);
            }
        }

        private void WriteSpace()
        {
            if (this.Settings.Indent == true) {
                this.writer.Write(" ");
            }
        }

        private void WriteEscapedLiteral(string value)
        {
            if (value == null) {
                return;
            }

            for (int i = 0; i < value.Length; ++i) {
                char c = value[i];
                switch (c) {
                    case '\"':
                        this.writer.Write("\\\"");
                        break;
                    case '\\':
                        this.writer.Write("\\\\");
                        break;
                    case '/':
                        this.writer.Write("\\/");
                        break;
                    case '\b':
                        this.writer.Write("\\b");
                        break;
                    case '\f':
                        this.writer.Write("\\f");
                        break;
                    case '\n':
                        this.writer.Write("\\n");
                        break;
                    case '\r':
                        this.writer.Write("\\r");
                        break;
                    case '\t':
                        this.writer.Write("\\t");
                        break;
                    default:
                        this.writer.Write(c);
                        break;
                }
            }
        }

        private void DoBeginValue()
        {
            if (!this.isWriteContextEmpty) {
                this.writer.Write(',');
            }

            if (this.writeContextStack.Peek() == WriteContext.Array) {
                this.WriteLine();
                this.WriteIndent();
            }
        }

        private void DoEndValue()
        {
            this.isWriteContextEmpty = false;
        }

        /// <summary>
        /// Write raw JSON value.
        /// </summary>
        /// <remarks>
        /// <para>Whitespace is still automatically added when specified; for instance,
        /// value will be indented if <see cref="JsonWriterSettings.Indent"/> is set to a
        /// value of <c>true</c>.</para>
        /// <para>If <paramref name="content"/> is a value of <c>null</c> then the value
        /// "null" is written to output.</para>
        /// </remarks>
        /// <param name="content">String to be written verbatim.</param>
        private void WriteValueRaw(string content)
        {
            this.DoBeginValue();

            this.writer.Write(content ?? "null");

            this.DoEndValue();
        }

        #endregion


        /// <inheritdoc/>
        public void WriteStartObject(int propertyCount)
        {
            if (propertyCount < 0) {
                throw new ArgumentOutOfRangeException("propertyCount", "Cannot be a negative value.");
            }

            this.WriteStartObject();
        }

        /// <summary>
        /// Writes the start of an object literal.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// {
        ///     "Name": "Bob"
        /// }
        /// ]]></code>
        /// <para>The above object literal is represented by the following writer logic:</para>
        /// <code language="csharp"><![CDATA[
        /// writer.WriteStartObject();
        /// writer.WritePropertyKey("Name");
        /// writer.WriteValue("Bob");
        /// writer.WriteEndObject();
        /// ]]></code>
        /// </example>
        /// <seealso cref="WritePropertyKey(string)"/>
        /// <seealso cref="WriteEndObject()"/>
        public void WriteStartObject()
        {
            this.DoBeginValue();

            this.writer.Write('{');

            this.writeContextStack.Push(WriteContext.Object);
            this.isWriteContextEmpty = true;
        }

        /// <inheritdoc/>
        public void WritePropertyKey(string key)
        {
            this.DoBeginValue();

            this.WriteLine();
            this.WriteIndent();

            this.writer.Write("\"");
            this.WriteEscapedLiteral(key);
            this.writer.Write("\":");

            this.WriteSpace();

            this.isWriteContextEmpty = true;
        }

        /// <inheritdoc/>
        public void WriteEndObject()
        {
            this.writeContextStack.Pop();

            if (!this.isWriteContextEmpty) {
                this.WriteLine();
                this.WriteIndent();
            }

            this.writer.Write('}');

            this.DoEndValue();
        }

        /// <inheritdoc/>
        public void WriteStartArray(int arrayLength)
        {
            if (arrayLength < 0) {
                throw new ArgumentOutOfRangeException("arrayLength", "Cannot be a negative value.");
            }

            this.WriteStartArray();
        }

        /// <summary>
        /// Writes the start of an array literal.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// [
        ///     "Bob",
        ///     "Jessica",
        ///     "Sandra"
        /// ]
        /// ]]></code>
        /// <para>The above array literal is represented by the following writer logic:</para>
        /// <code language="csharp"><![CDATA[
        /// writer.WriteStartArray();
        /// writer.WriteValue("Bob");
        /// writer.WriteValue("Jessica");
        /// writer.WriteValue("Sandra");
        /// writer.WriteEndArray();
        /// ]]></code>
        /// </example>
        /// <seealso cref="WriteEndArray()"/>
        public void WriteStartArray()
        {
            this.DoBeginValue();

            this.writer.Write('[');

            this.writeContextStack.Push(WriteContext.Array);
            this.isWriteContextEmpty = true;
        }

        /// <inheritdoc/>
        public void WriteEndArray()
        {
            this.writeContextStack.Pop();

            if (!this.isWriteContextEmpty) {
                this.WriteLine();
                this.WriteIndent();
            }

            this.writer.Write(']');

            this.DoEndValue();
        }

        /// <inheritdoc/>
        public void WriteNull()
        {
            this.WriteValueRaw("null");
        }

        /// <inheritdoc/>
        public void WriteInteger(long value)
        {
            this.WriteValueRaw(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <inheritdoc/>
        public void WriteDouble(double value)
        {
            this.WriteValueRaw(JsonFormattingUtility.DoubleToString(value));
        }

        /// <inheritdoc/>
        public void WriteString(string value)
        {
            this.DoBeginValue();

            this.writer.Write('"');
            this.WriteEscapedLiteral(value);
            this.writer.Write('"');

            this.DoEndValue();
        }

        /// <inheritdoc/>
        public void WriteBoolean(bool value)
        {
            this.WriteValueRaw(value ? "true" : "false");
        }

        /// <inheritdoc/>
        public void WriteBinary(byte[] value)
        {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            this.WriteStartArray(value.Length);
            for (int i = 0; i < value.Length; ++i) {
                this.WriteInteger(value[i]);
            }
            this.WriteEndArray();
        }

        /// <summary>
        /// Get current state of JSON encoded string which is being written.
        /// </summary>
        /// <returns>
        /// The JSON encoded string when writing using a <see cref="StringWriter"/>.
        /// </returns>
        public override string ToString()
        {
            var stringWriter = this.writer as StringWriter;
            if (stringWriter != null) {
                var sb = stringWriter.GetStringBuilder();
                stringWriter.Flush();

                // Return a value of "null" if output was empty.
                var result = sb.ToString();
                return !string.IsNullOrEmpty(result) ? result : "null";
            }

            return base.ToString();
        }
    }
}
