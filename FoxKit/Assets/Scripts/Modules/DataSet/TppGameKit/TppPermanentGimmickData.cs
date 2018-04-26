using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxKit.Modules.DataSet.Importer;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.TppGameKit
{
    /// <summary>
    /// Note: Gimmicks are weird. They're Data, not TransformData, and get their transform from an lba.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickData : Data
    {
        public UnityEngine.Object PartsFile;
        public UnityEngine.Object LocatorFile;
        public TppPermanentGimmickParameter Parameters;
        public uint Flags1;
        public uint Flags2;

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "partsFile")
            {
                var ptr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(ptr, out PartsFile);
            }
            else if (propertyData.Name == "locaterFile")
            {
                var ptr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(ptr, out LocatorFile);
            }
            else if (propertyData.Name == "parameters")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                Parameters = getEntity(address) as TppPermanentGimmickParameter;

                if (Parameters != null)
                {
                    Parameters.Owner = this;
                }
            }
            else if (propertyData.Name == "flags1")
            {
                Flags1 = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
            }
            else if (propertyData.Name == "flags2")
            {
                Flags2 = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
            }
        }
    }
}