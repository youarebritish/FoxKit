//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Grx
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
    public partial class Horizon : TransformData
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 304, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean enable;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 308, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, typeof(Horizon_ColorPreset))]
        private Horizon_ColorPreset colorPreset;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Color, 320, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Color skyColor;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Color, 336, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Color horizonColor;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 352, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean showObject;
    }
}