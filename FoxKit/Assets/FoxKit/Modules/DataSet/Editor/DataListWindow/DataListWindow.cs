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

        [SerializeField]
        private DataSet activeDataSet;

        [SerializeField]
        private string activeDataSetGuid;

        /// <summary>
        /// Tree view widget.
        /// </summary>
        private DataListTreeView treeView;
        
        public DataSet ActiveDataSet => this.activeDataSet;

        public DataListWindowItemContextMenuFactory.ShowItemContextMenuDelegate MakeShowItemContextMenuDelegate()
        {
            return DataListWindowItemContextMenuFactory.Create(this.SetActiveDataSet, this.RemoveDataSet);
        }

        /// <summary>
        /// Create a new Entity of a given type in the active DataSet.
        /// </summary>
        /// <param name="entityType">Type of the Entity to add.</param>
        public void AddEntity(Type entityType)
        {
            // TODO
            /*
            var instance = ScriptableObject.CreateInstance(entityType);
            instance.name = GenerateNameForType(entityType, this.activeDataSet);
            // TODO (instance as Entity).AssignDataSet(this.activeDataSet);

            this.ActiveDataSet.AddData(instance.name, instance as Data);
            
            this.treeView.Reload();
            this.treeView.SelectItem(instance as Data);*/
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
                this.activeDataSet);
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

            dataSet.LoadAllEntities();
            this.openDataSetGuids.Add(dataSetGuid);
            this.treeView.Reload();
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
            // TODO: This is getting DataSet objects, not DataSetAssets.
            var dataSet = userData as DataSetAsset;
            Assert.IsNotNull(dataSet);

            var path = AssetDatabase.GetAssetPath(dataSet);

            this.activeDataSet = dataSet.GetDataSet();
            this.activeDataSetGuid = path;
            this.treeView.SetActiveDataSet(dataSet.GetDataSet());
        }

        private void RemoveDataSet(object userData)
        {
            var dataSetGuid = userData as string;
            if (dataSetGuid == null)
            {
                return;
            }

            // TODO: Clean up
            // This shouldn't be called if the DataSet is being deleted; it will have been handled in FoxKitAssetModificationProcessor
            // Issue 2: Imported fox2s are read-only, which means the scene proxies can't be stored in them
            var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid))?.GetDataSet();
            Assert.IsNotNull(dataSet);

            dataSet?.UnloadAllEntities();

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
                    menu.AddItem(new GUIContent("Entity"), false, () => AddEntityWindow.Create(typeof(Data), false, GetInstance().AddEntity));
                }

                menu.ShowAsContext();
            }

            if (GUILayout.Button("DEBUG RESET", EditorStyles.toolbarDropDown))
            {
                this.activeDataSet = null;
                this.activeDataSetGuid = null;
                this.openDataSetGuids.Clear();
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
    }
}