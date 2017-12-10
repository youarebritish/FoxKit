using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FtexTool.Ftex.Enum;
using FtexTool.Ftexs;

namespace FtexTool.Ftex
{
    public class FtexFile
    {
        private const long MagicNumber1 = 4612226451348214854; // FTEX 85 EB 01 40

        private const int OneInt32 = 1;

        private const int ZeroInt32 = 0;

        private const byte ZeroByte = 0;

        private readonly Dictionary<int, FtexsFile> _ftexsFiles;

        private readonly List<FtexFileMipMapInfo> _mipMapInfos;

        public FtexFile()
        {
            _ftexsFiles = new Dictionary<int, FtexsFile>();
            _mipMapInfos = new List<FtexFileMipMapInfo>();
            Hash = new byte[16];
        }

        public short PixelFormatType { get; set; }

        public short Width { get; set; }

        public short Height { get; set; }

        public short Depth { get; set; }

        public byte MipMapCount { get; set; }

        // Flags 
        // 0x0 when file ends with _nrt 
        // 0x2 else
        public byte NrtFlag { get; set; }

        // Flags
        // 0   (~3%)
        // 17  (<1%)
        // 273 (~96%)
        public FtexUnknownFlags UnknownFlags { get; set; }

        public FtexTextureType TextureType { get; set; }

        public byte FtexsFileCount { get; set; }

        public byte AdditionalFtexsFileCount { get; set; }

        public byte[] Hash { get; set; }

        public IEnumerable<FtexFileMipMapInfo> MipMapInfos => _mipMapInfos;

        public IEnumerable<FtexsFile> FtexsFiles => _ftexsFiles.Values;

        public byte[] Data
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                foreach (var ftexsFile in FtexsFiles.Reverse())
                {
                    byte[] ftexsFileData = ftexsFile.Data;
                    stream.Write(ftexsFileData, 0, ftexsFileData.Length);
                }
                return stream.ToArray();
            }
        }

        public int Size
        {
            get
            {
                const int headerSize = 64;
                const int mipMapInfoSize = 16;
                return headerSize + mipMapInfoSize * _mipMapInfos.Count;
            }
        }

        public void AddFtexsFile(FtexsFile ftexsFile)
        {
            _ftexsFiles.Add(ftexsFile.FileNumber, ftexsFile);
        }

        public void AddFtexsFiles(IEnumerable<FtexsFile> ftexsFiles)
        {
            foreach (var ftexsFile in ftexsFiles)
            {
                AddFtexsFile(ftexsFile);
            }
        }

        public bool TryGetFtexsFile(int fileNumber, out FtexsFile ftexsFile)
        {
            return _ftexsFiles.TryGetValue(fileNumber, out ftexsFile);
        }

        public static FtexFile ReadFtexFile(Stream inputStream)
        {
            FtexFile result = new FtexFile();
            result.Read(inputStream);
            return result;
        }

        private void Read(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            reader.Assert(MagicNumber1);
            PixelFormatType = reader.ReadInt16();
            Width = reader.ReadInt16();
            Height = reader.ReadInt16();
            Depth = reader.ReadInt16();
            MipMapCount = reader.ReadByte();
            NrtFlag = reader.ReadByte();
            UnknownFlags = (FtexUnknownFlags)reader.ReadInt16();
            reader.Assert(OneInt32);
            reader.Assert(ZeroInt32);
            TextureType = (FtexTextureType) reader.ReadInt32();
            FtexsFileCount = reader.ReadByte();
            AdditionalFtexsFileCount = reader.ReadByte();
            reader.Assert(ZeroByte);
            reader.Assert(ZeroByte);
            reader.Assert(ZeroInt32);
            reader.Assert(ZeroInt32);
            reader.Assert(ZeroInt32);
            Hash = reader.ReadBytes(16);

            for (int i = 0; i < MipMapCount; i++)
            {
                FtexFileMipMapInfo fileMipMapInfo = FtexFileMipMapInfo.ReadFtexFileMipMapInfo(inputStream);
                AddMipMapInfo(fileMipMapInfo);
            }
        }

        private void AddMipMapInfo(FtexFileMipMapInfo fileMipMapInfo)
        {
            _mipMapInfos.Add(fileMipMapInfo);
        }

        public void AddMipMapInfos(IEnumerable<FtexFileMipMapInfo> mipMapInfos)
        {
            foreach (var mipMapInfo in mipMapInfos)
            {
                AddMipMapInfo(mipMapInfo);
            }
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(MagicNumber1);
            writer.Write(PixelFormatType);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Depth);
            writer.Write(MipMapCount);
            writer.Write(NrtFlag);
            writer.Write((short)UnknownFlags);
            writer.Write(OneInt32);
            writer.Write(ZeroInt32);
            writer.Write(Convert.ToInt32(TextureType));
            writer.Write(FtexsFileCount);
            writer.Write(AdditionalFtexsFileCount);
            writer.Write(ZeroByte);
            writer.Write(ZeroByte);
            writer.Write(ZeroInt32);
            writer.Write(ZeroInt32);
            writer.Write(ZeroInt32);
            writer.Write(Hash);
            foreach (var mipMap in MipMapInfos)
            {
                mipMap.Write(outputStream);
            }
        }

        public void UpdateOffsets()
        {
            int mipMapIndex = 0;
            foreach (var ftexsFile in FtexsFiles)
            {
                foreach (var ftexsFileMipMap in ftexsFile.MipMaps)
                {
                    FtexFileMipMapInfo ftexMipMapInfo = MipMapInfos.ElementAt(mipMapIndex);
                    ftexMipMapInfo.Size = ftexsFileMipMap.BlockSize;
                    ftexMipMapInfo.ChunkCount = Convert.ToInt16(ftexsFileMipMap.Chunks.Count());
                    ftexMipMapInfo.Offset = (int) ftexsFileMipMap.BaseOffset;
                    ++mipMapIndex;
                }
            }
        }
    }
}
