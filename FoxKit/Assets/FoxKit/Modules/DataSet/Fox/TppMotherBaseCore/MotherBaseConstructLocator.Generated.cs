//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.TppMotherBaseCore
{
    using System;
    using System.Collections.Generic;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.Lua;
    using FoxLib;
    using KopiLua;
    using OdinSerializer;
    using UnityEngine;
    using DataSetFile2 = DataSetFile2;
    using TppGameKit = FoxKit.Modules.DataSet.Fox.TppGameKit;
    using FoxCore;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class MotherBaseConstructLocator : TransformData
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 304, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Int32 type;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt16, 308, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.UInt16 index;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt8, 310, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Byte clusterId;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 311, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean isLowModel;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityLink, 312, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private List<FoxKit.Modules.DataSet.FoxCore.EntityLink> staticModels = new List<FoxKit.Modules.DataSet.FoxCore.EntityLink>();
    }
}
