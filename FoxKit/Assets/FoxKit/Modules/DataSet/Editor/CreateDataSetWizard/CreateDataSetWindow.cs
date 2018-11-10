namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.Archive;
    using FoxKit.Utils;

    using UnityEditor;

    using UnityEngine;

    public class CreateDataSetWindow : EditorWindow
    {
        private string name = string.Empty;

        private PackageDefinition package;

        private int typeIndex = 2;

        private bool isNameGeneratorExpanded;

        private List<string> forbiddenNames = new List<string>();

        void OnEnable()
        {
            this.forbiddenNames = (from asset in AssetDatabase.FindAssets($"t:{typeof(EntityFileAsset).Name}")
                                   select Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(asset))).ToList();
        }
        
        public static void Create()
        {
            var window = ScriptableObject.CreateInstance(typeof(CreateDataSetWindow)) as CreateDataSetWindow;
            window.minSize = new Vector2(325, 105);
            window.maxSize = new Vector2(450, 105);
            window.titleContent = new GUIContent("Create DataSet");
            window.ShowUtility();
        }

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 100;
            
            // Package
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            this.package = EditorGUILayout.ObjectField("Package", this.package, typeof(PackageDefinition), false) as PackageDefinition;
            EditorGUILayout.EndHorizontal();
            
            // Type
            var dataSetTypes = ReflectionUtils.GetAssignableConcreteClasses(typeof(EntityFileAsset));
            var setTypes = dataSetTypes as Type[] ?? dataSetTypes.ToArray();
            var options = from assetType in setTypes
                          select assetType.Name.Substring(0, assetType.Name.Length - 5);

            this.typeIndex = EditorGUILayout.Popup("Type", this.typeIndex, options.ToArray());
            
            // Name
            this.name = EditorGUILayout.TextField("Name", this.name);

            // Input validation
            if (string.IsNullOrEmpty(this.name))
            {
                GUI.enabled = false;
            }
            if (this.forbiddenNames.Contains(this.name))
            {
                GUI.enabled = false;
            }
            else if (this.package == null)
            {
                GUI.enabled = false;
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create", GUILayout.ExpandWidth(false), GUILayout.Width(100)))
            {
                CreateDataSet(this.name, this.package, setTypes.ToArray()[this.typeIndex]);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void CreateDataSet(string name, PackageDefinition package, Type type)
        {
            DataListWindow.DataListWindow.GetInstance().CreateDataSet(name, package, type);
        }

        private static bool IsDataSetNameValid(string name)
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}