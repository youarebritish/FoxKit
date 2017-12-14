namespace GzsTool.Core
{
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Various utility extension methods.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Convert stream to a byte array.
        /// </summary>
        /// <param name="input">
        /// The input stream.
        /// </param>
        /// <returns>
        /// The input stream as a byte array.
        /// </returns>
        public static byte[] ToByteArray(this Stream input)
        {
            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Skip a number of bytes.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="count">
        /// Number of bytes to skip.
        /// </param>
        internal static void Skip(this BinaryReader reader, int count)
        {
            reader.BaseStream.Skip(count);
        }

        /// <summary>
        /// Skip a number of bytes.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="count">
        /// Number of bytes to skip.
        /// </param>
        internal static void Skip(this Stream stream, int count)
        {
            stream.Seek(count, SeekOrigin.Current);
        }

        /// <summary>
        /// Write a number of zeroes.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="count">
        /// Number of zeroes to write.
        /// </param>
        internal static void WriteZeroes(this BinaryWriter writer, int count)
        {
            var zeroes = new byte[count];
            writer.Write(zeroes);
        }

        /// <summary>
        /// Read a string.
        /// </summary>
        /// <param name="binaryReader">
        /// The binary reader.
        /// </param>
        /// <param name="count">
        /// Number of characters to read.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal static string ReadString(this BinaryReader binaryReader, int count)
        {
            return new string(binaryReader.ReadChars(count));
        }

        /// <summary>
        /// Read a null-terminated string.
        /// </summary>
        /// <param name="binaryReader">
        /// The binary reader.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal static string ReadNullTerminatedString(this BinaryReader binaryReader)
        {
            var builder = new StringBuilder();
            char nextCharacter;
            while ((nextCharacter = binaryReader.ReadChar()) != 0)
            {
                builder.Append(nextCharacter);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Write a null-terminated string.
        /// </summary>
        /// <param name="binaryWriter">
        /// The binary writer.
        /// </param>
        /// <param name="text">
        /// The string to write.
        /// </param>
        internal static void WriteNullTerminatedString(this BinaryWriter binaryWriter, string text)
        {
            var data = Encoding.Default.GetBytes(text + '\0');
            binaryWriter.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Align the input stream.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        internal static void AlignRead(this Stream input, int alignment)
        {
            var alignmentRequired = input.Position % alignment;
            if (alignmentRequired > 0)
            {
                input.Position += alignment - alignmentRequired;
            }
        }

        /// <summary>
        /// Write alignment bytes.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="alignment">
        /// Alignment bytes.
        /// </param>
        /// <param name="data">
        /// The data to write.
        /// </param>
        internal static void AlignWrite(this Stream output, int alignment, byte data)
        {
            var alignmentRequired = output.Position % alignment;
            if (alignmentRequired <= 0)
            {
                return;
            }
            var alignmentBytes = Enumerable.Repeat(data, (int)(alignment - alignmentRequired)).ToArray();
            output.Write(alignmentBytes, 0, alignmentBytes.Length);
        }
    }
}
