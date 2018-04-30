using FoxKit.Utils;
using FoxKit.Utils.UI.StringMap;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class DataSet : Data
    {
        [SerializeField]
        public EntityStringMap DataList = new EntityStringMap();

        [SerializeField]
        public AddressEntityDictionary AddressMap = new AddressEntityDictionary();

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);

            foreach (var data in DataList.Values)
            {
                data.OnAssetsImported(tryGetImportedAsset);
            }
        }
    }
}