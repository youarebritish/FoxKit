using FoxKit.Modules.Lighting.LightProbes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace FoxKit.Modules.DataSet.Fox.TppEffect
{
    public enum TppLightProbe_DebugMode : int
    {
        Off = 0,
        Cubemap = 1,
        SHValue = 2,
        SHOcclusion = 3,
        SHWithSky = 4,
        SHOnlyCol = 5,
        SHOnlySubCol = 6,
        SHOnlyLightAll = 7
    }

    public enum TppLightProbe_DrawRejectionLevel : int
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        NoReject = 7
    }

    public enum TppLightProbe_PackingGeneration : int
    {
        All = 0,
        Generation7 = 1,
        Generation8 = 2
    }

    public enum TppLightProbe_ShapeType : int
    {
        Default = 0,
        TrialgularPrism = 1,
        SemiCylindrical = 2,
        HalfSquare = 3
    }

    public partial class TppLightProbe
    {
        public override void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
            base.PostOnLoaded(getSceneProxy);

            // Get light probe group, or if none has been created, create it.
            var lightProbeGroups = (from probeGroup in GameObject.FindObjectsOfType<LightProbeGroup>()
                                   where probeGroup.name == this.DataSetGuid
                                   select probeGroup).ToArray();

            LightProbeGroup lightProbeGroup = null;
            if (lightProbeGroups.Length == 0)
            {
                lightProbeGroup = new GameObject(this.DataSetGuid).AddComponent<LightProbeGroup>();
                lightProbeGroup.probePositions = new Vector3[] { };
                lightProbeGroup.transform.position = Vector3.zero;
            }
            else
            {
                lightProbeGroup = lightProbeGroups[0];
            }

            if (this.shCoefficientsData.Entity == null)
            {
                return;
            }

            var probePositions = lightProbeGroup.probePositions;
            Array.Resize(ref probePositions, probePositions.Length + 1);
            probePositions[probePositions.Length - 1] = this.Transform.Translation;
            lightProbeGroup.probePositions = probePositions;

            var bakedProbes = LightmapSettings.lightProbes.bakedProbes;
            Array.Resize(ref probePositions, probePositions.Length + 1);

            var sh = new SphericalHarmonicsL2();
            var shData = GetShData();
            if (shData == null)
            {
                return;
            }

            sh[0, 1] = shData.CoefficientsSets[0].TermR.m00;
            sh[0, 2] = shData.CoefficientsSets[0].TermR.m01;
            sh[0, 3] = shData.CoefficientsSets[0].TermR.m02;
            sh[0, 4] = shData.CoefficientsSets[0].TermR.m03;

            sh[1, 1] = shData.CoefficientsSets[0].TermR.m10;
            sh[1, 2] = shData.CoefficientsSets[0].TermR.m11;
            sh[1, 3] = shData.CoefficientsSets[0].TermR.m12;
            sh[1, 4] = shData.CoefficientsSets[0].TermR.m13;

            sh[2, 1] = shData.CoefficientsSets[0].TermR.m20;
            sh[2, 2] = shData.CoefficientsSets[0].TermR.m21;
            sh[2, 3] = shData.CoefficientsSets[0].TermR.m22;
            sh[2, 4] = shData.CoefficientsSets[0].TermR.m23;

            LightmapSettings.lightProbes.bakedProbes = bakedProbes;
        }
        
        private LightProbeSHCoefficientsAsset.LightProbe GetShData()
        {
            var coefficientData = (this.shCoefficientsData.Entity as TppLightProbeSHCoefficients);
            var lpsh = coefficientData.LpshFile;
            return lpsh.LightProbes.FirstOrDefault(lp => lp.Name == this.Name);
        }
    }
}
