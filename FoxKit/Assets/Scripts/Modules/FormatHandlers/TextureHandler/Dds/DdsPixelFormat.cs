using System;
using System.IO;
using System.Text;
using FtexTool.Dds.Enum;

namespace FtexTool.Dds
{
    public class DdsPixelFormat
    {
        private const int DefaultSize = 32;
        internal const int Dxt1FourCc = 0x31545844;
        internal const int Dxt2FourCc = 0x32545844;
        internal const int Dxt3FourCc = 0x33545844;
        internal const int Dtx4FourCc = 0x34545844;
        internal const int Dtx5FourCc = 0x35545844;
        internal const int Dx10FourCc = 0x30315844;
        public int Size { get; set; }
        public DdsPixelFormatFlag Flags { get; set; }
        public int FourCc { get; set; }
        public int RgbBitCount { get; set; }
        public uint RBitMask { get; set; }
        public uint GBitMask { get; set; }
        public uint BBitMask { get; set; }
        public uint ABitMask { get; set; }

        public static DdsPixelFormat ReadDdsPixelFormat(Stream inputStream)
        {
            DdsPixelFormat result = new DdsPixelFormat();
            result.Read(inputStream);
            return result;
        }

        private void Read(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            Size = reader.ReadInt32();
            Flags = (DdsPixelFormatFlag) reader.ReadUInt32();
            FourCc = reader.ReadInt32();
            RgbBitCount = reader.ReadInt32();
            RBitMask = reader.ReadUInt32();
            GBitMask = reader.ReadUInt32();
            BBitMask = reader.ReadUInt32();
            ABitMask = reader.ReadUInt32();
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(Size);
            writer.Write(Convert.ToUInt32(Flags));
            writer.Write(FourCc);
            writer.Write(RgbBitCount);
            writer.Write(RBitMask);
            writer.Write(GBitMask);
            writer.Write(BBitMask);
            writer.Write(ABitMask);
        }

        public static DdsPixelFormat DdsPfDxt1()
        {
            return DdsPfDx(Dxt1FourCc); // DXT1
        }

        public static DdsPixelFormat DdsPfDxt2()
        {
            return DdsPfDx(Dxt2FourCc); // DXT2
        }

        public static DdsPixelFormat DdsPfDxt3()
        {
            return DdsPfDx(Dxt3FourCc); // DXT3
        }

        public static DdsPixelFormat DdsPfDxt4()
        {
            return DdsPfDx(Dtx4FourCc); // DXT4
        }

        public static DdsPixelFormat DdsPfDxt5()
        {
            return DdsPfDx(Dtx5FourCc); // DXT5
        }

        public static DdsPixelFormat DdsPfA8R8G8B8()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 32,
                RBitMask = 0x00ff0000,
                GBitMask = 0x0000ff00,
                BBitMask = 0x000000ff,
                ABitMask = 0xff000000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfA1R5G5B5()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x00007c00,
                GBitMask = 0x000003e0,
                BBitMask = 0x0000001f,
                ABitMask = 0x00008000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfA4R4G4B4()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x00000f00,
                GBitMask = 0x000000f0,
                BBitMask = 0x0000000f,
                ABitMask = 0x0000f000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfR8G8B8()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgb,
                FourCc = 0,
                RgbBitCount = 24,
                RBitMask = 0x00ff0000,
                GBitMask = 0x0000ff00,
                BBitMask = 0x000000ff,
                ABitMask = 0x00000000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfR5G6B5()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgb,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x0000f800,
                GBitMask = 0x000007e0,
                BBitMask = 0x0000001f,
                ABitMask = 0x00000000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfDx10()
        {
            return DdsPfDx(Dx10FourCc); // DX10
        }

        private static DdsPixelFormat DdsPfDx(int fourCc)
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.FourCc,
                FourCc = fourCc
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsLuminance()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Luminance,
                RgbBitCount = 8,
                RBitMask = 0x000000ff
            };
            return pixelFormat;
        }

        public static int CalculateImageSize(DdsPixelFormat pixelFormat, int width, int height, int depth)
        {
            if (pixelFormat.RgbBitCount > 0)
                return (int)((long)width * height * depth * pixelFormat.RgbBitCount / 8);
            if (pixelFormat.Equals(DdsPfDxt1()))
                return (int)((long)width * height * depth) / 2; // ((width*height*32)/8)/8;
            if (pixelFormat.Equals(DdsPfDxt3()))
                return (width * height * depth); // ((width*height*32)/4)/8;
            if (pixelFormat.Equals(DdsPfDxt5()))
                return (width * height * depth); // ((width*height*32)/4)/8;
            throw new ArgumentException("Could not calculate the image size of the current pixel format.");
        }

        public static int GetMinimumImageSize(DdsPixelFormat pixelFormat)
        {
            if (pixelFormat.Equals(DdsPfDxt1()))
                return 8;
            if (pixelFormat.Equals(DdsPfDxt3()))
                return 16;
            if (pixelFormat.Equals(DdsPfDxt5()))
                return 16;
            return 4;
        }

        protected bool Equals(DdsPixelFormat other)
        {
            return Size == other.Size &&
                   Flags == other.Flags &&
                   FourCc == other.FourCc &&
                   RgbBitCount == other.RgbBitCount &&
                   RBitMask == other.RBitMask &&
                   GBitMask == other.GBitMask &&
                   BBitMask == other.BBitMask &&
                   ABitMask == other.ABitMask;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DdsPixelFormat) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Size;
                hashCode = (hashCode*397) ^ (int) Flags;
                hashCode = (hashCode*397) ^ FourCc;
                hashCode = (hashCode*397) ^ RgbBitCount;
                hashCode = (hashCode*397) ^ (int) RBitMask;
                hashCode = (hashCode*397) ^ (int) GBitMask;
                hashCode = (hashCode*397) ^ (int) BBitMask;
                hashCode = (hashCode*397) ^ (int) ABitMask;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Size: {Size}, Flags: {Flags}, FourCc: {FourCc}, RgbBitCount: {RgbBitCount}," +
                   $" RBitMask: {RBitMask}, GBitMask: {GBitMask}, BBitMask: {BBitMask}, ABitMask: {ABitMask}";
        }
    }
}
