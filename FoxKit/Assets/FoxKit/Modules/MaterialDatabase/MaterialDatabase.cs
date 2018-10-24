using UnityEngine;

namespace FoxKit.Modules.MaterialDatabase
{
    /// <summary>
    /// A material database which stores material presets.
    /// </summary>
    [CreateAssetMenu(fileName = "New Material Database", menuName = "FoxKit/Material Database", order = 3)]
    public class MaterialDatabase : ScriptableObject
    {
        public MaterialPreset[] materialPresets;
    }
}