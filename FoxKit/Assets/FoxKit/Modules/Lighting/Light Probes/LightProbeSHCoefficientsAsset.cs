namespace FoxKit.Modules.Lighting.LightProbes
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
        
    public class LightProbeSHCoefficientsAsset : ScriptableObject
    {
        [SerializeField]
        public List<LightProbe> LightProbes = new List<LightProbe>();

        [SerializeField]
        public List<uint> TimeValues = new List<uint>();

        [Serializable]
        public class LightProbe
        {
            /// <summary>
            /// Name of the light probe.
            /// </summary>
            public string Name;

            [SerializeField]
            public List<ShCoefficientsSet> CoefficientsSets = new List<ShCoefficientsSet>();

            [Serializable]
            public class ShCoefficientsSet
            {
                public Matrix4x4 TermR = Matrix4x4.zero;
                public Matrix4x4 TermG = Matrix4x4.zero;
                public Matrix4x4 TermB = Matrix4x4.zero;
                public Matrix4x4 SkyOcclusion = Matrix4x4.zero;
            }
        }
    }
}