namespace FoxKit.Modules.Gr.GrTexture.Exporter
{
    using System;
    using System.IO;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEditor;
    using FoxKit.Core;
    using FoxKit.Modules.Gr.GrTexture.Utils;

    public static class GrTextureExporter
    {
        /// <summary>
        /// Exports a GrTexture to an .ftex file and a group of .ftexs files.
        /// </summary>
        /// <param name="foxKitFormVariation">The Form Variation to export.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportGrTexture(string assetPath, ushort width, ushort height, ushort depth, ushort format, byte mipmapCount, byte[] data, string exportPath)
        {
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            var userData = AssetImporter.GetAtPath(assetPath).userData;
            string[] separators = { "NrtFlag: ", ", TextureType: ", ", UnknownFlags: " };
            var flags = userData.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            byte nrtFlag;
            if (!byte.TryParse(flags[0], out nrtFlag))
            {
                Debug.Log($"Error: Incorrect nrt flag value: {flags[0]}");
                return;
            }
            else
            {
                if (!(nrtFlag == 0x0 || nrtFlag == 0x2))
                {
                    Debug.Log($"Error: Incorrect nrt flag value: {nrtFlag}");
                    return;
                }
            }

            FoxLib.GrTexture.TextureType textureType;
            if (!FoxLib.GrTexture.TextureType.TryParse(flags[1], out textureType))
            {
                Debug.Log($"Error: Incorrect texture type value: {flags[1]}");
                return;
            }

            FoxLib.GrTexture.UnknownFlags unknownFlags;
            if (!FoxLib.GrTexture.UnknownFlags.TryParse(flags[2], out unknownFlags))
            {
                Debug.Log($"Error: Incorrect unknown flags: {flags[2]}");
                return;
            }

            var flippedData = FoxKit.Modules.Gr.GrTexture.Utils.DirectXTexHelper.Flip2DImage(width, height, mipmapCount, GrTextureUtils.GetDXGIFormat(format), data);

            Debug.Log("The flipped data is " + ( (flippedData == data) ? "the same" : "not the same") + ".");

            data = flippedData;

            FoxLib.GrTexture.GrTexture grTexture = new FoxLib.GrTexture.GrTexture(height, width, depth, format, nrtFlag, textureType, unknownFlags, mipmapCount, data);

            var precomputedSlicePitches = FoxKit.Modules.Gr.GrTexture.Utils.DirectXTexHelper.ComputeSlicePitches((uint)grTexture.Width, (uint)grTexture.Height, GrTextureUtils.GetDXGIFormat(grTexture.PixelFormat), (uint)grTexture.MipMapCount);

            var ftexsFileCount = GetFtexsFileCount(grTexture.DDSData.Length, grTexture.MipMapCount);

            BinaryWriter[] writers = new BinaryWriter[ftexsFileCount + 1];
            try
            {
                var filepathSansExtension = Path.GetDirectoryName(exportPath) + "\\" + Path.GetFileNameWithoutExtension(exportPath);
                writers[0] = new BinaryWriter(new FileStream(exportPath, FileMode.Create));
                for (int i = 1; i < writers.Length; i++)
                {
                    writers[i] = new BinaryWriter(new FileStream(filepathSansExtension + "." + i + ".ftexs", FileMode.Create));
                }

                var writeFunctions = new FoxLib.GrTexture.WriteFunctions[writers.Length];

                for (int i = 0; i < writers.Length; ++i)
                {
                    var forClosureCopy = i;
                    writeFunctions[forClosureCopy] = new FoxLib.GrTexture.WriteFunctions(writers[forClosureCopy].Write, writers[forClosureCopy].Write, writers[forClosureCopy].Write, writers[forClosureCopy].Write, writers[forClosureCopy].Write, writers[forClosureCopy].Write, numberOfBytes => WriteEmptyBytes(writers[forClosureCopy], numberOfBytes), () => writers[forClosureCopy].BaseStream.Position);
                }

                FoxLib.GrTexture.Write(grTexture, writeFunctions, precomputedSlicePitches);
            }
            finally
            {
                foreach (var writer in writers)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Writes a number of empty bytes.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        /// <param name="numberOfBytes">The number of empty bytes to write.</param>
        private static void WriteEmptyBytes(BinaryWriter writer, int numberOfBytes)
        {
            writer.Write(new byte[numberOfBytes]);
        }

        private static byte GetFtexsFileCount(int fileSize, int mipMapCount)
        {
            if (fileSize <= 76456)
            {
                if (fileSize <= 19112)
                {
                    return 1;
                }

                if (mipMapCount <= 3)
                {
                    return 1;
                }

                return 2;
            }

            if (mipMapCount <= 4)
            {
                return 1;
            }

            return 3;
        }
    }
}