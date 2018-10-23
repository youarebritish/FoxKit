namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;
                
    /// <summary>
    /// A form variation operation used by the Fox Engine used to show a given mesh group.
    /// </summary>
    [System.Serializable]
    public struct ShownMeshGroup
    {
        public StrCode32StringPair MeshGroupName;

        /// <summary>
        /// Initializes a new instance of the ShownMeshGroup struct.
        /// </summary>
        /// <param name="meshGroupName">Material instance name.</param>
        public ShownMeshGroup(StrCode32StringPair meshGroupName)
        {
            this.MeshGroupName = meshGroupName;
        }

        /// <summary>
        /// Creates a FoxKit ShownMeshGroup hash from a FoxLib ShownMeshGroup.
        /// </summary>`
        /// <param name="hash">The FoxLib ShownMeshGroup hash to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <returns>The created FoxKit ShownMeshGroup.</returns>
        public static ShownMeshGroup MakeFoxKitShownMeshGroup(uint hash, StrCode32HashManager nameHashManager)
        {
            return new ShownMeshGroup(nameHashManager.GetStringPairFromUnhashAttempt(hash));
        }

        /// <summary>
        /// Creates a FoxLib ShownMeshGroup hash from a FoxLib ShownMeshGroup.
        /// </summary>`
        /// <param name="shownMeshGroup">The FoxKit ShownMeshGroup hash to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <returns>The created FoxLib ShownMeshGroup (uint).</returns>
        public static uint MakeFoxLibShownMeshGroup(ShownMeshGroup shownMeshGroup, StrCode32HashManager nameHashManager)
        {
            return nameHashManager.GetHashFromStringPair(shownMeshGroup.MeshGroupName);
        }
    }
}