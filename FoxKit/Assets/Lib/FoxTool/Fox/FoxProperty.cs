using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FoxTool.Fox.Containers;
using FoxTool.Fox.Types;

namespace FoxTool.Fox
{
    public class FoxProperty : IXmlSerializable
    {
        private const int HeaderSize = 32;
        public string Name { get; set; }
        public ulong NameHash { get; set; }
        public FoxDataType DataType { get; set; }
        public FoxContainerType ContainerType { get; set; }
        public IFoxContainer Container { get; set; }
        public string EnumName { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("name");
            string dataType = reader.GetAttribute("type");
            DataType = ExtensionMethods.ParseFoxDataType(dataType);
            string containerType = reader.GetAttribute("container");
            ContainerType = ExtensionMethods.ParseFoxContainerType(containerType);
            bool emptyElement = reader.IsEmptyElement;
            reader.ReadStartElement("property");
            Container = FoxContainerFactory.CreateTypedContainer(DataType, ContainerType);
            if (emptyElement == false)
            {
                Container.ReadXml(reader);
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("type", DataType.ToXmlName());
            writer.WriteAttributeString("container", ContainerType.ToString());
            Container.WriteXml(writer);
        }

        public static FoxProperty ReadFoxProperty(Stream input)
        {
            FoxProperty property = new FoxProperty();
            property.Read(input);
            return property;
        }

        private void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            NameHash = reader.ReadUInt64();

            DataType = (FoxDataType) reader.ReadByte();
            ContainerType = (FoxContainerType) reader.ReadByte();
            short valueCount = reader.ReadInt16();
            short offset = reader.ReadInt16();
            ushort size = reader.ReadUInt16();

            int unknown2 = reader.ReadInt32();
            int unknown3 = reader.ReadInt32();
            int unknown4 = reader.ReadInt32();
            int unknown5 = reader.ReadInt32();

            Container = FoxContainerFactory.ReadFoxContainer(input, DataType, ContainerType, valueCount);
            input.AlignRead(16);
        }

        public void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
            Name = lookupTable.Lookup(NameHash);
            Container.ResolveStringLiterals(lookupTable);
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            long headerPosition = output.Position;
            output.Position += HeaderSize;
            Container.Write(output);
            output.AlignWrite(16, 0x00);
            long endPosition = output.Position;
            ushort size = (ushort) (endPosition - headerPosition);
            output.Position = headerPosition;
            writer.Write(NameHash);
            writer.Write((byte) DataType);
            writer.Write((byte) ContainerType);
            writer.Write((ushort) Container.Count());
            writer.Write((ushort) HeaderSize);
            writer.Write(size);
            writer.WriteZeros(16);
            output.Position = endPosition;
        }

        public void CalculateHashes()
        {
            NameHash = Hashing.HashString(Name);
            Container.CalculateHashes();
        }

        public void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
            // TODO: Replace Name and NameHash with a FoxStringLiteral
            literals.Add(new FoxStringLookupLiteral(Name, new FoxHash(NameHash)));
            Container.CollectStringLookupLiterals(literals);
        }
    }
}
