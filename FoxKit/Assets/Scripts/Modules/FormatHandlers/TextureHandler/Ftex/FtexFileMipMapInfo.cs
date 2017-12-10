using System.IO;
using System.Text;

namespace FtexTool.Ftex
{
    public class FtexFileMipMapInfo
    {
        public int Offset { get; set; }
        public int DecompressedFileSize { get; set; }
        public int Size { get; set; }
        public byte Index { get; set; }
        public byte FtexsFileNumber { get; set; }
        public short ChunkCount { get; set; }

        public static FtexFileMipMapInfo ReadFtexFileMipMapInfo(Stream inputStream)
        {
            FtexFileMipMapInfo result = new FtexFileMipMapInfo();
            result.Read(inputStream);
            return result;
        }

        public void Read(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            Offset = reader.ReadInt32();
            DecompressedFileSize = reader.ReadInt32();
            Size = reader.ReadInt32();
            Index = reader.ReadByte();
            FtexsFileNumber = reader.ReadByte();
            ChunkCount = reader.ReadInt16();
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(Offset);
            writer.Write(DecompressedFileSize);
            writer.Write(Size);
            writer.Write(Index);
            writer.Write(FtexsFileNumber);
            writer.Write(ChunkCount);
        }
    }
}
