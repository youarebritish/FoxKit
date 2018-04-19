// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace Rotorz.Json
{
    /// <summary>
    /// Interface for writing <see cref="JsonNode"/> instances into some persisted state
    /// such as a JSON encoded file.
    /// </summary>
    public interface IJsonWriter
    {
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
        /// writer.WriteStartObject(1);
        /// writer.WritePropertyKey("Name");
        /// writer.WriteValue("Bob");
        /// writer.WriteEndObject();
        /// ]]></code>
        /// </example>
        /// <param name="propertyCount">Count of properties in object literal.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="propertyCount"/> is a negative value.
        /// </exception>
        /// <seealso cref="WritePropertyKey(string)"/>
        /// <seealso cref="WriteEndObject()"/>
        void WriteStartObject(int propertyCount);

        /// <summary>
        /// Writes the key for a property inside an object literal.
        /// </summary>
        /// <remarks>
        /// <para>Any special characters inside the <paramref name="key"/> are
        /// automatically escaped.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// {
        ///     "Name": "Bob"
        /// }
        /// ]]></code>
        /// <para>The above object literal is represented by the following writer logic:</para>
        /// <code language="csharp"><![CDATA[
        /// writer.WriteStartObject(1);
        /// writer.WritePropertyKey("Name");
        /// writer.WriteValue("Bob");
        /// writer.WriteEndObject();
        /// ]]></code>
        /// </example>
        /// <param name="key">Key value.</param>
        /// <seealso cref="WriteStartObject(int)"/>
        /// <seealso cref="WritePropertyKey(string)"/>
        void WritePropertyKey(string key);

        /// <summary>
        /// Writes the end of an object literal.
        /// </summary>
        /// <seealso cref="WriteStartObject(int)"/>
        /// <seealso cref="WritePropertyKey(string)"/>
        void WriteEndObject();

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
        /// writer.WriteStartArray(3);
        /// writer.WriteValue("Bob");
        /// writer.WriteValue("Jessica");
        /// writer.WriteValue("Sandra");
        /// writer.WriteEndArray();
        /// ]]></code>
        /// </example>
        /// <param name="arrayLength">Length of the array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="arrayLength"/> is a negative value.
        /// </exception>
        /// <seealso cref="WriteEndArray()"/>
        void WriteStartArray(int arrayLength);

        /// <summary>
        /// Writes the end of an array literal.
        /// </summary>
        /// <seealso cref="WriteStartArray(int)"/>
        void WriteEndArray();

        /// <summary>
        /// Writes a value of <c>null</c>.
        /// </summary>
        void WriteNull();

        /// <summary>
        /// Writes an integer value.
        /// </summary>
        /// <param name="value">Value.</param>
        void WriteInteger(long value);

        /// <summary>
        /// Writes a double-precision floating point value.
        /// </summary>
        /// <param name="value">Value.</param>
        void WriteDouble(double value);

        /// <summary>
        /// Writes a string literal; special characters are automatically escaped.
        /// </summary>
        /// <remarks>
        /// <para>Should write an empty string if <paramref name="value"/> is <c>null</c>.</para>
        /// </remarks>
        /// <param name="value">Content for string.</param>
        void WriteString(string value);

        /// <summary>
        /// Writes a boolean value of <c>true</c> or <c>false</c>.
        /// </summary>
        /// <param name="value">Content for string.</param>
        void WriteBoolean(bool value);

        /// <summary>
        /// Writes an array of binary data.
        /// </summary>
        /// <param name="value">Array of zero or more bytes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="value"/> is <c>null</c>.
        /// </exception>
        void WriteBinary(byte[] value);
    }
}
