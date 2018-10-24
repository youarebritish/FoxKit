using UnityEngine;

namespace FoxKit.Modules.Atmosphere.SkyParameters
{
    /// <summary>
    /// A LUT which stores precomputed sky data.
    /// </summary>
    [CreateAssetMenu(fileName = "New Precomputed Sky Parameters", menuName = "FoxKit/PrecomputedSkyParameters")]
    public class SkyParameters : ScriptableObject
    {
        public Texture2D precomputedSkyParameters;
    }
}