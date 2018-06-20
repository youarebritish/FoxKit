using UnityEngine;

namespace FoxKit.Utils
{
    using FoxLib;

    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

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
        /// Converts a Fox Engine Vector4 to a Unity Vector4.
        /// </summary>
        /// <param name="foxVector">The Fox Engine vector.</param>
        /// <returns>The Unity vector.</returns>
        public static UnityEngine.Vector4 FoxToUnity(FoxLib.Core.Vector4 foxVector)
        {
            return new UnityEngine.Vector4(foxVector.Z, foxVector.Y, foxVector.X, foxVector.Z);
        }

        /// <summary>
        /// Converts a Unity Vector4 to a Fox Engine Vector4.
        /// </summary>
        /// <param name="unityVector">The Unity vector.</param>
        /// <returns>The Fox Engine vector.</returns>
        public static FoxLib.Core.Vector3 UnityToFox(Vector3 unityVector)
        {
            return new FoxLib.Core.Vector3(unityVector.z, unityVector.y, unityVector.x);
        }

        /// <summary>
        /// Converts a Unity Vector4 to a Fox Engine Vector4.
        /// </summary>
        /// <param name="unityVector">The Unity vector.</param>
        /// <returns>The Fox Engine vector.</returns>
        public static FoxLib.Core.Vector4 UnityToFox(UnityEngine.Vector4 unityVector)
        {
            return new FoxLib.Core.Vector4(unityVector.z, unityVector.y, unityVector.x, unityVector.w);
        }

        /// <summary>
        /// Converts a Unity Quaternion to a Fox Engine Quaternion.
        /// </summary>
        /// <param name="unityQuaternion">The Unity Quaternion.</param>
        /// <returns>The Fox Engine Quaternion.</returns>
        public static FoxLib.Core.Quaternion UnityToFox(Quaternion unityQuaternion)
        {
            return new FoxLib.Core.Quaternion(-unityQuaternion.z, -unityQuaternion.y, -unityQuaternion.x, unityQuaternion.w);
        }

        public static UnityEngine.Quaternion FoxToUnity(Core.Quaternion foxQuat)
        {
            return new UnityEngine.Quaternion(-foxQuat.Z, -foxQuat.Y, -foxQuat.X, foxQuat.W);
        }

        /// <summary>
        /// Converts a Fox Engine ColorRGB to a Unity Color.
        /// </summary>
        /// <param name="foxColor">The Fox Engine ColorRGB.</param>
        /// <returns>The Unity color.</returns>
        public static Color FoxColorRGBToUnityColor(FoxLib.Core.ColorRGB foxColor)
        {
            return new Color(foxColor.Red, foxColor.Green, foxColor.Blue);
        }

        /// <summary>
        /// Converts a Unity Color to a Fox Engine ColorRGB.
        /// </summary>
        /// <param name="unityColor">The Unity color.</param>
        /// <returns>The Fox Engine color.</returns>
        public static FoxLib.Core.ColorRGB UnityColorToFoxColorRGB(Color unityColor)
        {
            return new FoxLib.Core.ColorRGB(unityColor.r, unityColor.g, unityColor.b);
        }

        /// <summary>
        /// Converts a Fox Engine ColorRGBA to a Unity Color.
        /// </summary>
        /// <param name="unityColor">The Fox Engine ColorRGBA.</param>
        /// <returns>The Unity color.</returns>
        public static Color FoxColorRGBAToUnityColor(FoxLib.Core.ColorRGBA foxColor)
        {
            return new Color(foxColor.Red, foxColor.Green, foxColor.Blue, foxColor.Alpha);
        }

        /// <summary>
        /// Converts a Unity Color to a Fox Engine ColorRGBA.
        /// </summary>
        /// <param name="unityColor">The Unity color.</param>
        /// <returns>The Fox Engine color.</returns>
        public static FoxLib.Core.ColorRGBA UnityColorToFoxColorRGBA(Color unityColor)
        {
            return new FoxLib.Core.ColorRGBA(unityColor.r, unityColor.g, unityColor.b, unityColor.a);
        }
    }
}