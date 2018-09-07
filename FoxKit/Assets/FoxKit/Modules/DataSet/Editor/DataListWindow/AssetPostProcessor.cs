namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Linq;

    using UnityEditor;

    public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var importedDataSets = from importedAsset in importedAssets select importedAsset;
            var deletedDataSets = from deletedAsset in deletedAssets select deletedAsset;

            // TODO This should update it even if it's not open
            // But calling GetInstance() shows it which is annoying.
            if (DataListWindow.IsOpen)
            {
                EditorWindow.GetWindow<DataListWindow>().OnPostprocessDataSets(importedDataSets, deletedDataSets);
            }
        }
    }
}