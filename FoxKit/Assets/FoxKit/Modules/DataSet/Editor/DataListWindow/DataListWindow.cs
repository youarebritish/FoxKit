﻿namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class DataListWindow : EditorWindow
    {
        private const string PreferenceKeyOpenDataSets = "FoxKit.DataListWindow.OpenDataSets";

        /// <summary>
        /// DataSets currently open in the window.
        /// </summary>
        [SerializeField]
        private List<DataSet> openDataSets;

        /// <summary>
        /// Serializable state of the TreeView.
        /// </summary>
        [SerializeField]
        private TreeViewState treeViewState;

        [SerializeField]
        private DataSet activeDataSet;

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
            var instance = ScriptableObject.CreateInstance(entityType);
            instance.name = GenerateNameForType(entityType, this.activeDataSet);

            this.ActiveDataSet.AddData(instance.name, instance as Data);
            
            this.treeView.Reload();
            this.treeView.SelectItem(instance as Data);
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
            var asset = EditorUtility.InstanceIDToObject(instanceId) as DataSet;

            if (asset == null)
            {
                return false;
            }
            
            var window = GetInstance();
            window.OpenDataSet(asset);
            window.Focus();
            return true;
        }

        private static void SaveOpenDataSets(IEnumerable<DataSet> openDataSets)
        {
            var openDataSetsPaths = from dataSet in openDataSets
                                    select AssetDatabase.GetAssetPath(dataSet);
            PlayerPrefsX.SetStringArray(PreferenceKeyOpenDataSets, openDataSetsPaths.ToArray());
        }

        private static IEnumerable<DataSet> GetLastOpenDataSets()
        {
            var lastOpenDataSetsPaths = PlayerPrefsX.GetStringArray(PreferenceKeyOpenDataSets);
            return from path in lastOpenDataSetsPaths
                   where !string.IsNullOrEmpty(path)
                   select AssetDatabase.LoadAssetAtPath<DataSet>(path);
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
            if (this.treeViewState == null)
            {
                this.treeViewState = new TreeViewState();
            }

            if (this.openDataSets == null)
            {
                this.openDataSets = new List<DataSet>();
            }

            Selection.selectionChanged += this.OnUnitySelectionChange;

            this.openDataSets = GetLastOpenDataSets().ToList();
            this.treeView = new DataListTreeView(this.treeViewState, this.openDataSets, this.activeDataSet);
            this.treeView.Reload();
        }

        private void OnDisable()
        {
            SaveOpenDataSets(this.openDataSets);
            Selection.selectionChanged -= this.OnUnitySelectionChange;
        }

        /// <summary>
        /// Opens a DataSet in the Data List Window.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet to open.
        /// </param>
        private void OpenDataSet(DataSet dataSet)
        {
            this.activeDataSet = dataSet;
            this.treeView.SetActiveDataSet(dataSet);
            this.treeView.SelectDataSet(dataSet);

            if (this.openDataSets.Contains(dataSet))
            {
                return;
            }

            dataSet.LoadAllEntities();
            this.openDataSets.Add(dataSet);
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
            var dataSet = userData as DataSet;
            Assert.IsNotNull(dataSet);

            this.activeDataSet = dataSet;
            this.treeView.SetActiveDataSet(dataSet);
        }

        private void RemoveDataSet(object userData)
        {
            var dataSet = userData as DataSet;
            Assert.IsNotNull(dataSet);

            dataSet.UnloadAllEntities();

            if (this.activeDataSet == dataSet)
            {
                if (this.openDataSets.Count > 1)
                {
                    this.activeDataSet = this.openDataSets[0];
                    this.treeView.SetActiveDataSet(this.activeDataSet);
                }
                else
                {
                    this.activeDataSet = null;
                }
            }

            this.openDataSets.Remove(dataSet);
            this.treeView.RemoveDataSet(dataSet);
            this.treeView.Reload();
        }

        /// <summary>
        /// Update and draw the window's UI.
        /// </summary>
        private void OnGUI()
        {
            this.ProcessKeyboardShortcuts();

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));

            if (GUILayout.Button("+", "ToolbarButton", GUILayout.Width(45f)))
            {
                AddEntityWindow.Create();
            }

            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            this.treeView.OnGUI(new Rect(0, 17, this.position.width, this.position.height - 17));
        }

        private void ProcessKeyboardShortcuts()
        {
            var current = Event.current;
            if (current.type != EventType.ValidateCommand)
            {
                return;
            }
            
            if (current.commandName == "SoftDelete")
            {
                this.treeView.HandleDelete();
                current.Use();
            }
        }
    }
}