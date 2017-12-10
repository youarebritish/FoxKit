using System;

namespace FtexTool.Exceptions
{
    [Serializable]
    public class AssertionFailedException : FtexToolException
    {
        public AssertionFailedException(string message) : base(message)
        {
        }
    }
}
