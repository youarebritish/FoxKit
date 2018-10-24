using FoxKit.Modules.DataSet.Editor.Toolbar;
using FoxKit.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Editor.Command
{
    public class CreateDataSetCommand : IToolbarCommand
    {
        public Texture Icon => icon;        
        public string Tooltip => "Create a new DataSet.";
        public Type ToolbarType => typeof(FoxCoreToolbar);

        private static readonly Texture icon = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
        
        public void Execute()
        {
            var dataSet = ScriptableObject.CreateInstance<DataSetAsset>();
            var path = UnityFileUtils.GetUniqueAssetPathNameOrFallback("DataSet0000.asset");
            AssetDatabase.CreateAsset(dataSet, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = dataSet;
        }
    }
}