using UnityEngine;
using FoxKit.Utils;

[System.Serializable]
/// <summary>
/// A Fox Engine material preset.
/// </summary>
public struct MaterialPreset
{
    public float F0;
    public float RoughnessThreshold;
    public float ReflectionDependDiffuse;
    public float AnisotropicRoughness;

    public Color SpecularColor;
    public float Translucency;

    /// <summary>
    /// Converts from a FoxLib MaterialPreset to a FoxKit MaterialPreset.
    /// </summary>
    /// <param name="foxLibMaterialPreset"></param>
    public MaterialPreset(FoxLib.MaterialParamBinary.MaterialPreset foxLibMaterialPreset)
    {
        this.F0 = foxLibMaterialPreset.F0;
        this.RoughnessThreshold = foxLibMaterialPreset.RoughnessThreshold;
        this.ReflectionDependDiffuse = foxLibMaterialPreset.ReflectionDependDiffuse;
        this.AnisotropicRoughness = foxLibMaterialPreset.AnisotropicRoughness;

        this.SpecularColor = FoxUtils.FoxColorRGBToUnityColor(foxLibMaterialPreset.SpecularColor);
        this.Translucency = foxLibMaterialPreset.Translucency;
    }
}