using FoxKit.Modules.DataSet.FoxCore;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace FoxKit.Core
{
    public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        public delegate bool TryGetAsset(string filename, out UnityEngine.Object asset);

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // TODO: Handle existing assets
            var assets = new Dictionary<string, UnityEngine.Object>();
            foreach(var asset in importedAssets)
            {
                var loadedAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(asset);
                assets.Add(Path.GetFileName(asset), loadedAsset);
            }

            foreach (var asset in assets.Values)
            {
                var entity = asset as Entity;
                if (entity == null)
                {
                    continue;
                }
                entity.OnAssetsImported(assets.TryGetValue);
            }
        }
    }
}