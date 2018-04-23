using System.Collections.Generic;

namespace FoxTool.Fox
{
    public class FoxStringLiteralLookupTable
    {
        public FoxStringLiteralLookupTable()
            : this(new Dictionary<ulong, FoxStringLiteralBase>())
        {
        }

        public FoxStringLiteralLookupTable(Dictionary<ulong, FoxStringLiteralBase> globalLookupTable)
            : this(globalLookupTable, new Dictionary<ulong, FoxStringLiteralBase>())
        {
        }

        public FoxStringLiteralLookupTable(Dictionary<ulong, FoxStringLiteralBase> globalLookupTable,
            Dictionary<ulong, FoxStringLiteralBase> localLookupTable)
        {
            GlobalLookupTable = globalLookupTable;
            LocalLookupTable = localLookupTable;
        }

        private Dictionary<ulong, FoxStringLiteralBase> GlobalLookupTable { get; set; }
        private Dictionary<ulong, FoxStringLiteralBase> LocalLookupTable { get; set; }

        public FoxStringLiteralBase Lookup(ulong hash)
        {
            FoxStringLiteralBase result;
            if (LocalLookupTable.TryGetValue(hash, out result))
                return result;
            if (GlobalLookupTable.TryGetValue(hash, out result))
                return result;
            return null;
        }
    }
}
