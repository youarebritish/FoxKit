using FoxKit.Modules.DataSet.FoxCore;

namespace FoxKit.Modules.DataSet.GameCore
{
    public class GameObject : Data
    {
        public string TypeName;
        public uint GroupId;
        public uint TotalCount;
        public uint RealizedCount;
        public GameObjectParameter Parameters;
    }
}