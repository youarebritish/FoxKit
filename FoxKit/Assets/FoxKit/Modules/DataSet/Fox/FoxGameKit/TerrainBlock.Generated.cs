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
    using FoxCore;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class TerrainBlock : Data
    {
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.UInt32, 144, 1, Core.ContainerType.StaticArray, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private System.UInt32 id;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 176, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String terrainName = string.Empty;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.FilePtr, 152, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Object filePtr;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Path, 184, 1, Core.ContainerType.StaticArray, PropertyExport.EditorOnly, PropertyExport.EditorOnly, null, null)]
        private UnityEngine.Object filePath;
        
        [OdinSerializeAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Vector3, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private UnityEngine.Vector3 pos;
        
        public override short ClassId => 144;
        
        public override ushort Version => 0;
        
        public override string Category => "";
    }
}
