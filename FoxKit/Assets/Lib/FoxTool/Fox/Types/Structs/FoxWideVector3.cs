using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    public class FoxWideVector3 : FoxStruct
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public ushort A { get; set; }
        public ushort B { get; set; }

        public override void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            A = reader.ReadUInt16();
            B = reader.ReadUInt16();
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(A);
            writer.Write(B);
        }

        public override int Size()
        {
            return 3 * sizeof(float) + 2 * sizeof(ushort);
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
            A = ushort.Parse(reader.GetAttribute("a"));
            B = ushort.Parse(reader.GetAttribute("b"));
            reader.ReadStartElement("value");
            if (isEmptyElement == false)
                reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("x", X.ToStringRoundtrip());
            writer.WriteAttributeString("y", Y.ToStringRoundtrip());
            writer.WriteAttributeString("z", Z.ToStringRoundtrip());
            writer.WriteAttributeString("a", A.ToString());
            writer.WriteAttributeString("b", B.ToString());

        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "x=\"{0:r}\", y=\"{1:r}\", z=\"{2:r}\", a=\"{3}\", b=\"{4}\"", X,
                Y, Z, A, B);
        }
    }
}
