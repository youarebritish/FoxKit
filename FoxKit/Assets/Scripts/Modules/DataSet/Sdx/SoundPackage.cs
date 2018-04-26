using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class SoundPackage : Data
    {
        public UnityEngine.Object SoundDataFile;
        public bool SyncLoad;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "soundDataFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out SoundDataFile);
            }
            else if (propertyData.Name == "syncLoad")
            {
                SyncLoad = DataSetUtils.GetStaticArrayPropertyValue<FoxBool>(propertyData).Value;
            }
        }
    }
}