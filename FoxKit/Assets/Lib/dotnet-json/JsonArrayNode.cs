// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Rotorz.Json
{
    /// <summary>
    /// Node representing a JSON array which can contain zero or more JSON nodes.
    /// </summary>
    /// <example>
    /// <para>Manual construction of an array node:</para>
    /// <code language="csharp"><![CDATA[
    /// var arrayNode = new JsonArrayNode();
    /// arrayNode.Add(new JsonIntegerNode(1));
    /// arrayNode.Add(new JsonIntegerNode(2));
    /// arrayNode.Add(new JsonIntegerNode(3));
    /// ]]></code>
    /// </example>
    public sealed class JsonArrayNode : JsonNode, IList<JsonNode>
    {
        #region Factory Methods

        /// <summary>
        /// Create array node and populate from a native array of values.
        /// </summary>
        /// <remarks>
        /// <para>Array elements are cloned if they are already <see cref="JsonNode"/>
        /// instances.</para>
        /// </remarks>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Native array of objects.</param>
        /// <returns>
        /// New <see cref="JsonArrayNode"/> instance containing zero or more nodes.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="array"/> is <c>null</c>.
        /// </exception>
        public static JsonArrayNode FromArray<T>(T[] array)
        {
            if (array == null) {
                throw new ArgumentNullException("array");
            }

            var node = new JsonArrayNode();
            foreach (T element in array) {
                node.Add(ConvertFrom(element));
            }
            return node;
        }

        /// <summary>
        /// Create array node and populate from a collection of values.
        /// </summary>
        /// <remarks>
        /// <para>Collection entries are cloned if they are already <see cref="JsonNode"/>
        /// instances.</para>
        /// </remarks>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// New <see cref="JsonArrayNode"/> instance containing zero or more nodes.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="collection"/> is <c>null</c>.
        /// </exception>
        public static JsonArrayNode FromCollection(IEnumerable collection)
        {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            var node = new JsonArrayNode();
            foreach (var element in collection) {
                node.Add(ConvertFrom(element));
            }
            return node;
        }

        #endregion


        private readonly List<JsonNode> nodes;


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayNode"/> class.
        /// </summary>
        public JsonArrayNode()
        {
            this.nodes = new List<JsonNode>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayNode"/> class and
        /// populates with the specified <paramref name="length"/> of <c>null</c> entries.
        /// </summary>
        public JsonArrayNode(int length)
        {
            this.nodes = new List<JsonNode>(length);
            while (length-- > 0) {
                this.nodes.Add(null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayNode"/> class and
        /// populates from the specified array of nodes.
        /// </summary>
        /// <param name="nodes">Native array of nodes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="nodes"/> is <c>null</c>.
        /// </exception>
        public JsonArrayNode(JsonNode[] nodes)
        {
            if (nodes == null) {
                throw new ArgumentNullException("nodes");
            }

            this.nodes = new List<JsonNode>();
            foreach (var node in nodes) {
                this.nodes.Add(node);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayNode"/> class and
        /// populates from an enumerable collection of nodes.
        /// </summary>
        /// <param name="collection">Collection of nodes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="collection"/> is <c>null</c>.
        /// </exception>
        public JsonArrayNode(IEnumerable<JsonNode> collection)
        {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            this.nodes = new List<JsonNode>();
            this.nodes.AddRange(collection);
        }


        #region IList<JsonNode> Members

        /// <summary>
        /// Find index of specific node within array.
        /// </summary>
        /// <param name="item">Node instance or a value of <c>null</c>.</param>
        /// <returns>
        /// Zero-based index of first occurrence of input node within array if found;
        /// otherwise, a value of -1.
        /// </returns>
        public int IndexOf(JsonNode item)
        {
            return this.nodes.IndexOf(item);
        }

        /// <summary>
        /// Insert node at specific position within array.
        /// </summary>
        /// <param name="index">Zero-based index of node within array.</param>
        /// <param name="item">New node instance or a value of <c>null</c>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of range.
        /// </exception>
        public void Insert(int index, JsonNode item)
        {
            this.nodes.Insert(index, item);
        }

        /// <summary>
        /// Remove node at specific position within array.
        /// </summary>
        /// <param name="index">Zero-based index of node within array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of range.
        /// </exception>
        public void RemoveAt(int index)
        {
            this.nodes.RemoveAt(index);
        }

        /// <summary>
        /// Lookup node from array at specified index.
        /// </summary>
        /// <param name="index">Zero-based index of node within array.</param>
        /// <returns>
        /// Node instance or a value of <c>null</c>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of range.
        /// </exception>
        public JsonNode this[int index] {
            get { return this.nodes[index]; }
            set { this.nodes[index] = value; }
        }

        #endregion


        #region ICollection<JsonNode> Members

        /// <summary>
        /// Gets count of nodes in array.
        /// </summary>
        public int Count {
            get { return this.nodes.Count; }
        }

        bool ICollection<JsonNode>.IsReadOnly {
            get { return false; }
        }


        /// <summary>
        /// Add node to array.
        /// </summary>
        /// <param name="item">Node or a value of <c>null</c>.</param>
        public void Add(JsonNode item)
        {
            this.nodes.Add(item);
        }

        /// <summary>
        /// Clear all nodes from array.
        /// </summary>
        public void Clear()
        {
            this.nodes.Clear();
        }

        /// <summary>
        /// Determines whether array contains the specified node.
        /// </summary>
        /// <param name="item">Node of which to search for.</param>
        /// <returns>
        /// A value of <c>true</c> if array contains the specified node; otherwise a
        /// value of <c>false</c>.
        /// </returns>
        public bool Contains(JsonNode item)
        {
            return this.nodes.Contains(item);
        }

        /// <summary>
        /// Copy nodes to specified native array.
        /// </summary>
        /// <param name="array">Native array that should be populated with array nodes
        /// from this array.</param>
        /// <param name="arrayIndex">Zero-based index in <paramref name="array"/> at
        /// which copying begins.</param>
        public void CopyTo(JsonNode[] array, int arrayIndex)
        {
            this.nodes.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove node from array if found.
        /// </summary>
        /// <param name="item">Node which is to be removed.</param>
        /// <returns>
        /// A value of <c>true</c> if node was removed; otherwise, a value of <c>false</c>
        /// if array does not actually contain specified node.
        /// </returns>
        public bool Remove(JsonNode item)
        {
            return this.nodes.Remove(item);
        }

        #endregion


        #region IEnumerable<JsonNode> Members

        /// <summary>
        /// Returns an enumerator which iterates through collection of object nodes.
        /// </summary>
        /// <returns>
        /// Enumerator instance which can be used to iterate through array.
        /// </returns>
        public IEnumerator<JsonNode> GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        #endregion


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        #endregion


        /// <inheritdoc/>
        public override JsonNode Clone()
        {
            var clone = new JsonArrayNode();
            foreach (var node in this.nodes) {
                if (node != null) {
                    clone.Add(node.Clone());
                }
                else {
                    clone.Add(null);
                }
            }
            return clone;
        }

        /// <inheritdoc/>
        public override object ConvertTo(Type type)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            if (type.IsArray) {
                var elementType = type.GetElementType();

                var array = Array.CreateInstance(elementType, Count);
                for (int i = 0; i < this.nodes.Count; ++i) {
                    array.SetValue(this.nodes[i].ConvertTo(elementType), i);
                }

                return array;
            }
            else {
                var collectionType = type.GetInterface("ICollection`1");
                if (collectionType != null) {
                    var elementType = collectionType.GetGenericArguments()[0];

                    var collection = Activator.CreateInstance(type);
                    var addMethod = collectionType.GetMethod("Add", new Type[] { elementType });

                    var paramBuffer = new object[1];

                    foreach (var node in this.nodes) {
                        paramBuffer[0] = node.ConvertTo(elementType);
                        addMethod.Invoke(collection, paramBuffer);
                    }

                    return collection;
                }
            }

            throw new InvalidOperationException("Was unable to convert array to type '" + type.FullName + "'.");
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Count == 0) {
                return "[]";
            }
            return base.ToString();
        }

        /// <inheritdoc/>
        public override void Write(IJsonWriter writer)
        {
            writer.WriteStartArray(this.nodes.Count);

            foreach (var node in this.nodes) {
                if (node != null) {
                    node.Write(writer);
                }
                else {
                    writer.WriteNull();
                }
            }

            writer.WriteEndArray();
        }
    }
}
