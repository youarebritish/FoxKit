// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json
{
    /// <summary>
    /// Node holding a boolean value of <c>true</c> or <c>false</c>.
    /// </summary>
    public sealed class JsonBooleanNode : JsonNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBooleanNode"/> class with a
        /// value of <c>false</c>.
        /// </summary>
        public JsonBooleanNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBooleanNode"/> class with
        /// the specified value of <c>true</c> or <c>false</c>.
        /// </summary>
        /// <param name="value">Initial value of node.</param>
        public JsonBooleanNode(bool value)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        public bool Value { get; set; }


        /// <inheritdoc/>
        public override JsonNode Clone()
        {
            return new JsonBooleanNode(this.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Value ? "true" : "false";
        }

        /// <inheritdoc/>
        public override object ConvertTo(Type type)
        {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            return Convert.ChangeType(this.Value, type);
        }

        /// <inheritdoc/>
        public override void Write(IJsonWriter writer)
        {
            writer.WriteBoolean(this.Value);
        }
    }
}
