using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FoxTool.Fox.Types.Values
{
    public class FoxFloat : IFoxValue
    {
        public float Value { get; set; }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Value = reader.ReadSingle();
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(Value);
        }

        public int Size()
        {
            return sizeof (float);
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
                Value = ExtensionMethods.ParseFloatRoundtrip(reader.ReadString());
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
            return String.Format(CultureInfo.InvariantCulture, "{0:r}", Value);
        }
    }
}
