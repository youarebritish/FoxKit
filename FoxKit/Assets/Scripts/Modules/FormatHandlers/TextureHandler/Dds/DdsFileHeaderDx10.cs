using System;
using System.IO;
using System.Text;
using FtexTool.Dds.Enum;

namespace FtexTool.Dds
{
    public class DdsFileHeaderDx10
    {
        public DxgiFormat Format { get; set; }
        public D3D10ResourceDimension ResourceDimension { get; set; }
        private uint MiscFlag { get; set; }
        private uint ArraySize { get; set; }

        public static DdsFileHeaderDx10 Read(Stream inputStream)
        {
            DdsFileHeaderDx10 result = new DdsFileHeaderDx10();
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            result.Format = (DxgiFormat) reader.ReadUInt32();
            result.ResourceDimension = (D3D10ResourceDimension) reader.ReadInt32();
            result.MiscFlag = reader.ReadUInt32();
            result.ArraySize = reader.ReadUInt32();
            return result;
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(Convert.ToUInt32(Format));
            writer.Write(Convert.ToInt32(ResourceDimension));
            writer.Write(MiscFlag);
            writer.Write(ArraySize);
        }
    }
}
