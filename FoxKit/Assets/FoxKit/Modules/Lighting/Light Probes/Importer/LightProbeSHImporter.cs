namespace FoxKit.Modules.Lighting.Atmosphere.Importer
{
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;
    using System;
    using System.Collections.Generic;
    using System.Text;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// ScriptedImporter to handle importing atsh files.
    /// </summary>
    [ScriptedImporter(1, "lpsh")]
    public class LightProbeSHImporter : ScriptedImporter
    {
        internal static void AlignRead(Stream input, int alignment)
        {
            long alignmentRequired = input.Position % alignment;
            if (alignmentRequired > 0)
                input.Position += alignment - alignmentRequired;
        }

        /// <summary>
        /// Import a .lpsh file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // Note: Doesn't seem to always load coefficients? Look at avr_stage.lpsh.

            var asset = new GameObject { name = Path.GetFileNameWithoutExtension(ctx.assetPath) };
            ctx.AddObjectToAsset(asset.name, asset);
            ctx.SetMainObject(asset);

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                var version = reader.ReadUInt32();
                Assert.IsTrue(version == 4);

                var unknown1 = reader.ReadUInt32();
                var fileSize = reader.ReadUInt32();

                reader.BaseStream.Seek(124L, SeekOrigin.Begin);

                var numLightProbes = reader.ReadUInt32();

                reader.BaseStream.Seek(140L, SeekOrigin.Begin);

                var numDivs = reader.ReadUInt32();

                reader.BaseStream.Seek(208L, SeekOrigin.Begin);
                reader.BaseStream.Seek(numDivs * sizeof(uint), SeekOrigin.Current);

                AlignRead(reader.BaseStream, 16);

                var lpMetadata = new List<LightProbeMetadata>();

                for (var i = 0; i < numLightProbes; i++)
                {
                    lpMetadata.Add(new LightProbeMetadata(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32()));
                }
                
                var lightProbes = new List<LightProbeSHCoefficients>();
                foreach (var metadata in lpMetadata)
                {
                    reader.BaseStream.Seek(metadata.NameAddress, SeekOrigin.Begin);

                    var nameBuilder = new StringBuilder();
                    var nextChar = reader.ReadChar();
                    while (nextChar != '\0')
                    {
                        nameBuilder.Append(nextChar);
                        nextChar = reader.ReadChar();
                    }
                    
                    var gameObject = new GameObject(nameBuilder.ToString());
                    var lightProbe = gameObject.AddComponent<LightProbeSHCoefficients>();
                    lightProbes.Add(lightProbe);
                }

                for (var i = 0; i < lightProbes.Count; i++)
                {
                    var metadata = lpMetadata[i];
                    reader.BaseStream.Seek(metadata.ShDataAddress, SeekOrigin.Begin);

                    var lightProbe = lightProbes[i];

                    var isLastProbe = i == lightProbes.Count - 1;
                    while (isLastProbe || reader.BaseStream.Position < lpMetadata[i + 1].ShDataAddress)
                    {
                        var sh = new LightProbeSHCoefficientSet();
                        for (var coefficientIndex = 0; coefficientIndex < 9; coefficientIndex++)
                        {
                            Func<float> readHalf = () => Half.ToHalf(reader.ReadUInt16());
                            var r = readHalf();
                            var g = readHalf();
                            var b = readHalf();
                            var skyOcclusion = readHalf();
                            
                            SetMatrixValue(ref sh.TermR, 15 - coefficientIndex, r);
                            SetMatrixValue(ref sh.TermG, 15 - coefficientIndex, g);
                            SetMatrixValue(ref sh.TermB, 15 - coefficientIndex, b);
                            SetMatrixValue(ref sh.SkyOcclusion, 15 - coefficientIndex, skyOcclusion);
                        }

                        if (reader.BaseStream.Position + 32L > fileSize)
                        {
                            break;
                        }

                        lightProbe.Coefficients.Add(sh);
                    }

                    lightProbe.transform.SetParent(asset.transform);
                    ctx.AddObjectToAsset(lightProbe.name, lightProbe);
                }
            }
        }

        private static void SetMatrixValue(ref Matrix4x4 matrix, int index, float value)
        {
            switch (index)
            {
                case 15:
                    matrix.m33 = value;
                    return;
                case 14:
                    matrix.m32 = value;
                    return;
                case 13:
                    matrix.m31 = value;
                    return;
                case 12:
                    matrix.m30 = value;
                    return;
                case 11:
                    matrix.m23 = value;
                    return;
                case 10:
                    matrix.m22 = value;
                    return;
                case 9:
                    matrix.m21 = value;
                    return;
                case 8:
                    matrix.m20 = value;
                    return;
                case 7:
                    matrix.m13 = value;
                    return;
                case 6:
                    matrix.m12 = value;
                    return;
                case 5:
                    matrix.m11 = value;
                    return;
                case 4:
                    matrix.m10 = value;
                    return;
                case 3:
                    matrix.m03 = value;
                    return;
                case 2:
                    matrix.m02 = value;
                    return;
                case 1:
                    matrix.m01 = value;
                    return;
                case 0:
                    matrix.m00 = value;
                    return;
            }
        }

        private class LightProbeMetadata
        {
            public uint NameAddress { get; }
            public uint ShDataAddress { get; }
            public uint Unknown { get; }

            public LightProbeMetadata(uint nameAddress, uint shDataAddress, uint unknown)
            {
                this.NameAddress = nameAddress;
                this.ShDataAddress = shDataAddress;
                this.Unknown = unknown;
            }
        }
    }

}