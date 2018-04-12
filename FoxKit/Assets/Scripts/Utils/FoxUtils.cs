using UnityEngine;

namespace FoxKit.Utils
{
    /// <summary>
    /// Helper functions for working with Fox Engine-formatted data.
    /// </summary>
    public static class FoxUtils
    {
        /// <summary>
        /// Converts a Fox Engine Vector3 to a Unity Vector3.
        /// </summary>
        /// <param name="foxVector">The Fox Engine vector.</param>
        /// <returns>The Unity vector.</returns>
        public static Vector3 FoxToUnity(FoxLib.Core.Vector3 foxVector)
        {
            return new Vector3(foxVector.Z, foxVector.Y, foxVector.X);
        }

        /// <summary>
        /// Converts a Unity Vector3 to a Fox Engine Vector3.
        /// </summary>
        /// <param name="foxVector">The Unity vector.</param>
        /// <returns>The Fox Engine vector.</returns>
        public static FoxLib.Core.Vector3 UnityToFox(Vector3 unityVector)
        {
            return new FoxLib.Core.Vector3(unityVector.z, unityVector.y, unityVector.x);
        }
    }
}