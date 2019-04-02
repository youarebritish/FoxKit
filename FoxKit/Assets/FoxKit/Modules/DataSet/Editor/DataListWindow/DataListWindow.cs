namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FmdlStudio.Scripts.MonoBehaviours;

    using FoxKit.Modules.Archive;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.Fox.FoxGameKit;
    using FoxKit.Utils;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class DataListWindow : EditorWindow
    {
        private const string PreferenceKeyOpenDataSets = "FoxKit.DataListWindow.OpenDataSets";
        
        public static bool IsOpen { get; private set; }
        public DataSet ActiveDataSet => this.activeDataSet;

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
        
        /// <summary>
        /// Tree view widget.
        /// </summary>
        private DataListTreeView treeView;
                
        public DataListWindowItemContextMenuFactory.ShowItemContextMenuDelegate MakeShowItemContextMenuDelegate()
        {
            return DataListWindowItemContextMenuFactory.Create(
                this.SetActiveDataSet,
                delegate(object dataSets)
                    {
                        var guids = from dataSet in (dataSets as IEnumerable<DataSet>)
                                    select dataSet.DataSetGuid;
                        this.RemoveDataSets(guids);
                    });
        }

        public void RenameEntity(Data entity, string oldName, string newName)
        {
            entity.GetDataSet().RemoveData(oldName);
            entity.GetDataSet().AddData(newName, entity);

            this.treeView.Reload();
        }

        /// <summary>
        /// Create a new Entity of a given type in the active DataSet.
        /// </summary>
        /// <param name="entityType">Type of the Entity to add.</param>
        public Data AddEntity(Type entityType, GenerateEntityNameDelegate generateName = null)
        {
            var instance = Activator.CreateInstance(entityType) as Data;

            if (generateName == null)
            {
                instance.Name = GenerateNameForType(entityType, this.activeDataSet);
            }
            else
            {
                uint index = 0;
                var instanceName = generateName(index);
                while (this.activeDataSet.GetDataList().ContainsKey(instanceName))
                {
                    index++;
                    instanceName = generateName(index);
                }

                instance.Name = generateName(index);
            }

            var state = SingletonScriptableObject<DataListWindowState>.Instance;
            instance.DataSetGuid = state.ActiveDataSetGuid;

            this.activeDataSet.AddData(instance.Name, instance);

            // FIXME terrible hack
            DataSet.CreateSceneProxyForEntityDelegate createSceneProxy = entityName => state.CreateSceneProxyForEntity(this.activeDataSet.DataSetGuid, entityName);

            Entity.GetSceneProxyDelegate getSceneProxy = entityName => state.FindSceneProxyForEntity(
                this.activeDataSet.DataSetGuid,
                entityName);

            instance.OnLoaded(() => createSceneProxy(instance.Name));
            instance.PostOnLoaded(getSceneProxy);

            // TODO
            // There must be a better way of doing this
            this.treeView = new DataListTreeView(
                this.treeViewState,
                this.openDataSetGuids,
                this.activeDataSet,
                state.FindSceneProxyForEntity);
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
            var asset = EditorUtility.InstanceIDToObject(instanceId) as EntityFileAsset;

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
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            var hierarchyWindow = EditorWindow.focusedWindow;
            Type[] desiredDockNextTo = new Type[] { hierarchyWindow.GetType() };
            var window = GetWindow<DataListWindow>("Data List", desiredDockNextTo);

            var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Gizmos/DataSet/Icons/Editor/DataListWindowIcon.png");
            var titleContent = new GUIContent(" Data List", icon);
            window.titleContent = titleContent;

            window.Show();
            return window;
        }

        public void UpdateSelection()
        {
            this.treeView.UpdateSelection();
        }

        /// <summary>
        /// When the window is loaded, initialize the TreeView.
        /// </summary>
        private void OnEnable()
        {
            var state = SingletonScriptableObject<DataListWindowState>.Instance;
            if (!string.IsNullOrEmpty(state.ActiveDataSetGuid))
            {
                var path = AssetDatabase.GUIDToAssetPath(state.ActiveDataSetGuid);

                // The asset was probably deleted, so stop holding onto its GUID.
                if (!string.IsNullOrEmpty(path))
                {
                    var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(path);
                    if (dataSet == null)
                    {
                        state.ActiveDataSetGuid = null;
                    }
                    else
                    {
                        this.activeDataSet = dataSet.GetDataSet();
                    }

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
                SingletonScriptableObject<DataListWindowState>.Instance.FindSceneProxyForEntity);
            this.treeView.Reload();
        }

        private void OnDisable()
        {
            IsOpen = false;
            SaveOpenDataSets(this.openDataSetGuids);
            Selection.selectionChanged -= this.OnUnitySelectionChange;
        }

        /// <summary>
        /// Opens a DataSet in the Data List window and selects an Entity within it.
        /// </summary>
        /// <param name="dataSetGuid"></param>
        /// <param name="entityName"></param>
        public void OpenDataSet(string dataSetGuid, string entityName)
        {
            var dataSet = this.OpenDataSet(dataSetGuid);
            Assert.IsTrue(dataSet.GetDataList().ContainsKey(entityName));

            this.treeView.SelectItem(dataSet.GetData(entityName));
        }

        /// <summary>
        /// Opens a DataSet in the Data List Window.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet to open.
        /// </param>
        public DataSet OpenDataSet(string dataSetGuid)
        {
            Assert.IsFalse(string.IsNullOrEmpty(dataSetGuid));

            // Ignore any models already in the scene when the first DataSet is opened.
            if (this.openDataSetGuids.Count == 0)
            {
                var allModelsInScene = GameObject.FindObjectsOfType<FoxModel>();
                foreach (var model in allModelsInScene)
                {
                    SingletonScriptableObject<DataListWindowState>.Instance.IgnoreModel(model);
                }
            }

            var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid)).GetDataSet();
            Assert.IsNotNull(dataSet);

            this.activeDataSet = dataSet;
            SingletonScriptableObject<DataListWindowState>.Instance.ActiveDataSetGuid = dataSetGuid;
            this.treeView.SetActiveDataSet(dataSet);
            this.treeView.SelectDataSet(dataSet);
            
            if (this.openDataSetGuids.Contains(dataSetGuid))
            {
                return dataSet;
            }

            var state = SingletonScriptableObject<DataListWindowState>.Instance;
            DataSet.CreateSceneProxyForEntityDelegate createSceneProxy = entityName => state.CreateSceneProxyForEntity(dataSetGuid, entityName);

            Entity.GetSceneProxyDelegate getSceneProxy = entityName => state.FindSceneProxyForEntity(
                dataSetGuid,
                entityName);

            dataSet.LoadAllEntities(createSceneProxy, getSceneProxy);

            this.openDataSetGuids.Add(dataSetGuid);
            this.treeView.Reload();
            return dataSet;
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
            SingletonScriptableObject<DataListWindowState>.Instance.ActiveDataSetGuid = dataSet.DataSetGuid;
            this.treeView.SetActiveDataSet(dataSet.GetDataSet());
        }

        public bool IsDataSetOpen(string dataSetGuid)
        {
            return this.openDataSetGuids.Contains(dataSetGuid);
        }

        public void RemoveDataSets(IEnumerable<string> dataSetGuids)
        {
            foreach (var guid in dataSetGuids)
            {
                this.RemoveDataSet(guid);
            }
        }
        
        public void RemoveDataSet(string dataSetGuid)
        {
            Assert.IsFalse(string.IsNullOrEmpty(dataSetGuid));

            if (this.openDataSetGuids.Count == 1)
            {
                SingletonScriptableObject<DataListWindowState>.Instance.ClearIgnoredModels();
                this.activeDataSet = null;
            }

            // TODO: Clean up
            var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid))?.GetDataSet();
            Assert.IsNotNull(dataSet);

            dataSet?.UnloadAllEntities(entityName => SingletonScriptableObject<DataListWindowState>.Instance.DeleteSceneProxy(dataSetGuid, entityName, DataListWindowState.DestroyGameObject.Destroy));

            if (SingletonScriptableObject<DataListWindowState>.Instance.ActiveDataSetGuid == dataSetGuid)
            {
                if (this.openDataSetGuids.Count > 1)
                {
                    this.activeDataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(this.openDataSetGuids[0])).GetDataSet();
                    this.treeView.SetActiveDataSet(this.activeDataSet);
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

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                this.activeDataSet = null;
                this.openDataSetGuids.Clear();
                SingletonScriptableObject<DataListWindowState>.Instance.ClearState();
                this.treeView.Reload();
            }

            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            this.treeView.OnGUI(new Rect(0, 17, this.position.width, this.position.height - 17));
        }

        public void CreateDataSet(string name, PackageDefinition package, Type type)
        {
            Assert.IsFalse(string.IsNullOrEmpty(name));
            Assert.IsNotNull(package);
            Assert.IsNotNull(type);

            var dataSet = CreateInstance(type) as EntityFileAsset;
            Assert.IsNotNull(dataSet);

            dataSet.Initialize();

            var path = UnityFileUtils.GetUniqueAssetPathNameOrFallback($"{name}.asset");
            AssetDatabase.CreateAsset(dataSet, path);

            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(dataSet));
            dataSet.GetDataSet().DataSetGuid = guid;

            package.Entries.Add(dataSet);
            dataSet.PackageGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(package));

            this.OpenDataSet(guid);
        }

        private void CreateDataSet()
        {
            CreateDataSetWindow.Create();
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

        public delegate string GenerateEntityNameDelegate(uint id);

        private void OnHierarchyChange()
        {
            var allModelsInScene = GameObject.FindObjectsOfType<FoxModel>();

            foreach (var model in allModelsInScene)
            {
                if (this.openDataSetGuids.Count == 0)
                {
                    if (SingletonScriptableObject<DataListWindowState>.Instance.IsModelIgnored(model))
                    {
                        continue;
                    }

                    SingletonScriptableObject<DataListWindowState>.Instance.IgnoreModel(model);
                }

                if (SingletonScriptableObject<DataListWindowState>.Instance.IsModelIgnored(model))
                {
                    continue;
                }

                var parent = model.transform.parent;

                if (parent != null)
                {
                    var sceneProxy = parent.GetComponent<SceneProxy>();
                    if (sceneProxy != null)
                    {
                        continue;
                    }
                }

                var mouseEvent = Event.current;
                if (mouseEvent != null && mouseEvent.type == EventType.MouseDown && mouseEvent.button == 0)
                {
                    return;
                }

                var prefab = PrefabUtility.GetCorrespondingObjectFromSource(model.gameObject);
                if (prefab == null)
                {
                    return;
                }

                GenerateEntityNameDelegate generateName = id => $"{prefab.name}_{id:D4}";

                // New model was added to the scene. Add it to the active DataSet.
                var staticModel = this.AddEntity(typeof(StaticModel), generateName) as StaticModel;
                var transformEntity = new TransformEntity
                                          {
                                              Owner = staticModel,
                                              Translation = model.transform.position,
                                              RotQuat = model.transform.rotation,
                                              Scale = model.transform.localScale
                                          };
                staticModel.Transform = transformEntity;
                staticModel.ModelFile = prefab;

                var newSceneProxy =
                    SingletonScriptableObject<DataListWindowState>.Instance.FindSceneProxyForEntity(
                        staticModel.DataSetGuid,
                        staticModel.Name);
                model.transform.SetParent(newSceneProxy.transform, true);

                var modelProxy = model.gameObject.AddComponent<SceneProxyChild>();
                modelProxy.Owner = newSceneProxy;
                modelProxy.SetModel(staticModel.ModelFile);

                this.treeView.SelectItem(staticModel);
            }

            this.Repaint();
        }

        private void OnSelectionChange()
        {
            // When selecting a SceneProxy, if it belongs to an Entity, select it in the Data List window.
            foreach (var selection in Selection.gameObjects)
            {
                var sceneProxy = selection.GetComponent<SceneProxy>();
                if (sceneProxy == null)
                {
                    continue;
                }
                
                var entity = sceneProxy.Entity;
                this.treeView.SelectItem(entity);
            }
        }
    }
}