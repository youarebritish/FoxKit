namespace FoxKit.Modules.MaterialDatabase.Importer
{
    using System.Linq;
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;
    using FoxKit.Modules.MaterialDatabase;

    /// <summary>
    /// ScriptedImporter to handle importing fmtt files.
    /// </summary>
    [ScriptedImporter(1, "fmtt")]
    public class MaterialDatabaseImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .fmtt file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            FoxLib.MaterialParamBinary.MaterialPreset[] materialPresets = null;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                var readFunction = new FoxLib.MaterialParamBinary.ReadFunction(reader.ReadSingle);
                materialPresets = FoxLib.MaterialParamBinary.Read(readFunction);
            }

            var materialDatabase = UnityEngine.ScriptableObject.CreateInstance<MaterialDatabase>();

            materialDatabase.materialPresets = (from preset in materialPresets select new MaterialPreset(preset)).ToArray();

            ctx.AddObjectToAsset("fmtt", materialDatabase);
            ctx.SetMainObject(materialDatabase);
        }
    }
}