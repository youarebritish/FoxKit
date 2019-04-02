using UnityEngine;

namespace FoxKit.Utils
{
    using System;

    using FoxKit.Utils.Structs;

    using FoxLib;

    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;
    using Vector4 = UnityEngine.Vector4;

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
        public static Vector3 FoxToUnity(Core.Vector3 foxVector)
        {
            return new Vector3(-foxVector.X, foxVector.Y, foxVector.Z);
        }

        /// <summary>
        /// Converts a Fox Engine Vector4 to a Unity Vector4.
        /// </summary>
        /// <param name="foxVector">The Fox Engine vector.</param>
        /// <returns>The Unity vector.</returns>
        public static Vector4 FoxToUnity(Core.Vector4 foxVector)
        {
            return new Vector4(-foxVector.X, foxVector.Y, foxVector.Z, foxVector.W);
        }

        /// <summary>
        /// Converts a Unity Vector4 to a Fox Engine Vector4.
        /// </summary>
        /// <param name="unityVector">The Unity vector.</param>
        /// <returns>The Fox Engine vector.</returns>
        public static Core.Vector3 UnityToFox(Vector3 unityVector)
        {
            return new Core.Vector3(-unityVector.x, unityVector.y, unityVector.z);
        }

        /// <summary>
        /// Converts a Unity Vector4 to a Fox Engine Vector4.
        /// </summary>
        /// <param name="unityVector">The Unity vector.</param>
        /// <returns>The Fox Engine vector.</returns>
        public static Core.Vector4 UnityToFox(Vector4 unityVector)
        {
            return new Core.Vector4(-unityVector.x, unityVector.y, unityVector.z, unityVector.w);
        }

        /// <summary>
        /// Converts a Unity Quaternion to a Fox Engine Quaternion.
        /// </summary>
        /// <param name="unityQuaternion">The Unity Quaternion.</param>
        /// <returns>The Fox Engine Quaternion.</returns>
        public static Core.Quaternion UnityToFox(Quaternion unityQuaternion)
        {
            var angle = 0.0f;
            var axis = Vector3.zero;
            unityQuaternion.ToAngleAxis(out angle, out axis);
            axis.x = -axis.x;

            var newQuat = Quaternion.AngleAxis(-angle, axis);
            return new Core.Quaternion(newQuat.x, newQuat.y, newQuat.z, newQuat.w);
        }

        /// <summary>
        /// Converts a Fox Engine Quaternion to a Unity Quaternion.
        /// </summary>
        /// <param name="foxQuat">The Fox Engine Quaternion.</param>
        /// <returns>The Unity Quaternion.</returns>
        public static Quaternion FoxToUnity(Core.Quaternion foxQuat)
        {
            var angle = 0.0f;
            var axis = Vector3.zero;
            var unityQuaternion = new Quaternion(foxQuat.X, foxQuat.Y, foxQuat.Z, foxQuat.W);
            unityQuaternion.ToAngleAxis(out angle, out axis);
            axis.x = -axis.x;

            var newQuat = Quaternion.AngleAxis(-angle, axis);
            return new Quaternion(newQuat.x, newQuat.y, newQuat.z, newQuat.w);
        }

        /// <summary>
        /// Converts a Unity Matrix3x3 to a Fox Engine Matrix3.
        /// </summary>
        /// <param name="unityMatrix">The Unity Matrix3x3.</param>
        /// <returns>The Fox Engine Matrix3.</returns>
        public static Core.Matrix3 UnityToFox(Matrix3x3 unityMatrix)
        {
            return Core.Matrix3.Create(
                unityMatrix.m02,
                unityMatrix.m01,
                unityMatrix.m00,
                unityMatrix.m12,
                unityMatrix.m11,
                unityMatrix.m10,
                unityMatrix.m22,
                unityMatrix.m21,
                unityMatrix.m20);
        }

        /// <summary>
        /// Converts a Fox Engine Matrix3 to a Unity Matrix3x3.
        /// </summary>
        /// <param name="foxMatrix">The Fox Engine Matrix3.</param>
        /// <returns>The Unity Matrix3x3.</returns>
        public static Matrix3x3 FoxToUnity(Core.Matrix3 foxMatrix)
        {
            // TODO: Are thees conversions correct?
            var result = new Matrix3x3();
            result.SetColumn(0, new Vector3(-foxMatrix.Col0[0], foxMatrix.Col0[1], foxMatrix.Col0[2]));
            result.SetColumn(1, new Vector3(-foxMatrix.Col1[0], foxMatrix.Col1[1], foxMatrix.Col1[2]));
            result.SetColumn(2, new Vector3(-foxMatrix.Col2[0], foxMatrix.Col2[1], foxMatrix.Col2[2]));
            return result;
        }

