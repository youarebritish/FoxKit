namespace FoxKit.Modules.Atmosphere.SkyParameters.Importer
{
    using System;
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;
    using FoxKit.Modules.Atmosphere.SkyParameters;

    /// <summary>
    /// ScriptedImporter to handle importing pcsp files.
    /// </summary>
    [ScriptedImporter(1, "pcsp")]
    public class SkyParametersImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .pcsp file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            FoxLib.Core.HalfColorRGBA[] pixels = null;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                var readFunctions = new FoxLib.PrecomputedSkyParameters.ReadFunctions(new Func<Half>(() => Half.ToHalf(reader.ReadUInt16())), reader.ReadUInt32);
                pixels = FoxLib.PrecomputedSkyParameters.Read(readFunctions);
            }

            var unityTexture = Utils.FoxUtils.FoxHalfColorRGBAToUnityTexture2D(128, 64, pixels, false, true);

            unityTexture.alphaIsTransparency = false;
            unityTexture.anisoLevel = 0;
            unityTexture.wrapMode = UnityEngine.TextureWrapMode.Clamp;

            SkyParameters skyParameters = UnityEngine.ScriptableObject.CreateInstance<SkyParameters>();

            skyParameters.precomputedSkyParameters = unityTexture;
            skyParameters.name = Path.GetFileNameWithoutExtension(assetPath);

            ctx.AddObjectToAsset("pcsp", skyParameters);
            ctx.SetMainObject(skyParameters);
            ctx.AddObjectToAsset("texture", unityTexture);
        }
    }
}