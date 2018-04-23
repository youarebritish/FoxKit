using System.Collections.Generic;

namespace FoxTool.Fox.Enums
{
    public class FoxSwitch : IFoxEnum
    {
        protected readonly List<FoxEnumValue> _values;

        protected FoxSwitch()
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
