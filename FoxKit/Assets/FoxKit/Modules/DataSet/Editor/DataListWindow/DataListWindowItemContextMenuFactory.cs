namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
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
            AddMenuItem(menu, "Save DataSet As", OnSetActiveDataSet);
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
    }
   
}