//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.Nav
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
    using Nav;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class NavVehicleNavigationParameter : NavNavigationParameter
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 48, 1, Core.ContainerType.DynamicArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private List<System.Single> turningRadii = new List<System.Single>();
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 64, 1, Core.ContainerType.DynamicArray, PropertyExport.Never, PropertyExport.Never, null, null)]
        private List<System.Single> turningSpeeds = new List<System.Single>();
        
        public override short ClassId => 0;
        
        public override ushort Version => 0;
        
        public override string Category => "Nav";
    }
}
