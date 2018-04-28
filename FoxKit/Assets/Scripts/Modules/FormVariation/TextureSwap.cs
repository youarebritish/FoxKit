namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
                
    [System.Serializable]
    public struct TextureSwap
    {
        public StrCode32StringPair MaterialInstanceName;
        public StrCode32StringPair TextureTypeName;
        public StrCode64StringPair TextureFileName;

        /// <summary>
        /// Initializes a new instance of the TextureSwap struct.
        /// </summary>
        /// <param name="materialInstanceName">Material instance name.</param>
        /// <param name="textureTypeName">Texture type name.</param>
        /// <param name="textureFileName">Texture file name.</param>
        public TextureSwap(StrCode32StringPair materialInstanceName, StrCode32StringPair textureTypeName, StrCode64StringPair textureFileName)
        {
            this.MaterialInstanceName = materialInstanceName;

            this.TextureTypeName = textureTypeName;

            this.TextureFileName = textureFileName;
        }

        /// <summary>
        /// Creates a FoxKit TextureSwap from a given FoxLib TextureSwap.
        /// </summary>
        /// <param name="textureSwap">The FoxLib TextureSwap.</param>
        /// <param name="fileHashManager">The </param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit TextureSwap.</returns>
        public static TextureSwap MakeFoxKitTextureSwap(FoxLib.FormVariation.TextureSwap textureSwap, StrCode32HashManager nameHashManager, StrCode64HashManager fileHashManager)
        {
            StrCode32StringPair newMaterialInstanceName = nameHashManager.GetStringPairFromUnhashAttempt(textureSwap.MaterialInstanceHash);
            StrCode32StringPair newTextureTypeName = nameHashManager.GetStringPairFromUnhashAttempt(textureSwap.TextureTypeHash);
            StrCode64StringPair newTextureFileName = fileHashManager.GetStringPairFromUnhashAttempt(textureSwap.TextureFileHash);

            return new TextureSwap(newMaterialInstanceName, newTextureTypeName, newTextureFileName);
        }

        /// <summary>
        /// Creates a FoxLib TextureSwap from a given FoxKit TextureSwap.
        /// </summary>
        /// <param name="textureSwap">The FoxKit TextureSwap.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxLib TextureSwap.</returns>
        public static FoxLib.FormVariation.TextureSwap MakeFoxLibTextureSwap(TextureSwap textureSwap, StrCode32HashManager nameHashManager, StrCode64HashManager fileHashManager)
        {
            uint materialInstanceHash = nameHashManager.GetHashFromStringPair(textureSwap.MaterialInstanceName);

            uint textureTypeHash = nameHashManager.GetHashFromStringPair(textureSwap.TextureTypeName);

            ulong textureFileHash = fileHashManager.GetHashFromStringPair(textureSwap.TextureFileName);

            return new FoxLib.FormVariation.TextureSwap(materialInstanceHash, textureTypeHash, textureFileHash);
        }
    }
}