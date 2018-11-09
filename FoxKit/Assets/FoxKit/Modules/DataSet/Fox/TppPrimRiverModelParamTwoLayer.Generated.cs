//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
namespace FoxKit.Modules.DataSet.Fox
{
    using System;
    using System.Collections.Generic;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.Lua;
    using FoxLib;
    using static KopiLua.Lua;
    using OdinSerializer;
    using UnityEngine;
    using DataSetFile2 = DataSetFile2;
    using TppGameKit = FoxKit.Modules.DataSet.Fox.TppGameKit;
    using FoxCore;
    
    [SerializableAttribute, ExposeClassToLuaAttribute]
    public partial class TppPrimRiverModelParamTwoLayer : Data
    {
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 120, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String primRiverGroupName = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 128, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean visibility;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 136, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String baseTextureName = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 144, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String normalTextureName = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 152, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String cubeMapName = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.String, 160, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.String secondLayerBaseTextureName = string.Empty;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.EntityLink, 168, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private FoxKit.Modules.DataSet.FoxCore.EntityLink lightCaptureLocator;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 208, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollDirection0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 212, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollSpeed0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 216, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollScale0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 220, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollDirection1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 224, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollSpeed1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 228, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single scrollScale1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 232, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single transparency;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 236, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single transparencyDepthBlend;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 240, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single reflectionRate;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 244, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single distortionPower;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 248, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single surfaceShadowRate;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 252, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollDirection0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 256, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollSpeed0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 260, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollScale0;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 264, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollDirection1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 268, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollSpeed1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 272, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondScrollScale1;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 276, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondLayerTransparency;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 280, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondLayerTransparencyDepthBlend;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 284, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondLayerReflectionRate;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 288, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondLayerDistortionPower;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 292, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single secondLayerSurfaceShadowRate;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 296, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single maskDistortionRate;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Float, 300, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Single oilIntensity;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 304, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean useHnmTexture;
        
        [OdinSerializeAttribute, NonSerializedAttribute, PropertyInfoAttribute(Core.PropertyInfoType.Bool, 305, 1, Core.ContainerType.StaticArray, PropertyExport.EditorAndGame, PropertyExport.EditorAndGame, null, null)]
        private System.Boolean debugReset;
        
        public override short ClassId => 224;
        
        public override ushort Version => 4;
        
        public override string Category => "";
    }
}
