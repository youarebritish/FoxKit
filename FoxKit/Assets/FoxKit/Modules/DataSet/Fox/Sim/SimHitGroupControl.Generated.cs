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
    using Sim;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class SimHitGroupControl : SimControlElement
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityPtr, 72, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, typeof(Sim.SimHitGroupControlParam), null)]
        private Sim.SimHitGroupControlParam controlParam;
    }
}
