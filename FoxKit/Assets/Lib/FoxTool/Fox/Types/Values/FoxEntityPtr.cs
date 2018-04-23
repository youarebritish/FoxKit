using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace FoxTool.Fox.Types.Values
{
    public class FoxEntityPtr : IFoxValue
    {
        public ulong EntityPtr { get; set; }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            EntityPtr = reader.ReadUInt64();
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(EntityPtr);
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
                string entityPtr = reader.ReadString();
                EntityPtr = entityPtr.StartsWith("0x")
                    ? ulong.Parse(entityPtr.Substring(2, entityPtr.Length - 2), NumberStyles.AllowHexSpecifier)
                    : ulong.Parse(entityPtr);
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
            return string.Format("0x{0:X8}", EntityPtr);
        }
    }
}
