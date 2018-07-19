namespace FoxKit.Modules.Lighting.Atmosphere
{
    using System;

    using UnityEngine;

    [Serializable]
    public class AtmosphereSHCoefficientsSet
    {
        public Matrix4x4 Set0;
        public Matrix4x4 Set1;
        public Matrix4x4 Set2;
    }

    public class AtmosphereSHCoefficients : ScriptableObject
    {
        public AtmosphereSHCoefficientsSet[] ShSets;
    }
}