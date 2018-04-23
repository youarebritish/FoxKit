using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FoxTool.Fox.Containers
{
    public interface IFoxContainer : IXmlSerializable
    {
        void Read(Stream input, short valueCount);
        void Write(Stream output);
        void ResolveStringLiterals(FoxLookupTable lookupTable);
        int Count();
        void CalculateHashes();
        void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals);
    }
}
