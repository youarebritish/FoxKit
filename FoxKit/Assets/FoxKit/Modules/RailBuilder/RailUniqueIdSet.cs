namespace FoxKit.Modules.RailBuilder
{
    using UnityEngine;

    /// <summary>
    /// Set of hashed rail IDs.
    /// </summary>
    [CreateAssetMenu(fileName = "New Rail Unique Id Set", menuName = "FoxKit/Rail Unique Id Set", order = 4)]
    public class RailUniqueIdSet : ScriptableObject
    {
        public uint[] Ids = new uint[0];
    }
}