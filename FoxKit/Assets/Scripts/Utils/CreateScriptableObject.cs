namespace FoxKit.Utils
{
    using System.IO;

    using UnityEditor;

    using UnityEngine;

    public static class CreateScriptableObject
    {
        /// <summary>
        //	This makes it easy to create, name, and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            if (path == string.Empty)
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != string.Empty)
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).Name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }
    }
}