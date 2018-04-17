namespace FoxKit.Modules.RailBuilder
{
    using UnityEngine;

    /// <summary>
    /// Set of hashed rail IDs.
    /// </summary>
    [CreateAssetMenu(fileName = "New Rail Unique Id Set", menuName = "FoxKit/Rail/Rail Unique Id Set")]
    public class RailUniqueIdSet : ScriptableObject
    {
        public uint[] Ids = new uint[0];
    }
}