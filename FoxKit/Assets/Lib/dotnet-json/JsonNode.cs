// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Json.Serialization;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Rotorz.Json
{
    /// <summary>
    /// Base class inherited by all JSON node types.
    /// </summary>
    /// <remarks>
    /// <para>Refer to <see cref="ConvertFrom(object)"/> for some fundamental usage
    /// information.</para>
    /// </remarks>
    /// <seealso cref="JsonObjectNode"/>
    /// <seealso cref="JsonArrayNode"/>
    /// <seealso cref="JsonStringNode"/>
    /// <seealso cref="JsonIntegerNode"/>
    /// <seealso cref="JsonDoubleNode"/>
    /// <seealso cref="JsonBooleanNode"/>
    public abstract class JsonNode : ICloneable
    {
        #region Factory Methods

        /// <summary>
        /// Attempt to create node by parsing JSON encoded string.
        /// </summary>
        /// <param name="json">JSON encoded string.</param>
        /// <returns>
        /// Returns the new <see cref="JsonNode"/> instance. Always returns a value of
        /// <c>null</c> if input JSON encoded string was null or empty.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If an error was encountered whilst attempting to parse JSON encoded string.
        /// This exception typical indicates that input string contains one or more
        /// syntax errors.
        /// </exception>
        public static JsonNode ReadFrom(string json)
        {
            if (string.IsNullOrEmpty(json)) {
                return null;
            }

            using (var reader = new StringReader(json)) {
                return ReadFrom(reader);
            }
        }

        /// <summary>
        /// Attempt to create node by parsing JSON encoded string from a <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// <para>User code should close the provided stream when no longer required
        /// after data has been read; this can be accomplished with the <c>using</c>
        /// construct:</para>
        /// <code language="csharp"><![CDATA[
        /// JsonNode result;
        /// using (var fs = new FileStream(@"C:\TestFile.json", FileMode.Open, FileAccess.Read)) {
        ///     result = JsonNode.FromStream(fs);
        /// }
        /// ]]></code>
        /// </remarks>
        /// <param name="stream">Input stream.</param>
        /// <returns>
        /// Returns the new <see cref="JsonNode"/> instance. Always returns a value of
        /// <c>null</c> if input JSON encoded string was null or empty.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="JsonParserException">
        /// If an error was encountered whilst attempting to parse JSON encoded string.
        /// This exception typical indicates that input string contains one or more
        /// syntax errors.
        /// </exception>
        public static JsonNode ReadFrom(Stream stream)
        {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            return JsonReader.Create(stream).Read();
        }

        /// <summary>
        /// Attempt to create node by parsing JSON encoded string from a <see cref="TextReader"/>.
        /// </summary>
        /// <remarks>
        /// <para>User code should dispose the <see cref="TextReader"/> when no longer
        /// required after data has been read; this can be accomplished with the <c>using</c>
        /// construct:</para>
        /// <code language="csharp"><![CDATA[
        /// JsonNode result;
        /// using (var reader = new StringReader(json)) {
        ///     result = JsonNode.FromReader(reader);
        /// }
        /// ]]></code>
        /// </remarks>
        /// <param name="reader">Text reader.</param>
        /// <returns>
        /// Returns the new <see cref="JsonNode"/> instance. Always returns a value of
        /// <c>null</c> if input JSON encoded string was null or empty.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="reader"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="JsonParserException">
        /// If an error was encountered whilst attempting to parse JSON encoded string.
        /// This exception typical indicates that input string contains one or more
        /// syntax errors.
        /// </exception>
        public static JsonNode ReadFrom(TextReader reader)
        {
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            return JsonReader.Create(reader).Read();
        }

        /// <summary>
        /// Attempt to create JSON node representation from given input.
        /// </summary>
        /// <remarks>
        /// <para>This method uses type information from input to determine which type of
        /// nodes seem best suited. Common basic data types are supported along with
        /// objects, structures, lists, native arrays and even dictionaries. Dictionary
        /// support is limited to those with string type keys (<c>Dictionary&lt;string, YourType&gt;></c>).</para>
        /// <para>Input value is simply cloned if it is already a <see cref="JsonNode"/>.</para>
        /// </remarks>
        /// <example>
        /// <para>Create JSON object from a generic dictionary:</para>
        /// <code language="csharp"><![CDATA[
        /// // Prepare an example data structure.
        /// var lookupTable = new Dictionary<string, int>();
        /// lookupTable["Player"] = 42;
        /// lookupTable["Boss1"] = 72;
        /// lookupTable["Whale"] = 128;
        ///
        /// // Convert example data structure into a JSON object.
        /// var jsonObject = JsonNode.FromObject(lookupTable);
        ///
        /// // Read node from JSON object.
        /// var playerNode = jsonObject["Player"] as JsonLongNode;
        /// Debug.Log(playerNode.Value); // 42
        /// ]]></code>
        /// <para>Once you have a node representation of your data you can then proceed
        /// to output this to a JSON encoded text file:</para>
        /// <code language="csharp"><![CDATA[
        /// File.WriteAllText(outputPath, jsonObject.ToString());
        /// ]]></code>
        /// <para>The resulting text file would then look something like this:</para>
        /// <code language="json"><![CDATA[
        /// {
        ///     "Player": 42,
        ///     "Boss1": 72,
        ///     "Whale": 128
        /// }
        /// ]]></code>
        /// </example>
        /// <param name="value">Input value, array, collection, object instance, etc.</param>
        /// <returns>
        /// The new <see cref="JsonNode"/> instance; or a value of <c>null</c> if input
        /// was itself a value of <c>null</c>.
        /// </returns>
        /// <exception cref="System.Exception">
        /// If a problem was encountered whilst attempting to create node representation
        /// from input value.
        /// </exception>
        /// <seealso cref="ConvertTo{T}()"/>
        /// <seealso cref="ConvertTo(Type)"/>
        public static JsonNode ConvertFrom(object value)
        {
            if (value == null) {
                return null;
            }

            var valueNode = value as JsonNode;
            if (valueNode != null) {
                return valueNode.Clone();
            }

            var type = value.GetType();
            var metaType = MetaType.FromType(type);

            switch (metaType.TargetNodeType) {
                case MetaType.NodeType.Integer:
                    if (Type.GetTypeCode(type) == TypeCode.UInt64) {
                        return new JsonIntegerNode((long)Convert.ToUInt64(value, CultureInfo.InvariantCulture));
                    }
                    else {
                        return new JsonIntegerNode(Convert.ToInt64(value, CultureInfo.InvariantCulture));
                    }

                case MetaType.NodeType.Double:
                    return new JsonDoubleNode(Convert.ToDouble(value, CultureInfo.InvariantCulture));

                case MetaType.NodeType.Boolean:
                    return new JsonBooleanNode(Convert.ToBoolean(value, CultureInfo.InvariantCulture));

                case MetaType.NodeType.String:
                    return new JsonStringNode(Convert.ToString(value, CultureInfo.InvariantCulture));

                case MetaType.NodeType.Array:
                    if (type.IsArray) {
                        return JsonArrayNode.FromArray((object[])value);
                    }
                    else {
                        return JsonArrayNode.FromCollection((IEnumerable)value);
                    }

                case MetaType.NodeType.Object:
                    if (metaType.IsDictionaryStyleCollection) {
                        return FromDictionaryStyleCollection((ICollection)value, metaType);
                    }
                    else {
                        return JsonObjectNode.FromInstance(value);
                    }

                default:
                    throw new InvalidOperationException("Was unable to convert input value.");
            }
        }

        private static JsonObjectNode FromDictionaryStyleCollection(ICollection collection, MetaType metaType)
        {
            var node = new JsonObjectNode();
            if (collection.Count > 0) {
                foreach (var pair in collection) {
                    string key = (string)metaType.KeyPropertyInfo.GetValue(pair, null);
                    node[key] = ConvertFrom(metaType.ValuePropertyInfo.GetValue(pair, null));
                }
            }
            return node;
        }

        #endregion


        #region ICloneable Members

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Clone this node and create a deep copy of all descending nodes.
        /// </summary>
        /// <example>
        /// <para>Create deep copy of a JSON object for alteration:</para>
        /// <code language="csharp"><![CDATA[
        /// // Create new player card object.
        /// var card = new JsonObjectNode();
        /// card["Name"] = new JsonStringNode("Jessica");
        /// card["HP"] = new JsonLongNode(100);
        ///
        /// // Clone existing player card and modify.
        /// var copy = card.Clone();
        /// copy["Name"] = new JsonStringNode("Borris");
        ///
        /// // Each card now has a unique player name!
        /// ]]></code>
        /// </example>
        /// <returns>
        /// The new <see cref="JsonNode"/> instance.
        /// </returns>
        public abstract JsonNode Clone();

        #endregion


        /// <summary>
        /// Attempt to convert node to an object of the specified type.
        /// </summary>
        /// <param name="type">Target object or value type.</param>
        /// <returns>
        /// Object of specified type.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="type"/> was a value of <c>null</c>.
        /// </exception>
        /// <exception cref="System.Exception">
        /// If a problem was encountered whilst attempting to create object of specified
        /// type from node representation.
        /// </exception>
        /// <seealso cref="ConvertTo{T}()"/>
        /// <seealso cref="ConvertFrom(object)"/>
        public abstract object ConvertTo(Type type);

        /// <summary>
        /// Attempt to convert node to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">Target object or value type.</typeparam>
        /// <returns>
        /// Object of specified type.
        /// </returns>
        /// <exception cref="System.Exception">
        /// If a problem was encountered whilst attempting to create object of specified
        /// type from node representation.
        /// </exception>
        /// <seealso cref="ConvertTo(Type)"/>
        /// <seealso cref="ConvertFrom(object)"/>
        public T ConvertTo<T>()
        {
            return (T)this.ConvertTo(typeof(T));
        }

        /// <summary>
        /// Convert node to JSON encoded string.
        /// </summary>
        /// <remarks>
        /// <para>Resulting string includes indentation and whitespace; though this
        /// behaviour can be customized by supplying a <see cref="JsonWriterSettings"/>
        /// object to the other overload of this method.</para>
        /// </remarks>
        /// <returns>
        /// The JSON encoded string.
        /// </returns>
        /// <seealso cref="JsonUtility.ToJsonString(JsonNode)"/>
        /// <seealso cref="JsonUtility.ToJsonString(JsonNode, JsonWriterSettings)"/>
        /// <seealso cref="Write(IJsonWriter)"/>
        public override string ToString()
        {
            return JsonUtility.ToJsonString(this, JsonWriterSettings.DefaultSettings);
        }

        /// <summary>
        /// Write node data using the supplied <see cref="IJsonWriter"/>.
        /// </summary>
        /// <param name="writer">Object for writing node data to.</param>
        /// <seealso cref="ToString()"/>
        /// <seealso cref="JsonUtility.ToJsonString(JsonNode)"/>
        /// <seealso cref="JsonUtility.ToJsonString(JsonNode, JsonWriterSettings)"/>
        public abstract void Write(IJsonWriter writer);
    }
}
