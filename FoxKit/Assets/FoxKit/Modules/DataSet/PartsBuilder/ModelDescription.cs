namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.Importer;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Root Entity in a PartsFile. Links together various files and data describing a model.
    /// </summary>
    public class ModelDescription : PartDescription
    {
        /// <summary>
        /// The model file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object modelFile;

        /// <summary>
        /// The connect point file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object connectPointFile;

        /// <summary>
        /// The game rig file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object gameRigFile;

        /// <summary>
        /// The help bone file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object helpBoneFile;

        /// <summary>
        /// The lip adjust binary file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object lipAdjustBinaryFile;

        /// <summary>
        /// The facial setting file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object facialSettingFile;

        /// <summary>
        /// The invisible mesh names.
        /// </summary>
        [SerializeField]
        private List<string> invisibleMeshNames;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodFarPixelSize = 50.0f;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodNearPixelSize = 400.0f;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodPolygonSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private ModelDescriptionDrawRejectionLevel drawRejectionLevel = ModelDescriptionDrawRejectionLevel.Default;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private ModelDescriptionRejectFarRangeShadowCast rejectFarRangeShadowCast = ModelDescriptionRejectFarRangeShadowCast.Default;

        /// <summary>
        /// The model file path.
        /// </summary>
        [SerializeField]
        private string modelFilePath;

        /// <summary>
        /// The connect point file path.
        /// </summary>
        [SerializeField]
        private string connectPointFilePath;

        /// <summary>
        /// The game rig file path.
        /// </summary>
        [SerializeField]
        private string gameRigFilePath;

        /// <summary>
        /// The help bone file path.
        /// </summary>
        [SerializeField]
        private string helpBoneFilePath;

        /// <summary>
        /// The lip adjust binary file path.
        /// </summary>
        [SerializeField]
        private string lipAdjustBinaryFilePath;

        /// <summary>
        /// The facial setting file path.
        /// </summary>
        [SerializeField]
        private string facialSettingFilePath;
        
        /// <summary>
        /// Not sure what this is. Perhaps the distance at which the model stops being rendered?
        /// </summary>
        public enum ModelDescriptionDrawRejectionLevel
        {
            /// <summary>
            /// Size: 0.5m
            /// </summary>
            [Description("LEVEL0(size:0.5m)")]
            Level0 = 0,

            /// <summary>
            /// Size: 1m
            /// </summary>
            [Description("LEVEL1(size:1m)")]
            Level1 = 1,

            /// <summary>
            /// Size: 2m
            /// </summary>
            [Description("LEVEL2(size:2m)")]
            Level2 = 2,

            /// <summary>
            /// Size: 4m
            /// </summary>
            [Description("LEVEL3(size:4m)")]
            Level3 = 3,

            /// <summary>
            /// Size: 8m
            /// </summary>
            [Description("LEVEL4(size:8m)")]
            Level4 = 4,

            /// <summary>
            /// Size: 16m
            /// </summary>
            [Description("LEVEL5(size:16m)")]
            Level5 = 5,

            /// <summary>
            /// Size: 32m
            /// </summary>
            [Description("LEVEL6(size:32m)")]
            Level6 = 6,

            /// <summary>
            /// Don't reject.
            /// </summary>
            [Description("NO_REJECT")]
            NoReject = 7,

            /// <summary>
            /// Default rejection level.
            /// </summary>
            [Description("DEFAULT")]
            Default = 8
        }

        /// <summary>
        /// Not sure what this is. Perhaps whether or not to cast a shadow at long range?
        /// </summary>
        public enum ModelDescriptionRejectFarRangeShadowCast
        {
            /// <summary>
            /// Don't reject far-range shadows.
            /// </summary>
            NoReject = 0,

            /// <summary>
            /// Reject far-range shadows.
            /// </summary>
            Reject = 1,

            /// <summary>
            /// Default rejection setting.
            /// </summary>
            Default = 2
        }
        
        /// <inheritdoc />
        protected override short ClassId => 288;

        /// <inheritdoc />
        public override void OnAssetsImported(AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.modelFilePath, out this.modelFile);
            tryGetAsset(this.connectPointFilePath, out this.connectPointFile);
            tryGetAsset(this.gameRigFilePath, out this.gameRigFile);
            tryGetAsset(this.helpBoneFilePath, out this.helpBoneFile);
            tryGetAsset(this.lipAdjustBinaryFilePath, out this.lipAdjustBinaryFile);
            tryGetAsset(this.facialSettingFilePath, out this.facialSettingFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "modelFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "connectPointFile":
                    this.connectPointFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "gameRigFile":
                    this.gameRigFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "helpBoneFile":
                    this.helpBoneFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "lipAdjustBinaryFile":
                    this.lipAdjustBinaryFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "facialSettingFile":
                    this.facialSettingFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "invisibleMeshNames":
                    this.invisibleMeshNames = (from name in DataSetUtils.GetDynamicArrayValues<FoxString>(propertyData)
                                               select name.ToString()).ToList();
                    break;
                case "lodFarPixelSize":
                    this.lodFarPixelSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "lodNearPixelSize":
                    this.lodNearPixelSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "lodPolygonSize":
                    this.lodPolygonSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "drawRejectionLevel":
                    this.drawRejectionLevel = (ModelDescriptionDrawRejectionLevel)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
                    break;
                case "rejectFarRangeShadowCast":
                    this.rejectFarRangeShadowCast = (ModelDescriptionRejectFarRangeShadowCast)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
                    break;
            }
        }
    }
}