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
    public partial class AmbientOcclusionSettings : Data
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 120, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, typeof(AmbientOcclusionSettings_Method))]
        private AmbientOcclusionSettings_Method method;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 124, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, typeof(AmbientOcclusionSettings_LightAttachment))]
        private AmbientOcclusionSettings_LightAttachment attachment;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.Never, typeof(Grx.GrxLineSSAOParameters), null)]
        private Grx.GrxLineSSAOParameters lineSSAOParameters;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 136, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.Never, typeof(Grx.GrxAreaSSAOParameters), null)]
        private Grx.GrxAreaSSAOParameters areaSSAOParameters;
    }
}
