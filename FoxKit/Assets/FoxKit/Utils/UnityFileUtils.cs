namespace FoxKit.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    /// <summary>
    /// Collection of utility functions for working with files in Unity.
    /// </summary>
    public static class UnityFileUtils
    {
        /// <summary>
        /// Get the path for a new asset.
        /// </summary>
        /// <param name="filename">Filename of the new asset</param>
        /// <returns>Path for a new asset.</returns>
        public static string GetUniqueAssetPathNameOrFallback(string filename)
        {
            string path;
            try
            {
                // Private implementation of a filenaming function which puts the file at the selected path.
                System.Type assetdatabase = typeof(AssetDatabase);
                path = (string)assetdatabase.GetMethod("GetUniquePathNameAtSelectedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(assetdatabase, new object[] { filename });
            }
            catch
            {
                // Protection against implementation changes.
                path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + filename);
            }
            return path;
        }

        /// <summary>
        /// Get all assets of a given type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of asset to get.
        /// </typeparam>
        /// <returns>
        /// All assets of the given type..
        /// </returns>
        public static IEnumerable<T> GetAllAssetsOfType<T>() where T : UnityEngine.Object
        {
            return AssetDatabase.FindAssets($"t:{typeof(T)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null).ToList();
        }
    }
}