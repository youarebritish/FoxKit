//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.FoxCore
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
    using FoxCore;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class DataIdentifier : Data
    {
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 120, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String identifier = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityLink, 128, 1, Core.ContainerType.StringMap, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private Dictionary<string, FoxKit.Modules.DataSet.FoxCore.EntityLink> links = new Dictionary<string, FoxKit.Modules.DataSet.FoxCore.EntityLink>();
        
        public override short ClassId => 120;
        
        public override ushort Version => 0;
        
        public override string Category => "Data";
        
        [ExposeMethodToLua(MethodStaticity.Static)]
        static partial void GetDataWithIdentifier(lua_State lua);
        
        [ExposeMethodToLua(MethodStaticity.Static)]
        static partial void GetDataBodyWithIdentifier(lua_State lua);
    }
}
