//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox
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
    public partial class TppPrimRiverModelOverlay : Data
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 120, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String primRiverGroupName = string.Empty;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean visibility;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 132, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single depthBlendLength;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 136, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single raise;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityLink, 144, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private FoxKit.Modules.DataSet.FoxCore.EntityLink staticModel;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 184, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String maskTextureName = string.Empty;
    }
}
