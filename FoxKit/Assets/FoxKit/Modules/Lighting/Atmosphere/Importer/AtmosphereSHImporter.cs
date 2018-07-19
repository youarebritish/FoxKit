namespace FoxKit.Modules.Lighting.Atmosphere.Importer
{
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;
    using System;

    /// <summary>
    /// ScriptedImporter to handle importing atsh files.
    /// </summary>
    [ScriptedImporter(1, "atsh")]
    public class AtmosphereSHImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .atsh file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = UnityEngine.ScriptableObject.CreateInstance<AtmosphereSHCoefficients>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                reader.BaseStream.Seek(256L, SeekOrigin.Begin);
            }

            ctx.AddObjectToAsset(asset.name, asset);
            ctx.SetMainObject(asset);
        }
    }

}