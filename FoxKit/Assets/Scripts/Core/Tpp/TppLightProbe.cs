using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TppLightProbe : MonoBehaviour
{
    public Matrix4x4[] ParamSH = new Matrix4x4[4];
    public Matrix4x4[] ParamSHSky = new Matrix4x4[3];

    public Material ShSphereMapMaterial;

    void OnDrawGizmos()
    {
        if (ShSphereMapMaterial)
        {
            this.ShSphereMapMaterial.SetMatrixArray("_ParamSH", ParamSH);
            this.ShSphereMapMaterial.SetMatrixArray("_ParamSHSky", ParamSHSky);
        }
    }
}
