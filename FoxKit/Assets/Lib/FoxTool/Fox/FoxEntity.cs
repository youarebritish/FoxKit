using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FoxTool.Fox
{
    public class FoxEntity : IXmlSerializable
    {
        private const short HeaderSize = 64;
        private const uint MagicNumber = 0x746e65;
        private readonly List<FoxProperty> _dynamicProperties;
        private readonly List<FoxProperty> _staticProperties;

        public FoxEntity()
        {
            _staticProperties = new List<FoxProperty>();
            _dynamicProperties = new List<FoxProperty>();
        }

        public ulong ClassNameHash { get; set; }
        public string ClassName { get; set; }
        public short Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public short Version { get; set; }
        public uint Address { get; set; }

        public IEnumerable<FoxProperty> StaticProperties
        {
            get { return _staticProperties; }
        }

        public IEnumerable<FoxProperty> DynamicProperties
        {
            get { return _dynamicProperties; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            ClassName = reader.GetAttribute("class");
            Version = short.Parse(reader.GetAttribute("classVersion"));
            string addr = reader.GetAttribute("addr");
            Address = addr.StartsWith("0x")
                ? uint.Parse(addr.Substring(2, addr.Length - 2), NumberStyles.AllowHexSpecifier)
                : uint.Parse(addr);
            Unknown1 = short.Parse(reader.GetAttribute("unknown1"));
            Unknown2 = int.Parse(reader.GetAttribute("unknown2"));

            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement("entity");
            if (isEmptyElement) return;


            bool staticPropertiesElementEmpty = reader.IsEmptyElement;
            reader.ReadStartElement("staticProperties");
            if (staticPropertiesElementEmpty == false)
            {
                while (reader.LocalName == "property")
                {
                    FoxProperty staticProperty = new FoxProperty();
                    staticProperty.ReadXml(reader);
                    _staticProperties.Add(staticProperty);
                }
                reader.ReadEndElement();
            }

            bool dynamicPropertiesElementEmpty = reader.IsEmptyElement;
            reader.ReadStartElement("dynamicProperties");

            if (dynamicPropertiesElementEmpty == false)
            {
                while (reader.LocalName == "property")
                {
                    FoxProperty dynamicProperty = new FoxProperty();
                    dynamicProperty.ReadXml(reader);
                    _dynamicProperties.Add(dynamicProperty);
                }
                reader.ReadEndElement();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("class", ClassName);
            writer.WriteAttributeString("classVersion", Version.ToString());
            writer.WriteAttributeString("addr", String.Format("0x{0:X8}", Address));
            writer.WriteAttributeString("unknown1", Unknown1.ToString());
            writer.WriteAttributeString("unknown2", Unknown2.ToString());


            writer.WriteStartElement("staticProperties");
            foreach (var staticProperty in StaticProperties)
            {
                writer.WriteStartElement("property");
                staticProperty.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("dynamicProperties");
            foreach (var dynamicProperty in DynamicProperties)
            {
                writer.WriteStartElement("property");
                dynamicProperty.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public static FoxEntity ReadFoxEntity(Stream input)
        {
            FoxEntity entity = new FoxEntity();
            entity.Read(input);
            return entity;
        }

        private void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            short headerSize = reader.ReadInt16();
            Unknown1 = reader.ReadInt16();
            short padding1 = reader.ReadInt16();
            uint magicNumber1 = reader.ReadUInt32();
            Address = reader.ReadUInt32();
            uint padding2 = reader.ReadUInt32();
            Unknown2 = reader.ReadInt32();
            int unknown5 = reader.ReadInt32();
            Version = reader.ReadInt16();
            ClassNameHash = reader.ReadUInt64();
            ushort staticPropertyCount = reader.ReadUInt16();
            uint dynamicPropetyCount = reader.ReadUInt16();
            int offset = reader.ReadInt32();
            int staticDataSize = reader.ReadInt32();
            int dataSize = reader.ReadInt32();
            input.AlignRead(16);

            for (int i = 0; i < staticPropertyCount; i++)
            {
                FoxProperty property = FoxProperty.ReadFoxProperty(input);
                if (property.Container == null)
                    property.Container = null;
                _staticProperties.Add(property);
            }
            for (int i = 0; i < dynamicPropetyCount; i++)
            {
                FoxProperty property = FoxProperty.ReadFoxProperty(input);
                _dynamicProperties.Add(property);
            }
        }

        public void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
            ClassName = lookupTable.Lookup(ClassNameHash);
            foreach (var staticProperty in StaticProperties)
            {
                staticProperty.ResolveStringLiterals(lookupTable);
            }
            foreach (var dynamicProperty in DynamicProperties)
            {
                dynamicProperty.ResolveStringLiterals(lookupTable);
            }
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            long headerPosition = output.Position;
            output.Position += HeaderSize;
            foreach (var staticProperty in StaticProperties)
            {
                staticProperty.Write(output);
            }
            uint staticDataSize = (uint) (output.Position - headerPosition);
            foreach (var dynamicProperty in DynamicProperties)
            {
                dynamicProperty.Write(output);
            }
            long endPosition = output.Position;
            uint dataSize = (uint) (endPosition - headerPosition);
            output.Position = headerPosition;
            writer.Write(HeaderSize);
            writer.Write(Unknown1);
            writer.WriteZeros(2); // padding1
            writer.Write(MagicNumber);
            writer.Write(Address);
            writer.WriteZeros(4); // padding2
            writer.Write(Unknown2);
            writer.WriteZeros(4);
            writer.Write(Version);
            writer.Write(ClassNameHash);
            writer.Write(Convert.ToUInt16(StaticProperties.Count()));
            writer.Write(Convert.ToUInt16(DynamicProperties.Count()));
            writer.Write((int) HeaderSize);
            writer.Write(staticDataSize);
            writer.Write(dataSize);
            output.AlignWrite(16, 0x00);
            //writer.WriteZeros(12);
            output.Position = endPosition;
        }

        public void CalculateHashes()
        {
            ClassNameHash = Hashing.HashString(ClassName);
            foreach (var staticProperty in StaticProperties)
            {
                staticProperty.CalculateHashes();
            }

            foreach (var dynamicProperty in DynamicProperties)
            {
                dynamicProperty.CalculateHashes();
            }
        }

        public void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
            // TODO: Replace ClassName and ClassNameHash with a FoxStringLiteral
            literals.Add(new FoxStringLookupLiteral(ClassName, new FoxHash(ClassNameHash)));
            foreach (var staticProperty in StaticProperties)
            {
                staticProperty.CollectStringLookupLiterals(literals);
            }
            foreach (var dynamicProperty in DynamicProperties)
            {
                dynamicProperty.CollectStringLookupLiterals(literals);
            }
        }
    }
}
