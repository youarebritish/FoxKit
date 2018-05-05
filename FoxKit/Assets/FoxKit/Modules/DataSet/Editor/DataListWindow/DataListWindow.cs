namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

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
        private DataListTreeView dataListTreeView;

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
            
            var window = MakeOrGetWindow();
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
        private static DataListWindow MakeOrGetWindow()
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

            this.openDataSets = GetLastOpenDataSets().ToList();
            this.dataListTreeView = new DataListTreeView(this.treeViewState, this.openDataSets);
            this.dataListTreeView.Reload();
        }

        private void OnDisable()
        {
            SaveOpenDataSets(this.openDataSets);
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
            this.dataListTreeView.SetActiveDataSet(dataSet);

            if (this.openDataSets.Contains(dataSet))
            {
                return;
            }

            dataSet.LoadAllEntities();
            this.openDataSets.Add(dataSet);
            this.dataListTreeView.Reload();
        }

        /// <summary>
        /// Update and draw the window's UI.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Load", "ToolbarButton", GUILayout.Width(45f)))
            {
                EditorGUIUtility.ShowObjectPicker<DataSet>(null, false, string.Empty, 0);
            }

            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            this.dataListTreeView.OnGUI(new Rect(0, 17, this.position.width, this.position.height - 17));
        }
    }
}