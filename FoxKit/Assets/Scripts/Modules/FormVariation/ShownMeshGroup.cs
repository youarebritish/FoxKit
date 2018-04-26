namespace FoxKit.Modules.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
                
    /// <summary>
    /// A form variation operation used by the Fox Engine used to show a given mesh group.
    /// </summary>
    [System.Serializable]
    public struct ShownMeshGroup
    {
        public StringHashPair MeshGroupName;

        /// <summary>
        /// Creates a FoxKit ShownMeshGroup hash from a FoxLib ShownMeshGroup.
        /// </summary>
        /// <param name="hash">The FoxLib ShownMeshGroup hash to convert.</param>
        /// <returns>The created FoxKit ShownMeshGroup.</returns>
        public ShownMeshGroup(uint hash)
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