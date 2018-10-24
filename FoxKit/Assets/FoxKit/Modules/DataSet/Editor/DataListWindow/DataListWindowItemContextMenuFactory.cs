namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    public static class DataListWindowItemContextMenuFactory
    {
        public delegate void ShowItemContextMenuDelegate(string dataSetPath, DataSet dataSet);

        public static ShowItemContextMenuDelegate Create(GenericMenu.MenuFunction2 setActiveDataSet, GenericMenu.MenuFunction2 onRemoveDataSet)
        {
            return (dataSetPath, dataSet) => ShowContextMenu(dataSetPath, dataSet, setActiveDataSet, onRemoveDataSet);
        }

        private static void ShowContextMenu(string dataSetGuid, DataSet dataSet, GenericMenu.MenuFunction2 setActiveDataSet, GenericMenu.MenuFunction2 onRemoveDataSet)
        {
            var menu = new GenericMenu();
            AddMenuItem(menu, "Set Active DataSet", setActiveDataSet, dataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Export DataSet", SaveDataSetAs, dataSet);

            menu.AddSeparator(string.Empty);
            
            AddMenuItem(menu, "Unload DataSet", OnSetActiveDataSet);
            AddMenuItem(menu, "Remove DataSet", onRemoveDataSet, dataSetGuid);

            menu.AddSeparator(string.Empty);
            
            AddMenuItem(menu, "Select DataSet Asset", SelectDataSetAsset, dataSetGuid);
            AddMenuItem(menu, "Add Entity", OnAddEntity);

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

        private static void OnAddEntity()
        {
            AddEntityWindow.Create(typeof(Data), false, type => DataListWindow.GetInstance().AddEntity(type));
        }

        private static void SaveDataSetAs(object dataSet)
        {
            var castDataSet = dataSet as DataSet;
            var path = EditorUtility.SaveFilePanel("Export DataSet", string.Empty, castDataSet.Name + ".fox2", "fox2");
            if (path.Length == 0)
            {
                return;
            }
            
            var entities = new List<Entity> { castDataSet };
            // TODO entities.AddRange(((DataSet)dataSet).GetAllEntities());

            DataSetExporter.ExportDataSet(entities, path);
        }

        private static void SelectDataSetAsset(object dataSetPath)
        {
            var dataSetPathString = dataSetPath as string;
            Assert.IsFalse(string.IsNullOrEmpty(dataSetPathString));

            var dataSetAsset = AssetDatabase.LoadAssetAtPath<DataSetAsset>(dataSetPathString);
            Assert.IsNotNull(dataSetAsset);

            Selection.objects = new[] { dataSetAsset };
        }
    }
   
}