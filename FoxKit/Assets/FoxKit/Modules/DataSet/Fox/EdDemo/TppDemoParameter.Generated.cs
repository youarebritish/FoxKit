//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.EdDemo
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
    using Demo;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class TppDemoParameter : DemoParameter
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 64, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean pauseEnable;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 65, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean fadeSerchLightEnable;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 66, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean fadeDirectionalLightEnable;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 67, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean fadeSandStormEnable;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 68, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single sandStormDensityMin;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 72, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single fadeTime;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 76, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean useSetTime;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt32, 80, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.UInt32 hour;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt32, 84, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.UInt32 minute;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt32, 88, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.UInt32 second;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 92, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, typeof(TppDemoParameter_WeatherType))]
        private TppDemoParameter_WeatherType weatherType;
    }
}
