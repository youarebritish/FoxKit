using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    public class FoxVector4 : FoxStruct
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public override void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            W = reader.ReadSingle();
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(W);
        }

        public override int Size()
        {
            return 4*sizeof (float);
        }

        public override void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
        }

        public override void CalculateHashes()
        {
        }

        public override void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
        }

        public override void ReadXml(XmlReader reader)
        {
            var isEmptyElement = reader.IsEmptyElement;
            X = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("x"));
            Y = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("y"));
            Z = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("z"));
            W = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("w"));
            reader.ReadStartElement("value");
            if (isEmptyElement == false)
                reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("x", X.ToStringRoundtrip());
            writer.WriteAttributeString("y", Y.ToStringRoundtrip());
            writer.WriteAttributeString("z", Z.ToStringRoundtrip());
            writer.WriteAttributeString("w", W.ToStringRoundtrip());
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "x=\"{0:r}\", y=\"{1:r}\", z=\"{2:r}\", w=\"{3:r}\"",
                X, Y, Z, W);
        }
    }
}
