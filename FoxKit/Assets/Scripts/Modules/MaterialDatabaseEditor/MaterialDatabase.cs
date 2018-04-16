using UnityEngine;

namespace FoxKit.Modules.MaterialDatabaseEditor
{
    /// <summary>
    /// A material database which stores material presets.
    /// </summary>
    [CreateAssetMenu(fileName = "New Material Database", menuName = "FoxKit/Material Database")]
    public class MaterialDatabase : ScriptableObject
    {
        public MaterialPreset[] materialPresets;
    }
}