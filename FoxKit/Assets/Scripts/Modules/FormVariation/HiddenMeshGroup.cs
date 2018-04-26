namespace FoxKit.Modules.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
      
    /// <summary>
    /// A form variation operation used by the Fox Engine used to hide a given mesh group.
    /// </summary>
    [System.Serializable]
    public struct HiddenMeshGroup
    {
        public StringHashPair MeshGroupName;

        /// <summary>
        /// Creates a FoxKit HiddenMeshGroup hash from a FoxLib HiddenMeshGroup.
        /// </summary>`
        /// <param name="hash">The FoxLib HiddenMeshGroup hash to convert.</param>
        /// <returns>The created FoxKit HiddenMeshGroup.</returns>
        public HiddenMeshGroup(ulong hash)
        {
            string name;

            if (Hashing.TryGetFileNameFromHash(hash, out name) == true)
            {
                this.MeshGroupName.Name = name;
                this.MeshGroupName.IsHash = false;
            }
            else
            {
                this.MeshGroupName.Name = hash.ToString();
                this.MeshGroupName.IsHash = true;
            }
        }
    }
}