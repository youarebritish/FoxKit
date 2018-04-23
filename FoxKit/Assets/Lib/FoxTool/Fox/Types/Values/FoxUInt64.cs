using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FoxTool.Fox.Types.Values
{
    public class FoxUInt64 : IFoxValue
    {
        public ulong Value { get; set; }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Value = reader.ReadUInt64();
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(Value);
        }

        public int Size()
        {
            return sizeof (ulong);
        }

        public void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
        }

        public void CalculateHashes()
        {
        }

        public void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
        }

        public void ReadXml(XmlReader reader)
        {
            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement("value");
            if (isEmptyElement == false)
            {
                string value = reader.ReadString();
                Value = value.StartsWith("0x")
                    ? ulong.Parse(value.Substring(2, value.Length - 2), NumberStyles.AllowHexSpecifier)
                    : ulong.Parse(value);
                reader.ReadEndElement();
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
