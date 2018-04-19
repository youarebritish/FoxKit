// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json
{
    /// <summary>
    /// Node holding a string value.
    /// </summary>
    /// <remarks>
    /// <para>String node cannot contain a value of <c>null</c>; instead an empty string
    /// is automatically assumed if a value of <c>null</c> is provided.</para>
    /// </remarks>
    public sealed class JsonStringNode : JsonNode
    {
        private string value;


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringNode"/> class with an
        /// empty string.
        /// </summary>
        public JsonStringNode() : this("")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringNode"/> class with the
        /// specified string.
        /// </summary>
        /// <param name="value">Initial value of node.</param>
        public JsonStringNode(string value)
        {
            this.Value = value;
        }


        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        /// <remarks>
        /// <para>It is not possible to assign a value of <c>null</c> to a string node;
        /// instead an empty string is automatically assumed.</para>
        /// </remarks>
        public string Value {
            get { return this.value; }
            set { this.value = value != null ? value : ""; }
        }


        /// <inheritdoc/>
        public override JsonNode Clone()
        {
            return new JsonStringNode(this.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Value == ""
                ? "\"\""
                : base.ToString();
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
            writer.WriteString(this.Value);
        }
    }
}
