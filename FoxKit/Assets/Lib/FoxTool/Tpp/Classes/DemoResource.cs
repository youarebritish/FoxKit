using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DemoResource : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxString DemoIdentifierName { get; set; }
        public List<FoxString> DemoIdentifierKey { get; set; }
        public FoxUInt32 StreamBufferSizeInKbytes { get; set; }
        public FoxUInt32 SystemPacketSizeInKbytes { get; set; }
        public FoxUInt32 SystemPacketNum { get; set; }
        public FoxUInt32 SoundPacketSizeInKbytes { get; set; }
        public FoxUInt32 SoundPacketNum { get; set; }
        public FoxUInt32 DemoPacketSizeInKbytes { get; set; }
        public FoxUInt32 DemoPacketNum { get; set; }
    }
}
