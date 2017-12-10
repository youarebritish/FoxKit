using System;

namespace FtexTool.Exceptions
{
    [Serializable]
    public abstract class FtexToolException : ApplicationException
    {
        protected FtexToolException(string message) : base(message)
        {
        }
    }
}
