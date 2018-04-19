// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rotorz.Json
{
    /// <summary>
    /// Node representing a JSON object with named properties.
    /// </summary>
    /// <example>
    /// <para>Manual construction of an object node:</para>
    /// <code language="csharp"><![CDATA[
    /// var objectNode = new JsonObjectNode();
    /// objectNode["A"] = new JsonIntegerNode(1);
    /// objectNode["B"] = new JsonIntegerNode(2);
    /// objectNode["C"] = new JsonIntegerNode(3);
    /// ]]></code>
    /// </example>
    public sealed class JsonObjectNode : JsonNode, IDictionary<string, JsonNode>
    {
        #region Factory Methods

        /// <summary>
        /// Attempt to create object node from a .NET object.
        /// </summary>
        /// <param name="instance">Input object.</param>
        /// <returns>
        /// The new <see cref="JsonObjectNode"/> instance; otherwise a value of <c>null</c>
        /// if input was <c>null</c>.
        /// </returns>
        /// <exception cref="System.Exception">
        /// If error is encountered whilst creating object node. Errors are likely to
        /// occur when unable to convert property values into JSON nodes.
        /// </exception>
        internal static JsonObjectNode FromInstance(object instance)
        {
            if (instance == null) {
                return null;
            }

            var node = new JsonObjectNode();
            var metaType = MetaType.FromType(instance.GetType());

            metaType.InvokeOnSerializing(instance, default(StreamingContext));

            foreach (var member in metaType.SerializableMembers) {
                object value;
                if (member.Info.MemberType == MemberTypes.Field) {
                    var mi = (FieldInfo)member.Info;
                    value = mi.GetValue(instance);
                }
                else {
                    var pi = (PropertyInfo)member.Info;
                    value = pi.GetValue(instance, null);
                }
                node[member.ResolvedName] = ConvertFrom(value);
            }

            metaType.InvokeOnSerialized(instance, default(StreamingContext));

            return node;
        }

        /// <summary>
        /// Create object node from a generic dictionary.
        /// </summary>
        /// <remarks>
        /// <para>Property values are cloned if they are already <see cref="JsonNode"/>
        /// instances.</para>
        /// </remarks>
        /// <typeparam name="TValue">Type of property value.</typeparam>
        /// <param name="dictionary">Input dictionary.</param>
        /// <returns>
        /// The new <see cref="JsonObjectNode"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="dictionary"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.Exception">
        /// If error is encountered whilst creating object node. Errors are likely to
        /// occur when unable to convert property values into JSON nodes.
        /// </exception>
        public static JsonObjectNode FromDictionary<TValue>(IDictionary<string, TValue> dictionary)
        {
            if (dictionary == null) {
                throw new ArgumentNullException("dictionary");
            }

            var node = new JsonObjectNode();

            foreach (var property in dictionary) {
                node[property.Key] = ConvertFrom(property.Value);
            }

            return node;
        }

        #endregion


        private readonly Dictionary<string, JsonNode> properties = new Dictionary<string, JsonNode>();


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObjectNode"/> class.
        /// </summary>
        public JsonObjectNode()
        {
        }


        #region IDictionary<string, JsonNode> Members

        /// <summary>
        /// Add property to object.
        /// </summary>
        /// <param name="key">Unique property key.</param>
        /// <param name="value">Property value node or a value of <c>null</c>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="key"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If a property with the same key already exists within object.
        /// </exception>
        public void Add(string key, JsonNode value)
        {
            this.properties.Add(key, value);
        }

        /// <summary>
        /// Gets a value indicating whether object node contains a property with
        /// the specified key.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <returns>
        /// A value of <c>true</c> if property exists with the specified key; otherwise,
        /// a value of <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.properties.ContainsKey(key);
        }

        /// <summary>
        /// Remove property with the specified key if found.
        /// </summary>
        /// <param name="key">Key of property which is to be removed.</param>
        /// <returns>
        /// A value of <c>true</c> if property was removed; otherwise, a value of
        /// <c>false</c> of object does not actually contain the specified property.
        /// </returns>
        public bool Remove(string key)
        {
            return this.properties.Remove(key);
        }

        /// <summary>
        /// Try to get value of property with the specified key.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <param name="value">Method outputs property value when property exists with
        /// the specified key; otherwise outputs a value of <c>null</c>.</param>
        /// <returns>
        /// A value of <c>true</c> if property value was retrieved; otherwise, a value of
        /// <c>false</c> indicating that property does not exist.
        /// </returns>
        public bool TryGetValue(string key, out JsonNode value)
        {
            return this.properties.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets collection of property keys.
        /// </summary>
        public ICollection<string> Keys {
            get { return this.properties.Keys; }
        }

        /// <summary>
        /// Gets collection of property values.
        /// </summary>
        public ICollection<JsonNode> Values {
            get { return this.properties.Values; }
        }

        /// <summary>
        /// Lookup value of property with the specified key.
        /// </summary>
        /// <param name="key">Property key.</param>
        /// <returns>
        /// Value of property when property exists; otherwise, a value of <c>null</c>
        /// indicating that property does not exist.
        /// </returns>
        public JsonNode this[string key] {
            get {
                JsonNode node;
                this.properties.TryGetValue(key, out node);
                return node;
            }
            set {
                this.properties[key] = value;
            }
        }

        #endregion


        #region ICollection<KeyValuePair<string, JsonNode>> Members

        /// <summary>
        /// Gets count of properties in object.
        /// </summary>
        public int Count {
            get { return this.properties.Count; }
        }

        bool ICollection<KeyValuePair<string, JsonNode>>.IsReadOnly {
            get {
                var collection = (ICollection<KeyValuePair<string, JsonNode>>)this.properties;
                return collection.IsReadOnly;
            }
        }


        void ICollection<KeyValuePair<string, JsonNode>>.Add(KeyValuePair<string, JsonNode> item)
        {
            this.properties.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clear all properties from object.
        /// </summary>
        public void Clear()
        {
            this.properties.Clear();
        }

        bool ICollection<KeyValuePair<string, JsonNode>>.Contains(KeyValuePair<string, JsonNode> item)
        {
            return this.properties.ContainsKey(item.Key);
        }

        void ICollection<KeyValuePair<string, JsonNode>>.CopyTo(KeyValuePair<string, JsonNode>[] array, int arrayIndex)
        {
            var collection = (ICollection<KeyValuePair<string, JsonNode>>)this.properties;
            collection.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, JsonNode>>.Remove(KeyValuePair<string, JsonNode> item)
        {
            var collection = (ICollection<KeyValuePair<string, JsonNode>>)this.properties;
            return collection.Remove(item);
        }

        #endregion


        #region IEnumerable<KeyValuePair<string,JsonNode>> Members

        /// <summary>
        /// Returns an enumerator which iterates through collection of object property
        /// key and value pairs.
        /// </summary>
        /// <returns>
        /// Enumerator instance which can be used to iterate through collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, JsonNode>> GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        #endregion


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        #endregion


        /// <inheritdoc/>
        public override JsonNode Clone()
        {
            var node = new JsonObjectNode();
            foreach (var property in this.properties) {
                node[property.Key] = property.Value != null
                    ? property.Value.Clone()
                    : null;
            }
            return node;
        }

        /// <inheritdoc/>
        public override object ConvertTo(Type type)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            var obj = Activator.CreateInstance(type);

            var dictionary = obj as IDictionary;
            if (dictionary != null) {
                // Read properties into dictionary.
                var valueType = type.GetGenericArguments()[1];
                foreach (var property in this.properties) {
                    if (property.Value != null) {
                        dictionary[property.Key] = property.Value.ConvertTo(valueType);
                    }
                    else {
                        dictionary[property.Key] = null;
                    }
                }
            }
            else {
                var metaType = MetaType.FromType(type);
                metaType.InvokeOnDeserializing(obj, default(StreamingContext));

                // Read properties into object instance.
                foreach (var member in metaType.SerializableMembers) {
                    var valueNode = this[member.ResolvedName];
                    if (valueNode == null) {
                        continue;
                    }

                    if (member.Info.MemberType == MemberTypes.Field) {
                        var fi = (FieldInfo)member.Info;
                        fi.SetValue(obj, valueNode.ConvertTo(fi.FieldType));
                    }
                    else {
                        var pi = (PropertyInfo)member.Info;
                        pi.SetValue(obj, valueNode.ConvertTo(pi.PropertyType), null);
                    }
                }

                metaType.InvokeOnDeserialized(obj, default(StreamingContext));
            }

            return obj;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Count == 0) {
                return "{}";
            }
            return base.ToString();
        }

        /// <inheritdoc/>
        public override void Write(IJsonWriter writer)
        {
            writer.WriteStartObject(this.properties.Count);

            foreach (var property in this.properties) {
                writer.WritePropertyKey(property.Key);

                if (property.Value != null) {
                    property.Value.Write(writer);
                }
                else {
                    writer.WriteNull();
                }
            }

            writer.WriteEndObject();
        }
    }
}
