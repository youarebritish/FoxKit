using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Modules.DataSet.GameCore;
using System;

namespace FoxKit.Modules.DataSet.GameCore
{
    /// <summary>
    /// Base class for Entities with a physical location in the world.
    /// </summary>
    [Serializable]
    public class GameObjectLocator : TransformData
    {
        public string TypeName;
        public uint GroupId;
        public GameObjectLocatorParameter Parameters;
    }
}