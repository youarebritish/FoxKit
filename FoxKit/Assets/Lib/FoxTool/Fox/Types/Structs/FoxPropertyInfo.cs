using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    class FoxPropertyInfo : FoxStruct
    {
        public override void Read(Stream input)
        {
            throw new NotImplementedException();
        }

        public override void Write(Stream output)
        {
            throw new NotImplementedException();
        }

        public override int Size()
        {
            throw new NotImplementedException();
        }

        public override void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
            throw new NotImplementedException();
        }

        public override void CalculateHashes()
        {
            throw new NotImplementedException();
        }

        public override void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
            throw new NotImplementedException();
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
