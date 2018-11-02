using FoxKit.Modules.DataSet.FoxCore;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Core
{
    using System;
    using System.Linq;

    using FoxKit.Modules.DataSet.Fox.FoxCore;

    using Object = UnityEngine.Object;

    public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        public delegate bool TryGetAssetDelegate(string filename, out Object asset);
        public delegate DataSet GetDataSetDelegate(string filename);
        public delegate DataIdentifier GetDataIdentifierDelegate(string identifier);

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // TODO: Handle existing assets
            var assets = new Dictionary<string, UnityEngine.Object>();

            var dataSets =
                (from guid in AssetDatabase.FindAssets($"t:{typeof(DataSetAsset).Name}")
                 select AssetDatabase.GUIDToAssetPath(guid))
                .ToDictionary(Path.GetFileNameWithoutExtension, path => AssetDatabase.LoadAssetAtPath<DataSetAsset>(path).GetDataSet());

            List<DataIdentifier> dataIdentifiers = null;/*(from dataSet in dataSets
                                   from entity in dataSet.Value.GetDataList().Values
                                   where entity is DataIdentifier
                                   select entity as DataIdentifier).ToList();*/

            var tryGetAsset = MakeTryGetAssetDelegate(assets);
            var getDataSet = MakeGetDataSetDelegate(dataSets);
            var getDataIdentifier = MakeGetDataIdentifierDelegate(dataIdentifiers);

            foreach (var asset in importedAssets)
            {
                var loadedAsset = AssetDatabase.LoadAssetAtPath<Object>(asset);
                assets.Add(asset, loadedAsset);

                if (!(loadedAsset is DataSetAsset))
                {
                    continue;
                }

                // Assign GUID reference.
                var dataSet = (loadedAsset as DataSetAsset).GetDataSet();
                var guid = AssetDatabase.AssetPathToGUID(asset);
                //if (dataSet.DataSetGuid == guid)
                {
                    continue;
                }

                //dataSet.DataSetGuid = guid;
                foreach (var entity in dataSet.GetDataList())
                {
                    ((Data)entity.Value).DataSetGuid = guid;
                }
            }

            foreach (var asset in assets.Values)
            {
                var dataSetAsset = asset as DataSetAsset;
                if (dataSetAsset == null)
                {
                    continue;
                }

                foreach (var entity in dataSetAsset.GetDataSet().GetDataList().Values)
                {
                    entity.OnAssetsImported(getDataSet, tryGetAsset, getDataIdentifier);
                }
            }
        }

        private static TryGetAssetDelegate MakeTryGetAssetDelegate(Dictionary<string, Object> assets)
        {
            return (string path, out Object asset) => TryGetAsset(assets, path, out asset);
        }

        private static bool TryGetAsset(IDictionary<string, Object> newlyImportedAssets, string path, out Object asset)
        {
            if (string.IsNullOrEmpty(path))
            {
                asset = null;
                return true;
            }

            // First see if the asset was just imported.
            if (newlyImportedAssets.TryGetValue(path, out asset))
            {
                return true;
            }

            // Next see if the asset already exists in the project.
            asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (asset != null)
            {
                return true;
            }

            Debug.LogWarning($"Referenced asset {path} not found.");
            return false;
        }

        private static GetDataSetDelegate MakeGetDataSetDelegate(IDictionary<string, DataSet> dataSets)
        {
            return (string name) => GetDataSet(dataSets, name);
        }

        private static GetDataIdentifierDelegate MakeGetDataIdentifierDelegate(IEnumerable<DataIdentifier> dataIdentifiers)
        {
            return identifier => GetDataIdentifier(dataIdentifiers, identifier);
        }

        private static DataSet GetDataSet(IDictionary<string, DataSet> dataSets, string name)
        {
            DataSet result = null;

            // FIXME: Why does this happen?
            if (name == null)
            {
                return null;
            }
            
            if (dataSets.TryGetValue(name, out result))
            {
                return result;
            }
            
            Debug.LogWarning($"Referenced DataSet {name} not found.");
            return null;
        }

        private static DataIdentifier GetDataIdentifier(IEnumerable<DataIdentifier> dataIdentifiers, string identifier)
        {
            var result = dataIdentifiers.FirstOrDefault(dataIdentifier => dataIdentifier.Identifier == identifier);
            if (result == null)
            {
                Debug.LogWarning($"DataIdentifier {identifier} was not found.");
            }

            return result;
        }
    }
}