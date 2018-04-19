// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json
{
    /// <summary>
    /// Behaviour and output of <see cref="JsonWriter"/> can be customized by with custom
    /// settings by providing an instance of this class.
    /// </summary>
    public sealed class JsonWriterSettings
    {
        internal static JsonWriterSettings DefaultSettings { get; private set; }


        private bool indent;
        private string indentChars;
        private string newlineChars;


        static JsonWriterSettings()
        {
            DefaultSettings = new JsonWriterSettings();
            DefaultSettings.MarkReadOnly();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWriterSettings"/> class with default values.
        /// </summary>
        public JsonWriterSettings()
        {
            this.Reset();
        }


        /// <summary>
        /// Gets a value indicating whether the <see cref="JsonWriterSettings"/> have
        /// been marked as read-only.
        /// </summary>
        public bool IsReadOnly { get; private set; }


        /// <summary>
        /// Marks the <see cref="JsonWriterSettings"/> as read-only.
        /// </summary>
        public void MarkReadOnly()
        {
            this.IsReadOnly = true;
        }

        /// <summary>
        /// Throws an exception if the <see cref="JsonWriterSettings"/> have been marked
        /// as read-only. Properties can use this to assert prior to mutating properties.
        /// </summary>
        private void CheckReadOnly()
        {
            if (this.IsReadOnly) {
                throw new JsonException("Cannot modify property because settings are now read-only.");
            }
        }


        /// <summary>
        /// Gets or sets whether nested values should be indented within output.
        /// </summary>
        /// <exception cref="JsonException">
        /// If attempting to modify property after settings object has already been
        /// provided to a <see cref="JsonWriter"/> for usage.
        /// </exception>
        /// <seealso cref="IndentChars"/>
        public bool Indent {
            get { return this.indent; }
            set {
                this.CheckReadOnly();
                this.indent = value;
            }
        }

        /// <summary>
        /// Gets or sets string of characters which to used when indenting nested values
        /// within output.
        /// </summary>
        /// <remarks>
        /// <para>This property is only applicable when <see cref="Indent"/> is set to a
        /// value of <c>true</c>.</para>
        /// </remarks>
        /// <exception cref="JsonException">
        /// If attempting to modify property after settings object has already been
        /// provided to a <see cref="JsonWriter"/> for usage.
        /// </exception>
        /// <seealso cref="Indent"/>
        public string IndentChars {
            get { return this.indentChars; }
            set {
                this.CheckReadOnly();
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                this.indentChars = value;
            }
        }

        /// <summary>
        /// Gets or sets string of characters to use when adding new lines.
        /// </summary>
        /// <exception cref="JsonException">
        /// If attempting to modify property after settings object has already been
        /// provided to a <see cref="JsonWriter"/> for usage.
        /// </exception>
        public string NewLineChars {
            get { return this.newlineChars; }
            set {
                this.CheckReadOnly();
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                this.newlineChars = value;
            }
        }


        /// <summary>
        /// Reset to default setting values.
        /// </summary>
        /// <exception cref="JsonException">
        /// If attempting to modify property after settings object has already been
        /// provided to a <see cref="JsonWriter"/> for usage.
        /// </exception>
        public void Reset()
        {
            this.CheckReadOnly();

            this.indent = true;
            this.indentChars = "\t";
            this.newlineChars = "\r\n";
        }
    }
}
