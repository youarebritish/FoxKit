//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Ph
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
    using Ph;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class PhShoulderConstraintParam : PhConstraintParam
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 64, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Boolean limitedFlag;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 80, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private UnityEngine.Vector3 refA;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 96, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private UnityEngine.Vector3 refB;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 112, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Single limit;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 116, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Boolean limitedFlag1;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 128, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private UnityEngine.Vector3 refA1;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 144, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private UnityEngine.Vector3 refB1;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 160, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Single limit1;
    }
}
