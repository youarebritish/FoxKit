using System;

namespace FtexTool.Exceptions
{
    [Serializable]
    public class MissingFtexsFileException : FtexToolException
    {
        public MissingFtexsFileException(string message) : base(message)
        {
        }
    }
}
