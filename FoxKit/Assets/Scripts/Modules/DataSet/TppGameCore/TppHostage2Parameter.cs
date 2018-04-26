using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using FoxKit.Utils;
using FoxKit.Modules.DataSet.GameCore;
using FoxKit.Utils.UI.StringMap;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    public class TppHostage2Parameter : GameObjectParameter
    {
        public UnityEngine.Object PartsFile;
        public UnityEngine.Object MotionGraphFile;
        public UnityEngine.Object MtarFile;
        public UnityEngine.Object ExtensionMtarFile;
        public ObjectStringMap VfxFiles;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);
            
            if (propertyData.Name == "partsFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out PartsFile);
            }
            else if (propertyData.Name == "motionGraphFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out MotionGraphFile);
            }
            else if (propertyData.Name == "mtarFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out MtarFile);
            }
            else if (propertyData.Name == "extensionMtarFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out ExtensionMtarFile);
            }
            else if (propertyData.Name == "vfxFiles")
            {
                var dictionary = DataSetUtils.GetStringMap<FoxFilePtr>(propertyData);

                VfxFiles = new ObjectStringMap();
                foreach(var entry in dictionary)
                {
                    UnityEngine.Object file = null;
                    var fileFound = DataSetUtils.TryGetFile(entry.Value, out file);
                    VfxFiles.Add(entry.Key.ToString(), file);
                }
            }
        }
    }
}