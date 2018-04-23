using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace FoxTool.Fox.Types.Values
{
    public abstract class FoxStringBase : IFoxValue
    {
        private FoxStringLiteral StringLiteral { get; set; }

        public void Read(Stream input)
        {
            StringLiteral = FoxStringLiteral.ReadStringLiteral(input);
        }

        public void Write(Stream output)
        {
            StringLiteral.Write(output);
        }

        public int Size()
        {
            return FoxStringLiteral.Size();
        }

        public void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
            StringLiteral.Resolve(lookupTable);
        }

        public void CalculateHashes()
        {
            StringLiteral.CalculateHash();
        }

        public void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
            literals.Add(new FoxStringLookupLiteral(StringLiteral));
        }

        public void ReadXml(XmlReader reader)
        {
            var isEmptyElement = reader.IsEmptyElement;
            FoxHash hash = new FoxHash();
            hash.ReadXml(reader);
            reader.ReadStartElement("value");
            string literal = null;
            if (isEmptyElement == false)
            {
                literal = reader.ReadContentAsString();
                reader.ReadEndElement();
            }
            StringLiteral = new FoxStringLiteral(literal, hash);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            if (StringLiteral.Literal == null)
                StringLiteral.Hash.WriteXml(writer);
            else
                writer.WriteString(StringLiteral.Literal);
        }

        public override string ToString()
        {
            return StringLiteral.ToString();
        }
    }
}
