//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox.TppGameCore
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
    public partial class TppBossQuiet2Parameter : DataElement
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 56, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Object partsFile;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 80, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Object motionGraphFile;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 104, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Object mtarFile;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Object extensionMtarFile;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 152, 1, Core.ContainerType.StringMap, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private Dictionary<string, UnityEngine.Object> vfxFiles = new Dictionary<string, UnityEngine.Object>();
        
        public override short ClassId => 176;
        
        public override ushort Version => 0;
        
        public override string Category => "";
    }
}
