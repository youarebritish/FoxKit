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
    using KopiLua;
    using OdinSerializer;
    using UnityEngine;
    using DataSetFile2 = DataSetFile2;
    using TppGameKit = FoxKit.Modules.DataSet.Fox.TppGameKit;
    using FoxCore;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class UInt64StringMapPropertyDifference : PropertyDifference
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt64, 72, 1, Core.ContainerType.StringMap, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private Dictionary<string, System.UInt64> originalValues = new Dictionary<string, System.UInt64>();
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt64, 120, 1, Core.ContainerType.StringMap, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private Dictionary<string, System.UInt64> values = new Dictionary<string, System.UInt64>();
    }
}
