using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FoxTool.Fox.Types.Values
{
    public class FoxBool : IFoxValue
    {
        public bool Value { get; set; }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Value = reader.ReadBoolean();
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(Value);
        }

        public int Size()
        {
            return sizeof (bool);
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
                Value = bool.Parse(reader.ReadString());
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
            return Value ? "true" : "false";
        }
    }
}
