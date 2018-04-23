using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FoxTool.Fox
{
    public class FoxClass : IXmlSerializable
    {
        public string Name { get; set; }
        public string Super { get; set; }
        public string Version { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("name");
            Super = reader.GetAttribute("super");
            Version = reader.GetAttribute("version");
            reader.ReadStartElement("class");
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("super", Super);
            writer.WriteAttributeString("version", Version);
        }

        protected bool Equals(FoxClass other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Super, other.Super);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FoxClass) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Super != null ? Super.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
