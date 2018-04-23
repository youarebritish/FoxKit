using FoxTool.Fox.Types;

namespace FoxTool.Fox.Containers
{
    public class FoxDynamicArray<T> : FoxListBase<T> where T : IFoxValue, new()
    {
    }
}
