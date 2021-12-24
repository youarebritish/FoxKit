using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FoxKit.GrxArray.GrxArrayTool
{
    public static class Extensions
    {
        public static float ParseFloatRoundtrip(string text)
        {
            if (text == "-0")
            {
                return -0f;
            }

            return float.Parse(text, CultureInfo.InvariantCulture);
        }
        public static string ReadCString(this BinaryReader reader)
        {
            var chars = new List<char>();
            var @char = reader.ReadChar();
            while (@char != '\0')
            {
                chars.Add(@char);
                @char = reader.ReadChar();
            }

            return new string(chars.ToArray());
        }
        public static void WriteCString(this BinaryWriter writer, string iString)
        {
            char[] stringChars = iString.ToCharArray();
            foreach (var chara in stringChars)
                writer.Write(chara);
        }
        public static void WriteZeroes(this BinaryWriter writer, int count)
        {
            byte[] array = new byte[count];

            writer.Write(array);
        } //WriteZeroes
    }
}