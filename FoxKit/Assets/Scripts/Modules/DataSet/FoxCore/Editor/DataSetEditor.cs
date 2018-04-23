using FoxKit.Modules.DataSet.Editor.Toolbar;
using FoxKit.Modules.DataSet.GameCore;
using System;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [CustomEditor(typeof(DataSet))]
    public class DataSetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            if (GUILayout.Button("New Entity"))
            {
                /*var menu = new GenericMenu();
                var types = Toolbar.ToolbarTypes;
                foreach(var type in types)
                {
                    AddEntityTypeToMenu(menu, type);
                }
                menu.ShowAsContext();*/
            }
        }

        void AddEntityTypeToMenu(GenericMenu menu, Type type)
        {
            menu.AddItem(new GUIContent(type.Namespace), false, OnEntityTypeSelected, type);
        }

        void OnEntityTypeSelected(object type)
        {
            var typeAsType = type as Type;
            var entry = CreateInstance(typeAsType) as Entity;
            entry.name = typeAsType.Name + "0000";

            AssetDatabase.AddObjectToAsset(entry, target);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(entry));

            (target as DataSet).DataList.Add(entry.name, entry);
        }

        [SerializeField]
        Color m_Color = Color.white;

        // a method to simplify adding menu items
        void AddMenuItemForColor(GenericMenu menu, string menuPath, Color color)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            menu.AddItem(new GUIContent(menuPath), m_Color.Equals(color), OnColorSelected, color);
        }

        // the GenericMenu.MenuFunction2 event handler for when a menu item is selected
        void OnColorSelected(object color)
        {
            m_Color = (Color)color;
        }
    }
}