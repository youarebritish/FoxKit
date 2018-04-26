namespace FoxKit.Modules.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
                
    [System.Serializable]
    public struct TextureSwap
    {
        public StringHashPair MaterialInstanceName;
        public StringHashPair TextureTypeName;
        public StringHashPair TextureFileName;

        public TextureSwap(FoxLib.FormVariation.TextureSwap textureSwap)
        {
            uint materialInstanceHash = textureSwap.MaterialInstanceHash;
            uint textureTypeHash = textureSwap.TextureTypeHash;
            ulong textureFileNameHash = textureSwap.TextureFileHash;

            string materialInstanceName;

            if (Hashing.TryGetFileNameFromHash(materialInstanceHash, out materialInstanceName) == true)
            {
                this.MaterialInstanceName.Name = materialInstanceName;
                this.MaterialInstanceName.IsHash = false;
            }
            else
            {
                this.MaterialInstanceName.Name = materialInstanceHash.ToString();
                this.MaterialInstanceName.IsHash = true;
            }

            string textureTypeName; 

            if (Hashing.TryGetFileNameFromHash(textureTypeHash, out textureTypeName) == true)
            {
                this.TextureTypeName.Name = textureTypeName;
                this.TextureTypeName.IsHash = false;
            }
            else
            {
                this.TextureTypeName.Name = textureTypeHash.ToString();
                this.TextureTypeName.IsHash = true;
            }

            string textureFileName;

            if (Hashing.TryGetFileNameFromHash(textureFileNameHash, out textureFileName) == true)
            {
                this.TextureFileName.Name = textureFileName;
                this.TextureFileName.IsHash = false;
            }
            else
            {
                this.TextureFileName.Name = textureFileNameHash.ToString();
                this.TextureFileName.IsHash = true;
            }
        }
    }
}