namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;
      
    /// <summary>
    /// A form variation operation used by the Fox Engine used to hide a given mesh group.
    /// </summary>
    [System.Serializable]
    public struct HiddenMeshGroup
    {
        public StrCode32StringPair MeshGroupName;

        /// <summary>
        /// Initializes a new instance of the HiddenMeshGroup struct.
        /// </summary>
        /// <param name="meshGroupName">Material instance name.</param>
        public HiddenMeshGroup(StrCode32StringPair meshGroupName)
        {
            this.MeshGroupName = meshGroupName;
        }

        /// <summary>
        /// Creates a FoxKit HiddenMeshGroup hash from a FoxLib HiddenMeshGroup.
        /// </summary>`
        /// <param name="hash">The FoxLib HiddenMeshGroup hash to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <returns>The created FoxKit HiddenMeshGroup.</returns>
        public static HiddenMeshGroup MakeFoxKitHiddenMeshGroup(uint hash, StrCode32HashManager nameHashManager)
        {
            return new HiddenMeshGroup(nameHashManager.GetStringPairFromUnhashAttempt(hash));
        }

        /// <summary>
        /// Creates a FoxLib HiddenMeshGroup hash from a FoxLib HiddenMeshGroup.
        /// </summary>`
        /// <param name="shownMeshGroup">The FoxKit HiddenMeshGroup hash to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <returns>The created FoxLib HiddenMeshGroup (uint).</returns>
        public static uint MakeFoxLibHiddenMeshGroup(HiddenMeshGroup shownMeshGroup, StrCode32HashManager nameHashManager)
        {
            return nameHashManager.GetHashFromStringPair(shownMeshGroup.MeshGroupName);
        }
    }
}