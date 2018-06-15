namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine;
    
    public static class DataListWindowItemContextMenuFactory
    {
        public delegate void ShowItemContextMenuDelegate(DataSet dataSet);

        public static ShowItemContextMenuDelegate Create(GenericMenu.MenuFunction2 setActiveDataSet, GenericMenu.MenuFunction2 onRemoveDataSet)
        {
            return dataSet => ShowContextMenu(dataSet, setActiveDataSet, onRemoveDataSet);
        }

        private static void ShowContextMenu(DataSet dataSet, GenericMenu.MenuFunction2 setActiveDataSet, GenericMenu.MenuFunction2 onRemoveDataSet)
        {
            var menu = new GenericMenu();
            AddMenuItem(menu, "Set Active DataSet", setActiveDataSet, dataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Save DataSet", OnSetActiveDataSet);
            AddMenuItem(menu, "Export DataSet", SaveDataSetAs, dataSet);
            AddMenuItem(menu, "Save All", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Unload DataSet", OnSetActiveDataSet);
            AddMenuItem(menu, "Remove DataSet", onRemoveDataSet, dataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Discard changes", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Select DataSet Asset", OnSetActiveDataSet);
            AddMenuItem(menu, "Add Entity", OnSetActiveDataSet);

            menu.ShowAsContext();
        }

        private static void AddMenuItem(GenericMenu menu, string text, GenericMenu.MenuFunction callback)
        {
            menu.AddItem(new GUIContent(text), false, callback);
        }

        private static void AddMenuItem(GenericMenu menu, string text, GenericMenu.MenuFunction2 callback, object userData)
        {
            menu.AddItem(new GUIContent(text), false, callback, userData);
        }

        private static void OnSetActiveDataSet()
        {
            // TODO
        }

        private static void SaveDataSetAs(object dataSet)
        {
            var path = EditorUtility.SaveFilePanel("Export DataSet", string.Empty, dataSet + ".fox2", "fox2");
            if (path.Length == 0)
            {
                return;
            }
            
            var entities = new List<Entity> { dataSet as DataSet };
            entities.AddRange(((DataSet)dataSet).GetAllEntities());

            DataSetExporter.ExportDataSet(entities, path);
        }
    }
   
}