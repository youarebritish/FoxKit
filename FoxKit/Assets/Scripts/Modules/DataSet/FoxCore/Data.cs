using System;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public abstract class Data : Entity
    {
        /// <summary>
        /// Just use the ScriptableObject's name.
        /// </summary>
        public string Name => name;
    }
}