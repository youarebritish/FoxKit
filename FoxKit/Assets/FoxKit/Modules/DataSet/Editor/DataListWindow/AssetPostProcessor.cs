namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

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
            DataListWindow.GetInstance().OnPostprocessDataSets(importedDataSets, deletedDataSets);
        }
    }
}