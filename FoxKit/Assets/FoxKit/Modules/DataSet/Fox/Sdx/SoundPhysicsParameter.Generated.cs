//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Sdx
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
    public partial class SoundPhysicsParameter : Data
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 120, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String hitEvent;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String rollStartEvent;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 136, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String rollEndEvent;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 144, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String hitRtpcName;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 152, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String rollRtpcName;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 160, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String switchName;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 168, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String generalEvent1;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 176, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String generalEvent2;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 184, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single hitLowerPower;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 188, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single hitUpperPower;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 192, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single hitIntervalSeconds;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 196, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single hitLowerRtpc;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 200, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single hitUpperRtpc;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 204, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollLowerPower;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 208, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollUpperPower;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 212, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollStartSeconds;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 216, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollEndSeconds;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 220, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollLowerRtpc;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 224, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single rollUpperRtpc;
    }
}
