namespace FoxTool.Fox.Enums
{
    public class FoxEnumValue
    {
        public FoxEnumValue(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public int Value { get; private set; }
    }
}
