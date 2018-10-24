namespace FoxKit.Modules.Gr.GrTexture.Importer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Profiling;
    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;
    using FoxKit.Modules.Gr.GrTexture.Utils;

    /// <summary>
    /// ScriptedImporter to handle importing ftex files.
    /// </summary>
    [ScriptedImporter(1, "ftex")]
    public class GrTextureImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .ftex file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            #region Readers
            var filepathSansExtension = Path.GetDirectoryName(assetPath) + "\\" + Path.GetFileNameWithoutExtension(assetPath);

            List<BinaryReader> binaryReaders = new List<BinaryReader>();
            binaryReaders.Add(new BinaryReader(new FileStream(assetPath, FileMode.Open)));

            for (int i = 1; i < 7; i++)
            {
                var file = filepathSansExtension + "." + i + ".ftexs";

                if (!File.Exists(file))
                    break;

                var fileStream = new FileStream(file, FileMode.Open);
                binaryReaders.Add(new BinaryReader(fileStream));
            }
            #endregion

            try
            {
                var readFunctions = (from reader in binaryReaders select new FoxLib.GrTexture.ReadFunctions(reader.ReadUInt16, reader.ReadUInt32, reader.ReadUInt64, reader.ReadByte, reader.ReadBytes, (numberOfBytes => SkipBytes(reader, numberOfBytes)), (bytePos => MoveStream(reader, bytePos)))).ToArray();

                FoxLib.GrTexture.GrTexture grTexture = FoxLib.GrTexture.Read(readFunctions);

                var textureFormat = GrTextureUtils.GetTextureFormat(grTexture.PixelFormat, grTexture.Depth);
                var isLinear = GrTextureUtils.GetLinear(grTexture.TextureType);

                if (grTexture.TextureType == FoxLib.GrTexture.TextureType.Cube)
                {
                    #region Cube
                    var texture = new Cubemap(grTexture.Width, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, UnityEngine.Experimental.Rendering.TextureCreationFlags.MipChain);

                    var textureData = DirectXTexHelper.Decompress(grTexture.Width, grTexture.Height, GrTextureUtils.GetDXGIFormat(grTexture.PixelFormat), grTexture.MipMapCount, grTexture.DDSData);

                    for (int i = 0; i < textureData.Length; i++)
                    {
                        var faceData = textureData[i];

                        {
                            var colours = new Color[faceData.Length / 4];

                            for (int j = 0; j < faceData.Length / 4; j++)
                            {
                                var data = faceData.Skip(j * 4).Take(4).ToArray();

                                float R = data[0];
                                float G = data[1];
                                float B = data[2];
                                float A = data[3];

                                float nrmR = R / 255.0f;
                                float nrmG = G / 255.0f;
                                float nrmB = B / 255.0f;
                                float nrmA = A / 255.0f;

                                colours[j] = new Color(nrmR,
                                                       nrmG,
                                                       nrmB,
                                                       nrmA);
                            }

                            texture.SetPixels(colours, (CubemapFace)i);
                        }
                    }

                    texture.Apply(true);

                    ctx.AddObjectToAsset("ftex", texture);
                    ctx.SetMainObject(texture);

                    #endregion
                }
                else
                {
                    if (grTexture.Depth == 1)
                    {
                        #region 2D

                        var hasMipmaps = grTexture.MipMapCount > 1;
                        var texture = new Texture2D(grTexture.Width, grTexture.Height, textureFormat, hasMipmaps, isLinear);

                        if (hasMipmaps)
                        {
                            if (texture.mipmapCount > grTexture.MipMapCount)
                            {
                                var length = grTexture.DDSData.Length;
                                IntPtr pixelBuffer = DirectXTexHelper.GenerateNecessaryMipMapsAndFlipImage(grTexture.Width, grTexture.Height, GrTextureUtils.GetDXGIFormat(grTexture.PixelFormat), grTexture.MipMapCount, grTexture.DDSData, ref length);
                                texture.LoadRawTextureData(pixelBuffer, length);
                            }
                            else
                            {
                                texture.LoadRawTextureData(grTexture.DDSData);
                            }
                        }
                        else
                        {
                            byte[] newPixelData = DirectXTexHelper.Flip2DImage(grTexture.Width, grTexture.Height, GrTextureUtils.GetDXGIFormat(grTexture.PixelFormat), grTexture.MipMapCount, grTexture.DDSData);
                            texture.LoadRawTextureData(newPixelData);
                            texture.Apply();
                        }

                        ctx.AddObjectToAsset("ftex", texture, texture);
                        ctx.SetMainObject(texture);

                        #endregion
                    }
                    else
                    {
                        #region 3D

                        var texture = GrTextureUtils.CreateTexture3D(grTexture.Width, grTexture.Height, grTexture.Depth, GrTextureUtils.GetTextureFormat(grTexture.PixelFormat, grTexture.Depth), grTexture.DDSData);

                        ctx.AddObjectToAsset("ftex", texture);
                        ctx.SetMainObject(texture);
                        #endregion
                    }
                }

                this.userData = "NrtFlag: " + grTexture.NrtFlag + ", TextureType: " + grTexture.TextureType + ", UnknownFlags: " + grTexture.UnknownFlags;
            }
            finally
            {
                foreach (var reader in binaryReaders)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Skip reading a number of bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="numberOfBytes">The number of bytes to skip.</param>
        private static void SkipBytes(BinaryReader reader, long numberOfBytes)
        {
            reader.BaseStream.Position += numberOfBytes;
        }

        /// <summary>
        /// Move stream to a given position in bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="bytePos">The byte number to move the stream position to.</param>
        private static void MoveStream(BinaryReader reader, long bytePos)
        {
            reader.BaseStream.Position = bytePos;
        }

        private static FoxLib.GrTexture.ReadFunctions[] GetReadFunctions(string filepath)
        {
            var filepathSansExtension = Path.GetDirectoryName(filepath) + "\\" + Path.GetFileNameWithoutExtension(filepath);

            int[] textureIndices = { 1, 2, 3, 4, 5, 6 };
            string[] filepaths = (from index in textureIndices select filepathSansExtension + "." + index + ".ftexs").ToArray();
            string[] existingFilepaths = (from path in filepaths where File.Exists(path) select path).ToArray();
            FileStream[] streams = (from path in existingFilepaths select new FileStream(path, FileMode.Open)).ToArray();
            System.Collections.Generic.List<BinaryReader> readers = ((from stream in streams select new BinaryReader(stream)).ToList());
            readers.Insert(0, new BinaryReader(new FileStream(filepath, FileMode.Open)));
            var binaryReaders = readers.ToArray();

            return (from reader in binaryReaders select new FoxLib.GrTexture.ReadFunctions(reader.ReadUInt16, reader.ReadUInt32, reader.ReadUInt64, reader.ReadByte, reader.ReadBytes, (numberOfBytes => SkipBytes(reader, numberOfBytes)), (bytePos => MoveStream(reader, bytePos)))).ToArray();
        }
    }
}