using System;
using System.IO;
using System.Text;
using FtexTool.Dds.Enum;

namespace FtexTool.Dds
{
    public class DdsFileHeader
    {
        public const int DefaultHeaderSize = 124;
        public int Size { get; set; }
        public DdsFileHeaderFlags Flags { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PitchOrLinearSize { get; set; }
        public int Depth { get; set; }
        public int MipMapCount { get; set; }
        public DdsPixelFormat PixelFormat { get; set; }
        public DdsSurfaceFlags Caps { get; set; }
        public DdsCubemap Caps2 { get; set; }
        public int Caps3 { get; set; }
        public int Caps4 { get; set; }

        public static DdsFileHeader Read(Stream inputStream)
        {
            DdsFileHeader result = new DdsFileHeader();
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            result.Size = reader.ReadInt32();
            result.Flags = (DdsFileHeaderFlags) reader.ReadInt32();
            result.Height = reader.ReadInt32();
            result.Width = reader.ReadInt32();
            result.PitchOrLinearSize = reader.ReadInt32();
            result.Depth = reader.ReadInt32();
            result.MipMapCount = reader.ReadInt32();
            // int Reserved1[11];
            reader.Skip(44);
            result.PixelFormat = DdsPixelFormat.ReadDdsPixelFormat(inputStream);
            result.Caps = (DdsSurfaceFlags) reader.ReadInt32();
            result.Caps2 = (DdsCubemap) reader.ReadInt32();
            result.Caps3 = reader.ReadInt32();
            result.Caps4 = reader.ReadInt32();
            // int Reserved2;
            reader.Skip(4);
            return result;
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(Size);
            writer.Write(Convert.ToInt32(Flags));
            writer.Write(Height);
            writer.Write(Width);
            writer.Write(PitchOrLinearSize);
            writer.Write(Depth);
            writer.Write(MipMapCount);
            // int Reserved1[11];
            writer.WriteZeros(44);
            PixelFormat.Write(outputStream);
            writer.Write(Convert.ToInt32(Caps));
            writer.Write(Convert.ToInt32(Caps2));
            writer.Write(Caps3);
            writer.Write(Caps4);
            // int Reserved2
            writer.WriteZeros(4);
        }

        public bool IsDx10()
        {
            return PixelFormat != null &&
                   PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.FourCc) &&
                   PixelFormat.FourCc == DdsPixelFormat.Dx10FourCc;
        }

        public override string ToString()
        {
            return $"Size: {Size}, Flags: {Flags}, Height: {Height}, Width: {Width}," +
                   $" PitchOrLinearSize: {PitchOrLinearSize}, Depth: {Depth}, MipMapCount: {MipMapCount}," +
                   $" PixelFormat: {PixelFormat}, Caps: {Caps}, Caps2: {Caps2}, Caps3: {Caps3}, " + $"Caps4: {Caps4}";
        }
    }
}
