using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FoxTool.Fox
{
    public class FoxHash : IXmlSerializable
    {
        public const int Size = sizeof (ulong);
        private string _hashName = "hash";

        public FoxHash()
        {
        }

        public FoxHash(ulong hashValue)
        {
            HashValue = hashValue;
        }

        public FoxHash(string hashName)
        {
            _hashName = hashName;
        }

        public ulong HashValue { get; set; }

        public string HashName
        {
            get { return _hashName; }
            set { _hashName = value; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string hash = reader.GetAttribute(_hashName);
            if (hash != null)
            {
                HashValue = hash.StartsWith("0x")
                    ? ulong.Parse(hash.Substring(2, hash.Length - 2), NumberStyles.AllowHexSpecifier)
                    : ulong.Parse(hash);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(_hashName, String.Format("0x{0:X8}", HashValue));
        }

        public static FoxHash ReadFoxHash(Stream input)
        {
            FoxHash foxHash = new FoxHash();
            foxHash.Read(input);
            return foxHash;
        }

        public void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            HashValue = reader.ReadUInt64();
        }

        public void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(HashValue);
        }

        protected bool Equals(FoxHash other)
        {
            return HashValue == other.HashValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FoxHash) obj);
        }

        public override int GetHashCode()
        {
            return HashValue.GetHashCode();
        }
    }
}