        /// <summary>
        /// Converts a Unity Matrix4x4 to a Fox Engine Matrix4.
        /// </summary>
        /// <param name="unityMatrix">The Unity Matrix4x4.</param>
        /// <returns>The Fox Engine Matrix4.</returns>
        public static Core.Matrix4 UnityToFox(Matrix4x4 unityMatrix)
        {
            return Core.Matrix4.Create(
                -unityMatrix.m00,
                unityMatrix.m01,
                unityMatrix.m02,
                unityMatrix.m03,
                -unityMatrix.m10,
                unityMatrix.m11,
                unityMatrix.m12,
                unityMatrix.m13,
                -unityMatrix.m20,
                unityMatrix.m21,
                unityMatrix.m22,
                unityMatrix.m23,
                -unityMatrix.m30,
                unityMatrix.m31,
                unityMatrix.m32,
                unityMatrix.m33);
        }

        /// <summary>
        /// Converts a Fox Engine Matrix4 to a Unity Matrix4x4.
        /// </summary>
        /// <param name="foxMatrix">The Fox Engine Matrix4.</param>
        /// <returns>The Unity Matrix4x4.</returns>
        public static Matrix4x4 FoxToUnity(Core.Matrix4 foxMatrix)
        {
            // TODO: Are thees conversions correct?
            var result = new Matrix4x4();
            result.SetColumn(0, new Vector4(-foxMatrix.Col0[0], foxMatrix.Col0[1], foxMatrix.Col0[2], foxMatrix.Col0[3]));
            result.SetColumn(1, new Vector4(-foxMatrix.Col1[0], foxMatrix.Col1[1], foxMatrix.Col1[2], foxMatrix.Col1[3]));
            result.SetColumn(2, new Vector4(-foxMatrix.Col2[0], foxMatrix.Col2[1], foxMatrix.Col2[2], foxMatrix.Col2[3]));
            result.SetColumn(3, new Vector4(-foxMatrix.Col3[0], foxMatrix.Col3[1], foxMatrix.Col3[2], foxMatrix.Col3[3]));
            return result;
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
        /// <param name="foxColor">The Fox Engine ColorRGBA.</param>
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


        /// <summary>
        /// Converts a Fox Engine HalfColorRGB to a Unity Color.
        /// </summary>
        /// <param name="foxColor">The Fox Engine HalfColorRGB.</param>
        /// <returns>The Unity color.</returns>
        public static Color FoxHalfColorRGBToUnityColor(FoxLib.Core.HalfColorRGB foxColor)
        {
            return new Color(foxColor.Red, foxColor.Green, foxColor.Blue);
        }

        /// <summary>
        /// Converts a Unity Color to a Fox Engine HalfColorRGB.
        /// </summary>
        /// <param name="unityColor">The Unity color.</param>
        /// <returns>The Fox Engine color.</returns>
        public static FoxLib.Core.HalfColorRGB UnityColorToFoxHalfColorRGB(Color unityColor)
        {
            return new FoxLib.Core.HalfColorRGB((Half)unityColor.r, (Half)unityColor.g, (Half)unityColor.b);
        }

        /// <summary>
        /// Converts a Fox Engine HalfColorRGBA to a Unity Color.
        /// </summary>
        /// <param name="foxColor">The Fox Engine HalfColorRGBA.</param>
        /// <returns>The Unity color.</returns>
        public static Color FoxHalfColorRGBAToUnityColor(FoxLib.Core.HalfColorRGBA foxColor)
        {
            return new Color(foxColor.Red, foxColor.Green, foxColor.Blue, foxColor.Alpha);
        }

        /// <summary>
        /// Converts a Unity Color to a Fox Engine HalfColorRGBA.
        /// </summary>
        /// <param name="unityColor">The Unity color.</param>
        /// <returns>The Fox Engine color.</returns>
        public static FoxLib.Core.HalfColorRGBA UnityColorToFoxHalfColorRGBA(Color unityColor)
        {
            return new FoxLib.Core.HalfColorRGBA((Half)unityColor.r, (Half)unityColor.g, (Half)unityColor.b, (Half)unityColor.a);
        }

        /// <summary>
        /// Converts a Fox Engine HalfColorRGB array to a Unity Texture2D.
        /// </summary>
        /// <param name="width">Width of the to-be-constructed texture.</param>
        /// <param name="height">Height of the to-be-constructed texture.</param>
        /// <param name="foxColor">The Fox Engine color array.</param>
        /// <param name="mipmaps">If true, calculates mipmaps.</param>
        /// <param name="linear">If true, uses the linear color space.</param>
        /// <returns>The Unity Texture2D.</returns>
        public static Texture2D FoxHalfColorRGBToUnityTexture2D(int width, int height, FoxLib.Core.HalfColorRGB[] foxColor, bool mipmaps, bool linear)
        {
            var unityTexture = new Texture2D(width, height, TextureFormat.RGBAHalf, mipmaps, linear);

            for (int y = 0, coordinate = 0; y < unityTexture.height; y++)
            {
                for (int x = 0; x < unityTexture.width; x++)
                {
                    Color color = FoxHalfColorRGBToUnityColor(foxColor[coordinate]);
                    unityTexture.SetPixel(x, y, color);
                    coordinate++;
                }
            }

            return unityTexture;
        }

        /// <summary>
        /// Converts a Unity Texture2D to a Fox Engine HalfColorRGB array.
        /// </summary>
        /// <param name="unityTexture">The Unity Texture2D</param>
        /// <param name="mipLevel">The mip level of the texture to use.</param>
        /// <returns>The Fox Engine HalfColorRGB array.</returns>
        public static FoxLib.Core.HalfColorRGB[] UnityTexture2DToFoxHalfColorRGB(Texture2D unityTexture, int mipLevel)
        {
            var originalTexture = unityTexture.GetPixels(mipLevel);

            FoxLib.Core.HalfColorRGB[] foxTexture = new FoxLib.Core.HalfColorRGB[(unityTexture.width * unityTexture.height)];

            for (int i = 0; i < originalTexture.Length; i++)
            {
                foxTexture[i] = UnityColorToFoxHalfColorRGB(originalTexture[i]);
            }

            return foxTexture;
        }

        /// <summary>
        /// Converts a Fox Engine HalfColorRGBA array to a Unity Texture2D.
        /// </summary>
        /// <param name="width">Width of the to-be-constructed texture.</param>
        /// <param name="height">Height of the to-be-constructed texture.</param>
        /// <param name="foxColor">The Fox Engine color array.</param>
        /// <param name="mipmaps">If true, calculates mipmaps.</param>
        /// <param name="linear">If true, uses the linear color space.</param>
        /// <returns>The Unity Texture2D.</returns>
        public static Texture2D FoxHalfColorRGBAToUnityTexture2D(int width, int height, FoxLib.Core.HalfColorRGBA[] foxColor, bool mipmaps, bool linear)
        {
            var unityTexture = new Texture2D(width, height, TextureFormat.RGBAHalf, mipmaps, linear);

            for (int y = 0, coordinate = 0; y < unityTexture.height; y++)
            {
                for (int x = 0; x < unityTexture.width; x++)
                {
                    Color color = FoxHalfColorRGBAToUnityColor(foxColor[coordinate]);
                    unityTexture.SetPixel(x, y, color);
                    coordinate++;
                }
            }

            return unityTexture;
        }

        /// <summary>
        /// Converts a Unity Texture2D to a Fox Engine HalfColorRGBA array.
        /// </summary>
        /// <param name="unityTexture">The Unity Texture2D</param>
        /// <param name="mipLevel">The mip level of the texture to use.</param>
        /// <returns>The Fox Engine HalfColorRGBA array.</returns>
        public static FoxLib.Core.HalfColorRGBA[] UnityTexture2DToFoxHalfColorRGBA(Texture2D unityTexture, int mipLevel)
        {
            var originalTexture = unityTexture.GetPixels(mipLevel);

            FoxLib.Core.HalfColorRGBA[] foxTexture = new FoxLib.Core.HalfColorRGBA[(unityTexture.width * unityTexture.height)];

            for (int i = 0; i < originalTexture.Length; i++)
            {
                foxTexture[i] = UnityColorToFoxHalfColorRGBA(originalTexture[i]);
            }

            return foxTexture;
        }

        public static string UnityPathToFoxPath(string filePtr)
        {
            if (string.IsNullOrEmpty(filePtr))
            {
                return string.Empty;
            }

            // Fox Engine paths need to open with a /.
            return "/" + filePtr;
        }

        public static string FoxPathToUnityPath(string filePtr)
        {
            return FormatFilePath(filePtr);
        }

        private static string FormatFilePath(string path)
        {
            // Fox Engine paths open with a /, which Unity doesn't like.
            return string.IsNullOrEmpty(path) ? path : path.Substring(1);
        }
    }
}