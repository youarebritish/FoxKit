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
    public partial class Int16ArrayPropertyDifference : PropertyDifference
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int16, 72, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private List<System.Int16> originalValues = new List<System.Int16>();
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int16, 88, 1, Core.ContainerType.DynamicArray, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private List<System.Int16> values = new List<System.Int16>();
    }
}
