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
    public partial class Data : Entity
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 72, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String name = string.Empty;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityHandle, 80, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private FoxKit.Modules.DataSet.Fox.FoxCore.Entity dataSet;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 0, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.Never, null, null)]
        private System.String referencePath = string.Empty;
        
        public override short ClassId => -1;
        
        public override ushort Version => 2;
        
        public override string Category => "";
    }
}
