// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json
{
    /// <summary>
    /// Use this attribute to mark a property or a private field for serialization when
    /// composing a setting from a custom class or structure type.
    /// </summary>
    /// <example>
    /// <para>Define class with custom serialization for JSON:</para>
    /// <code language="csharp"><![CDATA[
    /// public class CustomJsonType
    /// {
    ///     // Public fields are automatically serialized:
    ///     public bool shouldEnableSuperPowers;
    ///
    ///
    ///     // Serialize field for read-only name:
    ///     [JsonProperty("Name")]
    ///     private string name;
    ///
    ///     public string Name {
    ///         get { return this.name; }
    ///     }
    ///
    ///
    ///     // Serialize property for read-and-write:
    ///     private int favouriteNumber = 42;
    ///
    ///     [JsonProperty]
    ///     public int FavouriteNumber {
    ///         get { return this.favouriteNumber; }
    ///         set { this.favouriteNumber = value; }
    ///     }
    ///
    ///
    ///     // Auto-properties can also be serialized:
    ///     [JsonProperty]
    ///     public int Score { get; private set; }
    /// }
    /// ]]></code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initialize new <see cref="JsonPropertyAttribute"/> instance and assume actual
        /// member name for serialization.
        /// </summary>
        public JsonPropertyAttribute()
        {
        }

        /// <summary>
        /// Initialize new <see cref="JsonPropertyAttribute"/> instance using custom
        /// property name instead of actual member name for serialization.
        /// </summary>
        /// <param name="propertyName">Custom property name for serialization.</param>
        public JsonPropertyAttribute(string propertyName)
        {
            this.Name = propertyName;
        }


        /// <summary>
        /// Gets custom name for field or property which will be used instead of actual
        /// member name.
        /// </summary>
        public string Name { get; private set; }
    }
}
