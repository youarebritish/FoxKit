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
    using FoxGameKit;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class OrthogonalCamera : Camera
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Int32, 448, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, typeof(OrthogonalCamera_ViewType))]
        private System.Int32 type;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 452, 1, Core.ContainerType.StaticArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private System.Single scale;
        
    }
}
