namespace FoxKit.Modules.DataSet.Sdx
{
    using System;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.PartsBuilder;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Structs;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;

    /// <summary>
    /// TODO: Figure out.
    /// </summary>
    public enum DrawMode
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        ShadowOnly = 1,

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        DisableShadow = 2
    }

    /// <inheritdoc />
    /// <summary>
    /// A static model that gets rendered in the world.
    /// </summary>
    [Serializable]
    public class StaticModel : TransformData
    {
        /// <summary>
        /// The model file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object modelFile;

        /// <summary>
        /// The collision file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object geomFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private bool isVisibleGeom;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private bool isIsolated = true;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodFarSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodNearSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private float lodPolygonSize = -1;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField]
        private Color color = UnityEngine.Color.white;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private DrawRejectionLevel drawRejectionLevel = DrawRejectionLevel.Default;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private DrawMode drawMode = DrawMode.Normal;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private RejectFarRangeShadowCast rejectFarRangeShadowCast = RejectFarRangeShadowCast.Default;

        /// <summary>
        /// The model file path.
        /// </summary>
        [SerializeField, HideInInspector]
        private string modelFilePath;

        /// <summary>
        /// The collision file path.
        /// </summary>
        [SerializeField, HideInInspector]
        private string geomFilePath;

        /// <inheritdoc />
        protected override short ClassId => 352;

        /// <inheritdoc />
        public override void OnAssetsImported(AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.modelFilePath, out this.modelFile);
            tryGetAsset(this.geomFilePath, out this.geomFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(
            FoxProperty propertyData,
            Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "modelFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(
                        DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "geomFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(
                        DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "isVisibleGeom":
                    this.isVisibleGeom = DataSetUtils.GetStaticArrayPropertyValue<FoxBool>(propertyData).Value;
                    break;
                case "isIsolated":
                    this.isIsolated = DataSetUtils.GetStaticArrayPropertyValue<FoxBool>(propertyData).Value;
                    break;
                case "lodFarSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "lodNearSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "lodPolygonSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "color":
                    var foxColor = DataSetUtils.GetStaticArrayPropertyValue<FoxColor>(propertyData);
                    this.color = new Color(foxColor.Red, foxColor.Green, foxColor.Blue, foxColor.Alpha);
                    break;
                case "drawRejectionLevel":
                    this.drawRejectionLevel = (DrawRejectionLevel)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
                    break;
                case "drawMode":
                    this.drawMode = (DrawMode)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
                    break;
                case "rejectFarRangeShadowCast":
                    this.rejectFarRangeShadowCast = (RejectFarRangeShadowCast)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
                    break;
            }
        }
    }
}