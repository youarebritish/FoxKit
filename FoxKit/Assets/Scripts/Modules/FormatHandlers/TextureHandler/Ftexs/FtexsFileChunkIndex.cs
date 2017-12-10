using System;
using System.IO;

namespace FtexTool.Ftexs
{
    public class FtexsFileChunkIndex
    {
        public const int IndexSize = 8;

        private const uint RelativeOffsetValue = 0x80000000;

        public short WrittenChunkSize => CompressData ? CompressedChunkSize : ChunkSize;

        public long DataOffset { get; private set; }

        public bool CompressData { get; private set; }

        public short CompressedChunkSize { get; set; }

        public short ChunkSize { get; set; }

        private uint EncodedDataOffset { get; set; }

        public void Read(BinaryReader reader, int baseOffset)
        {
            CompressedChunkSize = reader.ReadInt16();
            ChunkSize = reader.ReadInt16();
            EncodedDataOffset = reader.ReadUInt32();
            
            long dataOffset;
            if (EncodedDataOffset > RelativeOffsetValue)
            {
                dataOffset = baseOffset + (EncodedDataOffset - RelativeOffsetValue);
            }
            else
            {
                dataOffset = baseOffset + EncodedDataOffset;
            }

            DataOffset = dataOffset;
        }

        public void SetDataOffset(Stream outputStream, uint baseOffset, bool isSingleChunk)
        {
            DataOffset = outputStream.Position;

            CompressData = true;
            if (isSingleChunk && ChunkSize <= CompressedChunkSize)
            {
                EncodedDataOffset = IndexSize | RelativeOffsetValue;
                CompressData = false;
            }
            else
            {
                EncodedDataOffset = Convert.ToUInt32(outputStream.Position) - baseOffset;
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(WrittenChunkSize);
            writer.Write(ChunkSize);
            writer.Write(EncodedDataOffset);
        }
    }
}