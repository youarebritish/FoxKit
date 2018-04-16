using System.IO;
using System.Linq;

using UnityEngine.Assertions;

namespace FoxKit.Modules.MaterialDatabase.Exporter
{
    /// <summary>
    /// Collection of helper functions for exporting MaterialDatabase to fmtt format.
    /// </summary>
    public static class MaterialDatabaseExporter
    {
        /// <summary>
        /// Converts a FoxKit material preset to a FoxLib material preset.
        /// </summary>
        /// <param name="foxKitMaterialPreset"></param>
        /// <returns></returns>
        private static FoxLib.MaterialParamBinary.MaterialPreset convertTest(MaterialPreset foxKitMaterialPreset)
        {
            FoxLib.MaterialParamBinary.MaterialPreset foxLibMaterialPreset = new FoxLib.MaterialParamBinary.MaterialPreset(foxKitMaterialPreset.F0, foxKitMaterialPreset.RoughnessThreshold, foxKitMaterialPreset.ReflectionDependDiffuse, foxKitMaterialPreset.AnisotropicRoughness, Utils.FoxUtils.UnityColorToFoxColorRGB(foxKitMaterialPreset.SpecularColor), foxKitMaterialPreset.Translucency);
            return foxLibMaterialPreset;
        }

        /// <summary>
        /// Exports MaterialParams to an fmtt file.
        /// </summary>
        /// <param name="materialPresets">The Material Preset array to export.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportMaterialDatabase(MaterialPreset[] foxKitMaterialPresets, string exportPath)
        {
            var materialPresetCount = foxKitMaterialPresets.Length;
            Assert.IsTrue(materialPresetCount <= 256, "materialPresets count must be less than or equal to 256.");
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            FoxLib.MaterialParamBinary.MaterialPreset[] foxLibMaterialPresets = Enumerable.ToArray((from preset in foxKitMaterialPresets select (convertTest(preset))));

            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                var writeFunction = new FoxLib.MaterialParamBinary.WriteFunction(writer.Write);
                FoxLib.MaterialParamBinary.Write(foxLibMaterialPresets, writeFunction);
            }
        }
    }
}