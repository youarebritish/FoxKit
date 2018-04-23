using System.Collections.Generic;

namespace FoxTool.Fox.Enums
{
    public class FoxFlags : IFoxEnum
    {
        private readonly List<FoxEnumValue> _values;

        public FoxFlags()
        {
            _values = new List<FoxEnumValue>();
        }

        public string Name { get; set; }

        public IEnumerable<FoxEnumValue> Values
        {
            get { return _values; }
        }
    }
}
