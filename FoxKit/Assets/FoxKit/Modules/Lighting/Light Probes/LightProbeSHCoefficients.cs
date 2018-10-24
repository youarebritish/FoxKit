namespace FoxKit.Modules.Lighting.Atmosphere
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    [Serializable]
    public class LightProbeSHCoefficientSet
    {
        public Matrix4x4 TermR = Matrix4x4.zero;
        public Matrix4x4 TermG = Matrix4x4.zero;
        public Matrix4x4 TermB = Matrix4x4.zero;
        public Matrix4x4 SkyOcclusion = Matrix4x4.zero;
    }
    
    public class LightProbeSHCoefficients : MonoBehaviour
    {
        public List<LightProbeSHCoefficientSet> Coefficients = new List<LightProbeSHCoefficientSet>();
    }
}