//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
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
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class CheckpointTrapScriptModuleCondition : GeoTrapScriptModuleCondition
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Path, 368, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private List<UnityEngine.Object> checkpointScriptArray = new List<UnityEngine.Object>();
        
        public override short ClassId => 0;
        
        public override ushort Version => 0;
        
        public override string Category => "GameKit";
    }
}
