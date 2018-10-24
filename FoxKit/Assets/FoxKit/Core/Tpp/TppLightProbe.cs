namespace FoxKit.Core.Tpp
{
    using UnityEngine;

    /// <summary>
    /// Visualizer for debugging lpsh data.
    /// </summary>
    [ExecuteInEditMode]
    public class TppLightProbe : MonoBehaviour
    {
        /// <summary>
        /// SH param data.
        /// </summary>
        public Matrix4x4[] ParamSH = new Matrix4x4[4];

        /// <summary>
        /// Sky SH param data.
        /// </summary>
        public Matrix4x4[] ParamSHSky = new Matrix4x4[3];

        /// <summary>
        /// SH visualization material.
        /// </summary>
        public Material ShSphereMapMaterial;

        /// <summary>
        /// Update the material.
        /// </summary>
        void OnDrawGizmos()
        {
            if (ShSphereMapMaterial)
            {
                this.ShSphereMapMaterial.SetMatrixArray("_ParamSH", ParamSH);
                this.ShSphereMapMaterial.SetMatrixArray("_ParamSHSky", ParamSHSky);
            }
        }
    }
}