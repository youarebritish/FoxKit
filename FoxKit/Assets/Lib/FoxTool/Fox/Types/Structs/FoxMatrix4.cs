using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    public class FoxMatrix4 : FoxStruct
    {
        public float Row1Value1 { get; set; }
        public float Row1Value2 { get; set; }
        public float Row1Value3 { get; set; }
        public float Row1Value4 { get; set; }
        public float Row2Value1 { get; set; }
        public float Row2Value2 { get; set; }
        public float Row2Value3 { get; set; }
        public float Row2Value4 { get; set; }
        public float Row3Value1 { get; set; }
        public float Row3Value2 { get; set; }
        public float Row3Value3 { get; set; }
        public float Row3Value4 { get; set; }
        public float Row4Value1 { get; set; }
        public float Row4Value2 { get; set; }
        public float Row4Value3 { get; set; }
        public float Row4Value4 { get; set; }

        public override void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Row1Value1 = reader.ReadSingle();
            Row1Value2 = reader.ReadSingle();
            Row1Value3 = reader.ReadSingle();
            Row1Value4 = reader.ReadSingle();
            Row2Value1 = reader.ReadSingle();
            Row2Value2 = reader.ReadSingle();
            Row2Value3 = reader.ReadSingle();
            Row2Value4 = reader.ReadSingle();
            Row3Value1 = reader.ReadSingle();
            Row3Value2 = reader.ReadSingle();
            Row3Value3 = reader.ReadSingle();
            Row3Value4 = reader.ReadSingle();
            Row4Value1 = reader.ReadSingle();
            Row4Value2 = reader.ReadSingle();
            Row4Value3 = reader.ReadSingle();
            Row4Value4 = reader.ReadSingle();
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            writer.Write(Row1Value1);
            writer.Write(Row1Value2);
            writer.Write(Row1Value3);
            writer.Write(Row1Value4);
            writer.Write(Row2Value1);
            writer.Write(Row2Value2);
            writer.Write(Row2Value3);
            writer.Write(Row2Value4);
            writer.Write(Row3Value1);
            writer.Write(Row3Value2);
            writer.Write(Row3Value3);
            writer.Write(Row3Value4);
            writer.Write(Row4Value1);
            writer.Write(Row4Value2);
            writer.Write(Row4Value3);
            writer.Write(Row4Value4);
        }

        public override int Size()
        {
            return 16*sizeof (float);
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
            reader.ReadStartElement("value");
            if (isEmptyElement == false)
            {
                Row1Value1 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column1"));
                Row1Value2 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column2"));
                Row1Value3 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column3"));
                Row1Value4 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column4"));
                reader.ReadStartElement("Row1");
                Row2Value1 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column1"));
                Row2Value2 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column2"));
                Row2Value3 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column3"));
                Row2Value4 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column4"));
                reader.ReadStartElement("Row2");
                Row3Value1 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column1"));
                Row3Value2 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column2"));
                Row3Value3 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column3"));
                Row3Value4 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column4"));
                reader.ReadStartElement("Row3");
                Row4Value1 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column1"));
                Row4Value2 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column2"));
                Row4Value3 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column3"));
                Row4Value4 = ExtensionMethods.ParseFloatRoundtrip(reader.GetAttribute("Column4"));
                reader.ReadStartElement("Row4");
                reader.ReadEndElement();
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Row1");
            writer.WriteAttributeString("Column1", Row1Value1.ToStringRoundtrip());
            writer.WriteAttributeString("Column2", Row1Value2.ToStringRoundtrip());
            writer.WriteAttributeString("Column3", Row1Value3.ToStringRoundtrip());
            writer.WriteAttributeString("Column4", Row1Value4.ToStringRoundtrip());
            writer.WriteEndElement();
            writer.WriteStartElement("Row2");
            writer.WriteAttributeString("Column1", Row2Value1.ToStringRoundtrip());
            writer.WriteAttributeString("Column2", Row2Value2.ToStringRoundtrip());
            writer.WriteAttributeString("Column3", Row2Value3.ToStringRoundtrip());
            writer.WriteAttributeString("Column4", Row2Value4.ToStringRoundtrip());
            writer.WriteEndElement();
            writer.WriteStartElement("Row3");
            writer.WriteAttributeString("Column1", Row3Value1.ToStringRoundtrip());
            writer.WriteAttributeString("Column2", Row3Value2.ToStringRoundtrip());
            writer.WriteAttributeString("Column3", Row3Value3.ToStringRoundtrip());
            writer.WriteAttributeString("Column4", Row3Value4.ToStringRoundtrip());
            writer.WriteEndElement();
            writer.WriteStartElement("Row4");
            writer.WriteAttributeString("Column1", Row4Value1.ToStringRoundtrip());
            writer.WriteAttributeString("Column2", Row4Value2.ToStringRoundtrip());
            writer.WriteAttributeString("Column3", Row4Value3.ToStringRoundtrip());
            writer.WriteAttributeString("Column4", Row4Value4.ToStringRoundtrip());
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Row1Value1: {0}, Row1Value2: {1}, Row1Value3: {2}, Row1Value4: {3}, Row2Value1: {4}, Row2Value2: {5}, Row2Value3: {6}, Row2Value4: {7}, Row3Value1: {8}, Row3Value3: {9}, Row3Value2: {10}, Row3Value4: {11}, Row4Value1: {12}, Row4Value2: {13}, Row4Value3: {14}, Row4Value4: {15}",
                    Row1Value1, Row1Value2, Row1Value3, Row1Value4, Row2Value1, Row2Value2, Row2Value3, Row2Value4,
                    Row3Value1, Row3Value3, Row3Value2, Row3Value4, Row4Value1, Row4Value2, Row4Value3, Row4Value4);
        }
    }
}
