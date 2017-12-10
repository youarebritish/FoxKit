using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FtexTool.Ftexs
{
    public class FtexsFile
    {
        private readonly List<FtexsFileMipMap> _mipMaps;

        public FtexsFile()
        {
            _mipMaps = new List<FtexsFileMipMap>();
        }

        public IEnumerable<FtexsFileMipMap> MipMaps => _mipMaps;

        public byte FileNumber { get; set; }

        public byte[] Data
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                foreach (var mipMap in MipMaps)
                {
                    stream.Write(mipMap.Data, 0, mipMap.Data.Length);
                }
                return stream.ToArray();
            }
        }
        
        public void Read(
            Stream inputStream,
            short chunkCount,
            int baseOffset,
            int fileSize)
        {
            FtexsFileMipMap mipMap = FtexsFileMipMap.ReadFtexsFileMipMap(
                inputStream,
                chunkCount,
                baseOffset,
                fileSize);
            AddMipMap(mipMap);
        }
        
        public void AddMipMap(FtexsFileMipMap mipMap)
        {
            _mipMaps.Add(mipMap);
        }

        public void Write(Stream outputStream)
        {
            var mipMaps = MipMaps.Reverse().ToList();
            foreach (var mipMap in mipMaps)
            {
                mipMap.Write(outputStream);
            }
        }
    }
}
