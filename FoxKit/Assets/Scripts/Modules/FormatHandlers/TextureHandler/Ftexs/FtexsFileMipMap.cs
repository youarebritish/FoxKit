using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FtexTool.Ftexs
{
    public class FtexsFileMipMap
    {
        private readonly List<FtexsFileChunk> _chunks;

        public FtexsFileMipMap()
        {
            _chunks = new List<FtexsFileChunk>();
        }

        public int Alignment { get; private set; }

        public IEnumerable<FtexsFileChunk> Chunks => _chunks;

        public byte[] Data
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                foreach (var chunk in Chunks)
                {
                    chunk.CopyTo(stream);
                }
                return stream.ToArray();
            }
        }
        
        private int IndexBlockSize => FtexsFileChunkIndex.IndexSize * _chunks.Count;

        public uint BaseOffset { get; private set; }

        public int BlockSize
        {
            get
            {
                return IndexBlockSize + Chunks.Sum(chunk => chunk.WrittenChunkSize) + Alignment;
            }
        }
        
        public static FtexsFileMipMap ReadFtexsFileMipMap(
            Stream inputStream,
            short chunkCount,
            int baseOffset,
            int fileSize)
        {
            FtexsFileMipMap result = new FtexsFileMipMap();
            result.Read(inputStream, chunkCount, baseOffset, fileSize);
            return result;
        }
        
        public void Read(Stream inputStream, short chunkCount, int baseOffset, int fileSize)
        {
            if (chunkCount == 0)
            {
                FtexsFileChunk chunk = FtexsFileChunk.ReadFtexsFileSingleChunk(
                    inputStream,
                    fileSize);
                AddChunk(chunk);
            }

            for (int i = 0; i < chunkCount; i++)
            {
                FtexsFileChunk chunk = FtexsFileChunk.ReadFtexsFileChunk(inputStream, baseOffset);
                AddChunk(chunk);
            }
        }

        public void AddChunk(FtexsFileChunk chunk)
        {
            _chunks.Add(chunk);
        }

        public void AddChunks(IEnumerable<FtexsFileChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                AddChunk(chunk);
            }
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.ASCII, true);
            BaseOffset = Convert.ToUInt32(writer.BaseStream.Position);
            
            writer.BaseStream.Position += IndexBlockSize;
            foreach (var chunk in Chunks)
            {
                chunk.UpdateDataOffset(outputStream, baseOffset: BaseOffset, isSingleChunk: Chunks.Count() == 1);
                chunk.WriteData(outputStream);
            }

            Alignment = writer.Align(16);
            // TODO: Write the next chunk info(s) (for streaming?)
            writer.WriteZeros(8);

            long endPosition = writer.BaseStream.Position;

            writer.BaseStream.Position = BaseOffset;
            foreach (var chunk in Chunks)
            {
                chunk.WriteIndex(writer);
            }

            writer.BaseStream.Position = endPosition;
        }
    }
}
