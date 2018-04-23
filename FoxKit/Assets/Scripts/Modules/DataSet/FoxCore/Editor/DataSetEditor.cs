using FoxKit.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [CustomEditor(typeof(DataSet))]
    public class DataSetEditor : UnityEditor.Editor
    {
        // TODO Cache this somewhere so it's shared between menus
        // https://docs.unity3d.com/ScriptReference/Callbacks.DidReloadScripts.html ?
        // https://docs.unity3d.com/ScriptReference/EditorApplication-delayCall.html ?
        // https://docs.unity3d.com/ScriptReference/InitializeOnLoadMethodAttribute.html ?
        private static readonly Type[] TypesInAddMenu = ReflectionUtils.GetAssignableConcreteClasses(typeof(Data)).ToArray();

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            if (GUILayout.Button("New Entity"))
            {
                var menu = new GenericMenu();
                foreach(var type in TypesInAddMenu)
                {
                    AddEntityTypeToMenu(menu, type);
                }
                menu.ShowAsContext();
            }
        }

        void AddEntityTypeToMenu(GenericMenu menu, Type type)
        {
            menu.AddItem(new GUIContent(type.Name), false, OnEntityTypeSelected, type);
        }

        void OnEntityTypeSelected(object type)
        {
            var typeAsType = type as Type;
            var entry = CreateInstance(typeAsType) as Entity;

            var entitiesOfType = (target as DataSet).DataList.Values
                .Where(entity => entity.GetType() == typeAsType)
                .Count();

            entry.name = typeAsType.Name + entitiesOfType.ToString("D4");

            AssetDatabase.AddObjectToAsset(entry, target);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(entry));

            (target as DataSet).DataList.Add(entry.name, entry);
        }
    }
}