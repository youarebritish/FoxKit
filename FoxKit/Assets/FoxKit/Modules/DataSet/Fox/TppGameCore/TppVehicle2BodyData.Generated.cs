//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.Lua;
    using FoxLib;
    using static KopiLua.Lua;
    using OdinSerializer;
    using UnityEngine;
    using DataSetFile2 = DataSetFile2;
    using TppGameKit = FoxKit.Modules.DataSet.Fox.TppGameKit;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class TppVehicle2BodyData : Data
    {
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt8, 177, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Byte vehicleTypeIndex;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt8, 178, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Byte proxyVehicleTypeIndex;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt8, 179, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Byte bodyImplTypeIndex;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 152, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private UnityEngine.Object partsFile;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt8, 176, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Byte bodyInstanceCount;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 136, 1, Core.ContainerType.DynamicArray, PropertyExport.Never, PropertyExport.Never, typeof(TppGameCore.TppVehicle2WeaponParameter), null)]
        private List<TppGameCore.TppVehicle2WeaponParameter> weaponParams = new List<TppGameCore.TppVehicle2WeaponParameter>();
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 120, 1, Core.ContainerType.DynamicArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private List<UnityEngine.Object> fovaFiles = new List<UnityEngine.Object>();
        
        public override short ClassId => 128;
        
        public override ushort Version => 3;
        
        public override string Category => "";
    }
}
