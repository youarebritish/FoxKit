using FoxKit.Modules.DataSet.FoxCore;

namespace FoxKit.Core
{
    public static class FoxKitEditor
    {
        public static Entity InspectedEntity { get; set; }
    }

    public class FoxStringRef
    {
        private string @string;

        private FoxStringRef(string value)
        {
            @string = value;
        }

        public static explicit operator string(FoxStringRef value)
        {
            return value.@string;
        }

        public static explicit operator FoxStringRef(string value)
        {
            return new FoxStringRef(value);
        }
    }
}