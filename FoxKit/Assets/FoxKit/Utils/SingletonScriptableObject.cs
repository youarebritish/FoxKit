namespace FoxKit.Utils
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on editor, null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public abstract class SingletonScriptableObject<T> : ScriptableObject
        where T : ScriptableObject
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    T[] objs = null;

                    var objsGUID = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    var count = objsGUID.Length;
                    objs = new T[count];

                    for (int i = 0; i < count; i++)
                    {
                        objs[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(objsGUID[i]));
                    }
                    
                    if (objs.Length == 0)
                    {
                        Debug.LogError("No asset of type \"" + typeof(T).Name + "\" has been found in loaded resources. Attempting to create it.");
                        return CreateScriptableObject.CreateAsset<T>();
                    }
                    
                    else if (objs.Length > 1)
                    {
                        Debug.LogError("There's more than one asset of type \"" + typeof(T).Name + "\" loaded in this project. There should be exactly one asset of this type in the project.");
                        return null;
                    }

                    instance = (objs.Length > 0) ? objs[0] : null;
                }
                return instance;
            }
        }
    }
}