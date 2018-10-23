using System;
using System.IO;

using UnityEngine.Assertions;

namespace FoxKit.Modules.Atmosphere.SkyParameters.Exporter
{
    /// <summary>
    /// Collection of helper functions for exporting SkyParameters to pcsp format.
    /// </summary>
    public static class SkyParametersExporter
    {
        /// <summary>
        /// Exports SkyParameters to an pcsp file.
        /// </summary>
        /// <param name="texture">The Unity Texture2D to export.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportSkyParameters(UnityEngine.Texture2D texture, string exportPath)
        {
            Assert.IsTrue(texture.width == 128, "texture width must be equal to 128.");
            Assert.IsTrue(texture.height == 64, "texture height must be equal to 64.");
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            FoxLib.Core.HalfColorRGBA[] precomputedSkyParameters = Utils.FoxUtils.UnityTexture2DToFoxHalfColorRGBA(texture, 0);

            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                var writeFunction = new FoxLib.PrecomputedSkyParameters.WriteFunctions(new Action<Half>(half => writer.Write (Half.GetBytes(half))), writer.Write);
                FoxLib.PrecomputedSkyParameters.Write(precomputedSkyParameters, writeFunction);
            }
        }
    }
}