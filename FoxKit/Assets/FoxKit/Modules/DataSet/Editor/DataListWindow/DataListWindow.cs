namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class DataListWindow : EditorWindow
    {
        private const string PreferenceKeyOpenDataSets = "FoxKit.DataListWindow.OpenDataSets";
        
        public static bool IsOpen { get; private set; }

        /// <summary>
        /// DataSets currently open in the window.
        /// </summary>
        [SerializeField]
        private List<string> openDataSetGuids;
        
        /// <summary>
        /// Serializable state of the TreeView.
        /// </summary>
        [SerializeField]
        private TreeViewState treeViewState;

        private DataSet activeDataSet;

        [SerializeField]
        private string activeDataSetGuid;

        [SerializeField]
        private List<SceneProxyRecord> sceneProxyRecords = new List<SceneProxyRecord>();

        /// <summary>
        /// Tree view widget.
        /// </summary>
        private DataListTreeView treeView;
        
        public DataSet ActiveDataSet => this.activeDataSet;

        public DataListWindowItemContextMenuFactory.ShowItemContextMenuDelegate MakeShowItemContextMenuDelegate()
        {
            return DataListWindowItemContextMenuFactory.Create(this.SetActiveDataSet, dataSetGuid => this.RemoveDataSet(dataSetGuid as string));
        }

        /// <summary>
        /// Create a new Entity of a given type in the active DataSet.
        /// </summary>
        /// <param name="entityType">Type of the Entity to add.</param>
        public Data AddEntity(Type entityType)
        {
            var instance = Activator.CreateInstance(entityType) as Data;
            instance.Name = GenerateNameForType(entityType, this.activeDataSet);
            instance.DataSetGuid = this.activeDataSetGuid;

            this.ActiveDataSet.AddData(instance.Name, instance);
            
            // TODO
            // There must be a better way of doing this
            this.treeView = new DataListTreeView(
                this.treeViewState,
                this.openDataSetGuids,
                this.activeDataSet,
                this.FindSceneProxyForEntity);
            this.treeView.Reload();

            return instance;
        }

        /// <summary>
        /// Generates a name for a new Entity which is unique and valid for the given DataSet.
        /// </summary>
        /// <param name="type">The type of Entity whose name to generate.</param>
        /// <param name="dataSet">The DataSet to create a unique name for.</param>
        /// <returns>The generated name.</returns>
        private static string GenerateNameForType(Type type, DataSet dataSet)
        {
            var index = 0;
            var instanceName = type.Name + index.ToString("D4");
            while (dataSet.GetDataList().ContainsKey(instanceName))
            {
                index++;
                instanceName = type.Name + index.ToString("D4");
            }

            return instanceName;
        }

        public void OnPostprocessDataSets(IEnumerable<string> importedFiles, IEnumerable<string> deletedFiles)
        {
            // Remove deleted DataSets.
            foreach (var deletedFilePath in deletedFiles)
            {
                var deletedFileGuid = AssetDatabase.AssetPathToGUID(deletedFilePath);
                if (this.openDataSetGuids.Contains(deletedFileGuid))
                {
                    this.RemoveDataSet(deletedFileGuid);
                }
            }

            // Link reimported DataSets with their open (now-invalidated) counterparts.
            foreach (var importedFilePath in importedFiles)
            {
                var importedFileGuid = AssetDatabase.AssetPathToGUID(importedFilePath);
                if (!this.openDataSetGuids.Contains(importedFileGuid))
                {
                    continue;
                }

                this.RemoveDataSet(importedFileGuid);
                this.OpenDataSet(importedFileGuid);
            }

            this.treeView.Reload();
        }

        /// <summary>
        /// Called when the user double clicks on an asset.
        /// Checks if the asset is a DataSet, and if so, opens it in the Data List Window and gives it focus.
        /// </summary>
        /// <param name="instanceId">
        /// The instance ID of the selected asset.
        /// </param>
        /// <param name="line">
        /// The line number.
        /// </param>
        /// <returns>
        /// True if the asset's opening was handled by the Data List Window, else false.
        /// </returns>
        [OnOpenAsset]
        private static bool OnOpenedAsset(int instanceId, int line = -1)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceId) as DataSetAsset;

            if (asset == null)
            {
                return false;
            }
            
            var window = GetInstance();
            window.OpenDataSet(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset)));
            window.Focus();
            return true;
        }

        private static void SaveOpenDataSets(IEnumerable<string> openDataSets)
        {
            PlayerPrefsX.SetStringArray(PreferenceKeyOpenDataSets, openDataSets.ToArray());
        }

        private static IEnumerable<string> GetLastOpenDataSets()
        {
            var lastOpenDataSetsPaths = PlayerPrefsX.GetStringArray(PreferenceKeyOpenDataSets);
            return from path in lastOpenDataSetsPaths where !string.IsNullOrEmpty(path) select path;
        }
        
        /// <summary>
        /// Gets the current Data List Window or makes a new instance if it's not currently open.
        /// </summary>
        /// <returns>
        /// The <see cref="DataListWindow"/>.
        /// </returns>
        [MenuItem("FoxKit/Data List Window")]
        public static DataListWindow GetInstance()
        {
            var window = GetWindow<DataListWindow>();
            window.titleContent = new GUIContent("Data List");
            window.Show();
            return window;
        }

        /// <summary>
        /// When the window is loaded, initialize the TreeView.
        /// </summary>
        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(this.activeDataSetGuid))
            {
                var path = AssetDatabase.GUIDToAssetPath(this.activeDataSetGuid);

                // The asset was probably deleted, so stop holding onto its GUID.
                if (!string.IsNullOrEmpty(path))
                {
                    var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(path);
                    Assert.IsNotNull(dataSet, path);

                    this.activeDataSet = dataSet.GetDataSet();
                }
                else
                {
                    this.activeDataSet = null;
                }
            }

            IsOpen = true;

            if (this.treeViewState == null)
            {
                this.treeViewState = new TreeViewState();
            }

            if (this.openDataSetGuids == null)
            {
                this.openDataSetGuids = new List<string>();
            }

            Selection.selectionChanged += this.OnUnitySelectionChange;

            this.openDataSetGuids = GetLastOpenDataSets().ToList();
            this.treeView = new DataListTreeView(
                this.treeViewState,
                this.openDataSetGuids,
                this.activeDataSet,
                this.FindSceneProxyForEntity);
            this.treeView.Reload();
        }

        private void OnDisable()
        {
            IsOpen = false;
            SaveOpenDataSets(this.openDataSetGuids);
            Selection.selectionChanged -= this.OnUnitySelectionChange;
        }

        /// <summary>
        /// Opens a DataSet in the Data List Window.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet to open.
        /// </param>
        private void OpenDataSet(string dataSetGuid)
        {
            Assert.IsFalse(string.IsNullOrEmpty(dataSetGuid));

            var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid)).GetDataSet();
            Assert.IsNotNull(dataSet);

            this.activeDataSet = dataSet;
            this.activeDataSetGuid = dataSetGuid;
            this.treeView.SetActiveDataSet(dataSet);
            this.treeView.SelectDataSet(dataSet);
            
            if (this.openDataSetGuids.Contains(dataSetGuid))
            {
                return;
            }

            DataSet.CreateSceneProxyForEntityDelegate createSceneProxy = entityName => this.CreateSceneProxyForEntity(dataSetGuid, entityName);

            Entity.GetSceneProxyDelegate getSceneProxy =
                entityName => this.FindSceneProxyForEntity(dataSetGuid, entityName);

            dataSet.LoadAllEntities(createSceneProxy, getSceneProxy);

            this.openDataSetGuids.Add(dataSetGuid);
            this.treeView.Reload();
        }

        public SceneProxy CreateSceneProxyForEntity(string dataSetGuid, string entityName)
        {
            var gameObject = new GameObject(entityName);
            var sceneProxy = gameObject.AddComponent<SceneProxy>();
            var asset = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid));
            var entity = asset.GetDataSet().GetData(entityName);
            var transformData = entity as TransformData;

            Assert.IsNotNull(transformData);

            sceneProxy.Entity = transformData;
            sceneProxy.Asset = asset;

            var sceneProxyRecord = new SceneProxyRecord(dataSetGuid, entityName, sceneProxy);
            this.sceneProxyRecords.Add(sceneProxyRecord);

            return sceneProxy;
        }

        public SceneProxy FindSceneProxyForEntity(string dataSetGuid, string entityName)
        {
            foreach (var record in this.sceneProxyRecords)
            {
                if (record.DoesBelongToEntity(dataSetGuid, entityName))
                {
                    return record.SceneProxy;
                }
            }

            Debug.LogError($"Unable to find scene proxy for entity {entityName} in DataSet {AssetDatabase.GUIDToAssetPath(dataSetGuid)}");
            return null;
        }
        
        private void DeleteSceneProxyRecordsForDataSet(string dataSetGuid, bool destroyGameObjects)
        {
            var recordsToDelete = (from record in this.sceneProxyRecords
                                  where record.DoesBelongToDataSet(dataSetGuid)
                                  select record).ToList();

            foreach (var record in recordsToDelete)
            {
                this.sceneProxyRecords.Remove(record);

                if (destroyGameObjects && record.SceneProxy != null)
                {
                    GameObject.DestroyImmediate(record.SceneProxy);
                }
            }

            this.sceneProxyRecords.RemoveAll(record => record.DoesBelongToDataSet(dataSetGuid));
        }

        public void DeleteSceneProxyRecordForEntity(string dataSetGuid, string entityName, bool destroyGameObject)
        {
            SceneProxyRecord recordToDelete = null;

            foreach (var record in this.sceneProxyRecords)
            {
                if (record.DoesBelongToEntity(dataSetGuid, entityName))
                {
                    recordToDelete = record;
                }
            }

            if (recordToDelete == null)
            {
                return;
            }

            if (destroyGameObject && recordToDelete.SceneProxy != null)
            {
                GameObject.DestroyImmediate(recordToDelete.SceneProxy.gameObject);
            }
            
            this.sceneProxyRecords.Remove(recordToDelete);
        }
        
        private void OnUnitySelectionChange()
        {
            if (EditorWindow.focusedWindow == this)
            {
                return;
            }

            // TODO: Only unlock if it was forced to lock by the Data List window.
            ActiveEditorTracker.sharedTracker.isLocked = false;
        }

        public void SetActiveDataSet(object userData)
        {
            var dataSet = userData as DataSet;
            Assert.IsNotNull(dataSet);
            
            this.activeDataSet = dataSet.GetDataSet();
            this.activeDataSetGuid = dataSet.DataSetGuid;
            this.treeView.SetActiveDataSet(dataSet.GetDataSet());
        }

        public bool IsDataSetOpen(string dataSetGuid)
        {
            return this.openDataSetGuids.Contains(dataSetGuid);
        }
        
        public void RemoveDataSet(string dataSetGuid)
        {
            Assert.IsFalse(string.IsNullOrEmpty(dataSetGuid));

            // TODO: Clean up
            var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid))?.GetDataSet();
            Assert.IsNotNull(dataSet);

            dataSet?.UnloadAllEntities(entityName => this.DeleteSceneProxyRecordForEntity(dataSetGuid, entityName, true));

            if (this.activeDataSetGuid == dataSetGuid)
            {
                if (this.openDataSetGuids.Count > 1)
                {
                    this.activeDataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(this.openDataSetGuids[0])).GetDataSet();
                    this.treeView.SetActiveDataSet(this.activeDataSet);
                }
                else
                {
                    this.activeDataSet = null;
                }
            }
            
            this.openDataSetGuids.Remove(dataSetGuid);
            this.treeView.Reload();
        }

        /// <summary>
        /// Update and draw the window's UI.
        /// </summary>
        private void OnGUI()
        {
            this.ProcessKeyboardShortcuts();

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            
            if (GUILayout.Button("Create", EditorStyles.toolbarDropDown))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("DataSet"), false, this.CreateDataSet);

                if (this.activeDataSet != null)
                {
                    menu.AddItem(new GUIContent("Entity"), false, () => AddEntityWindow.Create(typeof(Data), false, type => GetInstance().AddEntity(type)));
                }

                menu.ShowAsContext();
            }

            if (GUILayout.Button("DEBUG RESET", EditorStyles.toolbarDropDown))
            {
                this.activeDataSet = null;
                this.activeDataSetGuid = null;
                this.openDataSetGuids.Clear();
                this.sceneProxyRecords.Clear();
                this.treeView.Reload();
            }

            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            this.treeView.OnGUI(new Rect(0, 17, this.position.width, this.position.height - 17));
        }

        private void CreateDataSet()
        {
            // TODO 
            /*
            var dataSet = CreateInstance<DataSet>();

            var path = UnityFileUtils.GetUniqueAssetPathNameOrFallback("DataSet0000.asset");
            AssetDatabase.CreateAsset(dataSet, path);

            this.OpenDataSet(path);*/
        }

        private void ProcessKeyboardShortcuts()
        {
            var current = Event.current;
            if (current.type != EventType.ValidateCommand)
            {
                return;
            }

            if (current.commandName != "SoftDelete")
            {
                return;
            }

            this.treeView.HandleDelete();
            current.Use();
        }

        [Serializable]
        private class SceneProxyRecord
        {
            [SerializeField]
            private string owningDataSetGuid;

            [SerializeField]
            private string owningEntityName;

            [SerializeField]
            private SceneProxy sceneProxy;

            public SceneProxy SceneProxy => this.sceneProxy;

            public SceneProxyRecord(string owningDataSetGuid, string owningEntityName, SceneProxy sceneProxy)
            {
                this.owningDataSetGuid = owningDataSetGuid;
                this.owningEntityName = owningEntityName;
                this.sceneProxy = sceneProxy;
            }

            public bool DoesBelongToDataSet(string dataSetGuid)
            {
                return this.owningDataSetGuid == dataSetGuid;
            }

            public bool DoesBelongToEntity(string dataSetGuid, string entityName)
            {
                return this.DoesBelongToDataSet(dataSetGuid) && this.owningEntityName == entityName;
            }
        }
    }
}