//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Graphx
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
    public partial class GraphxSpatialGraphDataNode : DataElement
    {
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 64, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Vector3 position;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityHandle, 80, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorAndGame, PropertyExport.Never, null, null)]
        private List<FoxKit.Modules.DataSet.Fox.FoxCore.Entity> inlinks = new List<FoxKit.Modules.DataSet.Fox.FoxCore.Entity>();
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityHandle, 96, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorAndGame, PropertyExport.Never, null, null)]
        private List<FoxKit.Modules.DataSet.Fox.FoxCore.Entity> outlinks = new List<FoxKit.Modules.DataSet.Fox.FoxCore.Entity>();
        
        public override short ClassId => 80;
        
        public override ushort Version => 0;
        
        public override string Category => "Graphx";
        
        [ExposeMethodToLua(MethodStaticity.Instance)]
        partial void GetPosition(lua_State lua);
    }
}
