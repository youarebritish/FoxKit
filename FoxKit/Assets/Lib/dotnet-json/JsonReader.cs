// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Rotorz.Json
{
    /// <summary>
    /// Reads zero or more <see cref="JsonNode"/> instances from some stream of JSON
    /// encoded text.
    /// </summary>
    /// <remarks>
    /// <para>This class was implemented from the specification presented on the
    /// <a href="http://json.org">http://json.org</a> website.</para>
    /// <para>One interesting thing about this specification is that it doesn't identify
    /// a standardized way to represent floating-point values of NaN, -Infinity and
    /// Infinity.</para>
    /// <para>According to various forum and Q&amp;A postings on the Internet, a number
    /// of security issues arise when using JavaScript compatible constant values because
    /// in some cases hackers are able to inject malicious code into the application.</para>
    /// <para>To avoid this vulnerability such values are encoded as simple strings which
    /// can then be detected when deserializing JSON encoded data. Fortunately
    /// <c>System.Convert.ChangeType</c> can be used to deal with this implementation
    /// specific.</para>
    /// </remarks>
    internal sealed class JsonReader
    {
        #region Factory Methods

        /// <summary>
        /// Creates a new <see cref="JsonReader"/> instance from a stream.
        /// </summary>
        /// <remarks>
        /// <para>Remember to close the provided <see cref="Stream"/> when it is no
        /// longer required after invoking this method.</para>
        /// </remarks>
        /// <param name="stream">Stream.</param>
        /// <returns>
        /// The new <see cref="JsonReader"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="stream"/> is not readable.
        /// </exception>
        /// <seealso cref="Read()"/>
        public static JsonReader Create(Stream stream)
        {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanRead) {
                throw new ArgumentException("Cannot read from stream.", "stream");
            }

            return Create(new StreamReader(stream));
        }

        /// <summary>
        /// Creates a new <see cref="JsonReader"/> instance from a text reader. This
        /// allows JSON encoded text to be parsed from a variety of sources including
        /// strings, files, etc.
        /// </summary>
        /// <remarks>
        /// <para>Remember to dispose the provided <see cref="TextReader"/> when it is no
        /// longer required after invoking this method.</para>
        /// </remarks>
        /// <param name="reader">Text reader.</param>
        /// <returns>
        /// The new <see cref="JsonReader"/> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="reader"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="Read()"/>
        public static JsonReader Create(TextReader reader)
        {
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            return new JsonReader(reader);
        }

        #endregion


        // Maximum lookahead must be large enough to hold the largest value which can be
        // matched when parsing JSON. At the moment this is a value of 6 since the
        // unicode character escape sequence is "\u####" (6 characters).
        private const int MaximumLookahead = 6;


        private readonly TextReader jsonReader;

        private StringBuilder stringLiteral = new StringBuilder();
        private StringBuilder unicodeSequence = new StringBuilder();


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonReader"/> class.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="reader"/> is <c>null</c>.
        /// </exception>
        private JsonReader(TextReader reader)
        {
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            this.jsonReader = reader;

            this.ReadBuffer();
            this.SkipWhitespace();
        }


        #region Text Reader

        /// <summary>
        /// Indicates phase of reading JSON encoded text.
        /// </summary>
        private enum ReadPhase
        {
            /// <summary>
            /// Reading from input stream using text reader.
            /// </summary>
            ReadingStream,

            /// <summary>
            /// Has reached end of input stream using text reader but buffered input
            /// still needs to be parsed.
            /// </summary>
            HasReachedEndOfStream,

            /// <summary>
            /// Has reached end of input; any further requests will yield '\0'.
            /// </summary>
            HasReachedEndOfInput,
        }


        // Buffer holds a maximum of 2048 characters from input stream.
        private char[] buffer;
        // Indicates current size of buffer (used size).
        private int bufferSize;
        // Current position in `_buffer`.
        private int bufferPos;

        // Current reading phase of parser.
        private ReadPhase phase = ReadPhase.ReadingStream;

        private bool hasReachedEnd;

        // Current line number in original input.
        private int lineNumber = 1;
        // Current position in line of original input.
        private int linePosition = 0;

        private bool initLineEnding;
        private char lineEnding;
        private int lineEndingLength = 1;


        /// <summary>
        /// Gets a value indicating whether end of input has been reached.
        /// </summary>
        private bool HasReachedEnd {
            get { return this.hasReachedEnd || this.Peek() == '\0'; }
        }


        private void ReadBuffer()
        {
            if (this.phase == ReadPhase.HasReachedEndOfInput) {
                throw new InvalidOperationException("Attempting to read buffer after end of input has been reached.");
            }

            char[] buffer = this.buffer;
            int trailingCount;

            if (buffer == null) {
                // Create buffer for reading input.
                buffer = this.buffer = new char[2048];
                trailingCount = 0;
            }
            else {
                // Move any trailing unused characters to start of buffer.
                trailingCount = this.bufferSize - this.bufferPos;
                for (int i = 0; i < trailingCount; ++i) {
                    buffer[i] = buffer[this.bufferPos + i];
                }

                // Zero-out any excess characters.
                if (this.phase == ReadPhase.HasReachedEndOfStream) {
                    for (int i = trailingCount; i < buffer.Length; ++i) {
                        buffer[i] = default(char);
                    }

                    this.phase = ReadPhase.HasReachedEndOfInput;
                }
            }

            if (this.phase == ReadPhase.ReadingStream) {
                this.bufferSize = trailingCount + this.jsonReader.ReadBlock(buffer, trailingCount, buffer.Length - trailingCount);

                // Has end of input stream been reached?
                if (this.bufferSize < buffer.Length) {
                    this.phase = ReadPhase.HasReachedEndOfStream;

                    // Zero-out any excess characters.
                    for (int i = this.bufferSize; i < buffer.Length; ++i) {
                        buffer[i] = default(char);
                    }
                }
            }

            if (!this.initLineEnding) {
                // Use either '\r' or '\n' to quickly detect line endings for maintaining
                // line number and position. Initially we do not know which type of line
                // ending characters are being used. If a file contains mixed styles then
                // line number and position feedback will be inaccurate when syntax
                // errors are encountered.
                for (int i = 0; i < buffer.Length - 1; ++i) {
                    char c = buffer[i];
                    if (c == '\r' || c == '\n') {
                        this.initLineEnding = true;
                        this.lineEnding = c;
                        if (c == '\r' && buffer[i + 1] == '\n') {
                            this.lineEndingLength = 2;
                        }
                        break;
                    }
                }
            }

            this.bufferPos = 0;
        }

        /// <summary>
        /// Peeks at next character in buffer but does not advance buffer position.
        /// </summary>
        /// <returns>
        /// The next character if further input remains; otherwise, a value of '\0' is
        /// returned.
        /// </returns>
        /// <seealso cref="Peek(int)"/>
        /// <seealso cref="Accept(int)"/>
        private char Peek()
        {
            return this.buffer[this.bufferPos];
        }

        /// <summary>
        /// Peeks at nth next character in buffer but does not advance buffer position.
        /// </summary>
        /// <param name="offset">Zero-based offset at which to peek. Specifying a value
        /// of 1 is no different to using <see cref="Peek()"/> instead.</param>
        /// <returns>
        /// The nth next character if further input remains; otherwise, a value of '\0'
        /// is returned.
        /// </returns>
        /// <seealso cref="Peek()"/>
        /// <seealso cref="Accept(int)"/>
        private char Peek(int offset)
        {
            return this.buffer[this.bufferPos + offset];
        }

        private char ReadChar()
        {
            int remainingCharsInBuffer = this.bufferSize - this.bufferPos;
            if (remainingCharsInBuffer < MaximumLookahead && this.phase != ReadPhase.HasReachedEndOfInput) {
                // Insufficient characters for lookahead, continue reading input buffer.
                this.ReadBuffer();
            }

            // Has end of input been reached?
            if (this.bufferPos >= this.bufferSize) {
                this.hasReachedEnd = true;
                return default(char);
            }

            char c = this.Peek();
            ++this.bufferPos;
            ++this.linePosition;

            if (this.lineEnding == c) {
                ++this.lineNumber;
                this.linePosition = 1 - this.lineEndingLength;
            }

            return c;
        }

        /// <summary>
        /// Accept one or more characters from input and advanced to next position in
        /// buffer.
        /// </summary>
        /// <param name="count">Number of input characters to accept.</param>
        /// <seealso cref="Peek()"/>
        /// <seealso cref="Peek(int)"/>
        private void Accept(int count = 1)
        {
            while (count-- > 0) {
                this.ReadChar();
            }
        }

        #endregion


        /// <summary>
        /// Skips whitespace by accepting all input characters which contain spaces, new
        /// lines and indentation type characters.
        /// </summary>
        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(this.Peek())) {
                this.Accept();
            }
        }

        /// <summary>
        /// Matches string in input by peeking at characters ahead of the current buffer
        /// position. This is useful for detecting special keywords such as "true",
        /// "false" and "null".
        /// </summary>
        /// <remarks>
        /// <para>This method does not advanced current position within buffer; instead
        /// <see cref="Accept(int)"/> should be called by passing in the length of the
        /// matched string.</para>
        /// </remarks>
        /// <param name="match">String which is to be matched.</param>
        /// <returns>
        /// A value of <c>true</c> if string was matched; otherwise, a value of <c>false</c>.
        /// </returns>
        /// <seealso cref="Accept(int)"/>
        private bool MatchString(string match)
        {
            int length = Math.Min(MaximumLookahead, match.Length);
            for (int i = 0; i < length; ++i) {
                if (this.Peek(i) != match[i]) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Reads <see cref="JsonNode"/> from JSON encoded input.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonNode"/> instance of the applicable type; otherwise, a value
        /// of <c>null</c> if input content was either empty or consisted entirely of
        /// whitespace.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        public JsonNode Read()
        {
            this.SkipWhitespace();
            if (!this.HasReachedEnd) {
                return this.ReadValue();
            }
            return null;
        }

        /// <summary>
        /// Reads value node (null, integer, double, boolean, string, array or object).
        /// </summary>
        /// <returns>
        /// New <see cref="JsonNode"/> holding value; returns a value of <c>null</c> when
        /// <c>null</c> is detected in input.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private JsonNode ReadValue()
        {
            if (this.MatchString("true")) {
                this.Accept(4);
                return new JsonBooleanNode(true);
            }
            else if (this.MatchString("false")) {
                this.Accept(5);
                return new JsonBooleanNode(false);
            }
            else if (this.MatchString("null")) {
                this.Accept(4);
                return null;
            }
            else {
                char c = this.Peek();

                if (c == '-' || (c >= '0' && c <= '9')) {
                    return this.ReadNumeric();
                }

                switch (c) {
                    case '"':
                        return new JsonStringNode(this.ReadStringLiteral("String"));
                    case '[':
                        return this.ReadArray();
                    case '{':
                        return this.ReadObject();
                    default:
                        if (this.HasReachedEnd) {
                            throw new JsonParserException("Unexpected end of input; expected value.", this.lineNumber, this.linePosition);
                        }
                        else {
                            throw new JsonParserException("Encountered unexpected input '" + c + "'", this.lineNumber, this.linePosition);
                        }
                }
            }
        }


        /// <summary>
        /// Reads string literal for a value or a property key.
        /// </summary>
        /// <remarks>
        /// <para>Character escape sequences are automatically evaluated whilst parsing
        /// string literal.</para>
        /// </remarks>
        /// <param name="context">Context of literal; indicates whether literal is being
        /// parsed for a value or for a property key. This argument helps to provide more
        /// meaningful syntax error messages.</param>
        /// <returns>
        /// The resulting string.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private string ReadStringLiteral(string context)
        {
            this.stringLiteral.Length = 0;

            this.Accept();

            char c;

            while (!this.HasReachedEnd) {
                switch (this.Peek()) {
                    case '"':
                        this.Accept();
                        return this.stringLiteral.ToString();

                    case '\\':
                        this.Accept();
                        if (this.HasReachedEnd) {
                            break;
                        }

                        switch (this.Peek()) {
                            case '"':
                            case '\\':
                            case '/':
                                this.stringLiteral.Append(this.ReadChar());
                                break;
                            case 'b':
                                this.stringLiteral.Append('\b');
                                this.Accept();
                                break;
                            case 'f':
                                this.stringLiteral.Append('\f');
                                this.Accept();
                                break;
                            case 'n':
                                this.stringLiteral.Append('\n');
                                this.Accept();
                                break;
                            case 'r':
                                this.stringLiteral.Append('\r');
                                this.Accept();
                                break;
                            case 't':
                                this.stringLiteral.Append('\t');
                                this.Accept();
                                break;
                            case 'u':
                                this.Accept();

                                this.unicodeSequence.Length = 0;
                                for (int i = 0; i < 4; ++i) {
                                    if (this.HasReachedEnd) {
                                        throw new JsonParserException("Encountered unicode character escape sequence.", this.lineNumber, this.linePosition);
                                    }

                                    c = this.ReadChar();
                                    if (c >= '0' && c <= '9' || c >= 'A' && c <= 'F' || c >= 'a' && c <= 'f') {
                                        this.unicodeSequence.Append(c);
                                    }
                                    else {
                                        throw new JsonParserException("Encountered illegal character '" + c + "' in unicode escape sequence.", this.lineNumber, this.linePosition);
                                    }
                                }

                                this.stringLiteral.Append((char)int.Parse(this.unicodeSequence.ToString(), NumberStyles.HexNumber));
                                break;

                            default:
                                throw new JsonParserException("Encountered illegal escape sequence '\\" + this.Peek() + "'.", this.lineNumber, this.linePosition);
                        }
                        break;

                    default:
                        c = this.ReadChar();
                        this.CheckStringCharacter(c, context);
                        this.stringLiteral.Append(c);
                        break;
                }
            }

            throw new JsonParserException("Expected '\"' but reached end of input.", this.lineNumber, this.linePosition);
        }

        /// <summary>
        /// Checks input character to determine whether it is permitted within a string
        /// literal. For instance, control characters are not permitted in JSON encoded
        /// string literals, instead an escape sequence must be used '\n'.
        /// </summary>
        /// <param name="c">Candidate character.</param>
        /// <param name="context">Context of literal; indicates whether literal is being
        /// parsed for a value or for a property key. This argument helps to provide more
        /// meaningful syntax error messages.</param>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private void CheckStringCharacter(char c, string context)
        {
            if (char.IsControl(c)) {
                if (c == '\r' || c == '\n') {
                    throw new JsonParserException(context + " cannot span multiple lines. Consider using escape sequence '\n' instead.", this.lineNumber, this.linePosition);
                }
                else if (c == '\t') {
                    throw new JsonParserException(context + " cannot include tab character. Consider using escape sequence '\t' instead.", this.lineNumber, this.linePosition);
                }
                else {
                    throw new JsonParserException(context + " cannot contain control character. Consider using escape sequence instead", this.lineNumber, this.linePosition);
                }
            }
        }

        /// <summary>
        /// Reads a JSON array containing zero-or-more values.
        /// </summary>
        /// <returns>
        /// The new <see cref="JsonNode"/> instance.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private JsonNode ReadArray()
        {
            this.Accept();

            this.SkipWhitespace();

            var node = new JsonArrayNode();
            while (!this.HasReachedEnd) {
                if (this.Peek() == ']') {
                    this.Accept();
                    return node;
                }
                else if (this.Peek() == ',' && node.Count != 0) {
                    this.Accept();

                    this.SkipWhitespace();
                    if (this.HasReachedEnd) {
                        break;
                    }
                }

                node.Add(this.ReadValue());
                this.SkipWhitespace();
            }

            throw new JsonParserException("Expected ']' but reached end of input.", this.lineNumber, this.linePosition);
        }

        /// <summary>
        /// Reads a JSON object which comprises of zero-or-more named properties.
        /// </summary>
        /// <returns>
        /// The new <see cref="JsonNode"/> instance.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private JsonNode ReadObject()
        {
            this.Accept();

            this.SkipWhitespace();
            var node = new JsonObjectNode();
            while (!this.HasReachedEnd) {
                if (this.Peek() == '}') {
                    this.Accept();
                    return node;
                }
                else if (this.Peek() == ',' && node.Count != 0) {
                    this.Accept();

                    this.SkipWhitespace();
                    if (this.HasReachedEnd) {
                        break;
                    }
                }

                string key = this.ReadStringLiteral("Key");
                this.SkipWhitespace();
                if (this.HasReachedEnd) {
                    break;
                }

                if (this.Peek() != ':') {
                    throw new JsonParserException("Found '" + this.Peek() + "' but expected ':'", this.lineNumber, this.linePosition);
                }
                this.Accept();
                this.SkipWhitespace();
                if (this.HasReachedEnd) {
                    break;
                }

                node[key] = this.ReadValue();
                this.SkipWhitespace();
            }

            throw new JsonParserException("Expected '}' but reached end of input.", this.lineNumber, this.linePosition);
        }

        /// <summary>
        /// Reads a numeric value and determines whether to Creates a new <see cref="JsonIntegerNode"/>
        /// or <see cref="JsonDoubleNode"/> based upon formatting of input value.
        /// </summary>
        /// <remarks>
        /// <para>Please note that this method is unable to read values of NaN, -Infinity
        /// or Infinity since those are interpretted as string literals which can be
        /// processed at a later stage when deserializing nodes to generate object graphs.</para>
        /// </remarks>
        /// <returns>
        /// The new <see cref="JsonNode"/> instance.
        /// </returns>
        /// <exception cref="JsonParserException">
        /// If a syntax error was encountered whilst attempting to parse input content.
        /// Exception contains identifies the source of the error by providing the line
        /// number and position.
        /// </exception>
        private JsonNode ReadNumeric()
        {
            this.stringLiteral.Length = 0;

            bool integral = true;

            if (this.Peek() == '-') {
                this.stringLiteral.Append('-');
                this.Accept();
            }
            if (this.HasReachedEnd) {
                throw new JsonParserException("Expected numeric value but reached end of input.", this.lineNumber, this.linePosition);
            }

            while (char.IsDigit(this.Peek())) {
                this.stringLiteral.Append(this.ReadChar());
            }

            if (!this.HasReachedEnd && this.Peek() == '.') {
                integral = false;

                this.stringLiteral.Append('.');
                this.Accept();
                if (this.HasReachedEnd) {
                    throw new JsonParserException("Expected numeric value but reached end of input.", this.lineNumber, this.linePosition);
                }

                while (char.IsDigit(this.Peek())) {
                    this.stringLiteral.Append(this.ReadChar());
                }
            }

            char c = this.Peek();
            if (!this.HasReachedEnd && (c == 'e' || c == 'E')) {
                integral = false;

                this.stringLiteral.Append('e');
                this.Accept();
                if (this.HasReachedEnd) {
                    throw new JsonParserException("Expected numeric value but reached end of input.", this.lineNumber, this.linePosition);
                }

                c = this.Peek();
                if (c == '+' || c == '-') {
                    this.stringLiteral.Append(c);
                    this.Accept();

                    if (this.HasReachedEnd) {
                        throw new JsonParserException("Expected numeric value but reached end of input.", this.lineNumber, this.linePosition);
                    }
                }

                while (char.IsDigit(this.Peek())) {
                    this.stringLiteral.Append(this.ReadChar());
                }
            }

            if (integral) {
                return new JsonIntegerNode(long.Parse(this.stringLiteral.ToString(), CultureInfo.InvariantCulture));
            }
            else {
                return new JsonDoubleNode(double.Parse(this.stringLiteral.ToString(), CultureInfo.InvariantCulture));
            }
        }
    }
}
