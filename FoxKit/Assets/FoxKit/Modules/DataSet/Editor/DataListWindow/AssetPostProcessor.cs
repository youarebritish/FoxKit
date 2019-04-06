namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    //public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    //{
    //    private static void OnPostprocessAllAssets(
    //        string[] importedAssets,
    //        string[] deletedAssets,
    //        string[] movedAssets,
    //        string[] movedFromAssetPaths)
    //    {
    //        var importedDataSets = from importedAsset in importedAssets select importedAsset;
    //        var deletedDataSets = from deletedAsset in deletedAssets select deletedAsset;
            
    //        var wasDataListWindowOpen = DataListWindow.IsOpen;
    //        var window = DataListWindow.GetInstance();
    //        window.OnPostprocessDataSets(importedDataSets, deletedDataSets);

    //        if (!wasDataListWindowOpen)
    //        {
    //            window.Close();
    //        }
    //    }
    //}
}