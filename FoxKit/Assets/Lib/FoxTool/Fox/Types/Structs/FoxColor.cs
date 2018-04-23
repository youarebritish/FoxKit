using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    public class FoxColor : FoxStruct
    {
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }

        public override void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Red = reader.ReadSingle();
            Green = reader.ReadSingle();
            Blue = reader.ReadSingle();
            Alpha = reader.ReadSingle();
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(Red);
            writer.Write(Green);
            writer.Write(Blue);
            writer.Write(Alpha);
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
            Red = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("r"));
            Green = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("g"));
            Blue = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("b"));
            Alpha = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("a"));
            reader.ReadStartElement("value");
            if (isEmptyElement == false)
                reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("r", Red.ToStringRoundtrip());
            writer.WriteAttributeString("g", Green.ToStringRoundtrip());
            writer.WriteAttributeString("b", Blue.ToStringRoundtrip());
            writer.WriteAttributeString("a", Alpha.ToStringRoundtrip());
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "r={0:r}, g={1:r}, b={2:r}, a={3:r}", Red, Green,
                Blue, Alpha);
        }
    }
}
