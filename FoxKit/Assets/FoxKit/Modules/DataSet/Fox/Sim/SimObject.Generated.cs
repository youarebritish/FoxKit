//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Sim
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
    public partial class SimObject : Data
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 120, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, typeof(Sim.SimObjectParam), null)]
        private Sim.SimObjectParam objectParam;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 128, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, typeof(Sim.SimEngineParam), null)]
        private Sim.SimEngineParam engineParam;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 136, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, typeof(Sim.SimControlElement), null)]
        private List<Sim.SimControlElement> controls = new List<Sim.SimControlElement>();
    }
}
