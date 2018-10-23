using System;
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
    }
}