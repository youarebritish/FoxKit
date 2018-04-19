// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json
{
    /// <summary>
    /// Node holding double precision floating point value.
    /// </summary>
    public sealed class JsonDoubleNode : JsonNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDoubleNode"/> class with a
        /// value of zero.
        /// </summary>
        public JsonDoubleNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDoubleNode"/> class with the
        /// specified double precision numeric value.
        /// </summary>
        /// <param name="value">Initial value of node.</param>
        public JsonDoubleNode(double value)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        public double Value { get; set; }


        /// <inheritdoc/>
        public override JsonNode Clone()
        {
            return new JsonDoubleNode(this.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonFormattingUtility.DoubleToString(this.Value);
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
            writer.WriteDouble(this.Value);
        }
    }
}
