namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using FoxKit.Core.WIP;
    using UnityEngine;
    using UnityEditor;
    using OdinSerializer;
                
    [System.Serializable]
    public class TextureSwap
    {
        public Material MaterialInstance;

        [OdinSerialize]
        public Str32CodeHashPair MaterialInstanceName;

        [OdinSerialize]
        public Str32CodeHashPair TextureType;

        public Texture Texture;

        [OdinSerialize]
        public PathFileNameCode64HashPair TextureFileName;

        public TextureSwap()
        {
            MaterialInstance = null;
            MaterialInstanceName = null;
            TextureType = null;
            Texture = null;
            TextureFileName = null;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
            public TextureSwap(Material material, string textureType, Texture texture)
        {
            this.MaterialInstance = material;
            this.TextureType = textureType;
            this.Texture = texture;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public TextureSwap(Str32CodeHashPair materialInstanceName, Str32CodeHashPair textureTypeName, PathFileNameCode64HashPair textureFileName)
        {
            this.MaterialInstanceName = materialInstanceName;

            this.TextureType = textureTypeName;

            this.Texture = AssetDatabase.LoadAssetAtPath<Texture>(textureFileName);
            this.TextureFileName = textureFileName;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public static TextureSwap Convert(FoxLib.FormVariation.TextureSwap textureSwap, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            Str32CodeHashPair materialInstanceName = textureSwap.MaterialInstanceHash;
            materialInstanceName.TryUnhashString(str32DictFunc);

            Str32CodeHashPair textureTypeName = textureSwap.TextureTypeHash;
            textureTypeName.TryUnhashString(str32DictFunc);

            PathFileNameCode64HashPair textureFileName = textureSwap.TextureFileHash;
            textureFileName.TryUnhashString(str64DictFunc);

            return new TextureSwap(materialInstanceName, textureTypeName, textureFileName);
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public FoxLib.FormVariation.TextureSwap Convert()
        {
            return new FoxLib.FormVariation.TextureSwap(this.MaterialInstanceName, this.TextureType, this.TextureFileName);
        }
    }
}