using System.IO;
using System.Text;

namespace FtexTool.Dds
{
    public class DdsFile
    {
        private const int MagicNumber = 0x20534444;
        public DdsFileHeader Header { get; set; }
        public DdsFileHeaderDx10 HeaderDx10 { get; set; }
        public byte[] Data { get; set; }

        public static DdsFile Read(Stream inputStream)
        {
            DdsFile result = new DdsFile();
            BinaryReader reader = new BinaryReader(inputStream, Encoding.Default, true);
            reader.Assert(MagicNumber);
            result.Header = DdsFileHeader.Read(inputStream);

            if (result.Header.IsDx10())
            {
                result.HeaderDx10 = DdsFileHeaderDx10.Read(inputStream);
            }
            MemoryStream dataStream = new MemoryStream();
            inputStream.CopyTo(dataStream);
            result.Data = dataStream.ToArray();
            return result;
        }

        public void Write(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream, Encoding.Default, true);
            writer.Write(MagicNumber);
            Header.Write(outputStream);
            if (Header.IsDx10())
            {
                HeaderDx10.Write(outputStream);
            }
            writer.Write(Data);
        }
    }
}
