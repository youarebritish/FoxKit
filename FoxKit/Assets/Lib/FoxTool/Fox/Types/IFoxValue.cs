using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FoxTool.Fox.Types
{
    public interface IFoxValue : IXmlSerializable
    {
        void Read(Stream input);
        void Write(Stream output);
        int Size();
        void ResolveStringLiterals(FoxLookupTable lookupTable);
        void CalculateHashes();
        void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals);
    }
}
