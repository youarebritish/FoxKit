using System;

namespace FtexTool.Ftex.Enum
{
    [Flags]
    public enum FtexUnknownFlags : short
    {
        Flag1 = 1,
        Flag2 = 16, 
        Flag3 = 256,

        // (~3%) Most files end with _clp
        Clp = 0,

        // (<1%) 
        Unknown = Flag1 | Flag2,

        // (~96%) All remaining files
        Default = Flag1 | Flag2 | Flag3
    }
}