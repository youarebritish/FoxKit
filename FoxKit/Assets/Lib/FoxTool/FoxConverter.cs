using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FoxTool.Fox;

namespace FoxTool
{
    internal static class FoxConverter
    {
        public static void DecompileFox(FoxFile foxFile, Stream output)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Constants.StringEncoding,
                Indent = true
            };

            XmlWriter writer = XmlWriter.Create(output, settings);
            XmlSerializer serializer = new XmlSerializer(typeof (FoxFile));
            serializer.Serialize(writer, foxFile);
            writer.Close();
        }

        public static void CompileFox(Stream input, Stream output)
        {
            FoxFile foxFile = ReadFoxFile(input);
            foxFile.CalculateHashes();
            foxFile.CollectStringLookupLiterals();
            foxFile.Write(output);
        }

        private static FoxFile ReadFoxFile(Stream input)
        {
            var xmlReaderSettings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            XmlReader reader = XmlReader.Create(input, xmlReaderSettings);
            XmlSerializer serializer = new XmlSerializer(typeof (FoxFile));
            var foxFile = (FoxFile) serializer.Deserialize(reader);
            return foxFile;
        }
    }
}
