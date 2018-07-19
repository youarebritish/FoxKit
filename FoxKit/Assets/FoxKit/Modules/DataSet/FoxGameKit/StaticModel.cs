namespace FoxKit.Modules.DataSet.Sdx
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.PartsBuilder;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    using AssetPostprocessor = FoxKit.Core.AssetPostprocessor;

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
        [SerializeField, Modules.DataSet.Property("Model")]
        private UnityEngine.Object modelFile;

        /// <summary>
        /// The collision file.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Geom")]
        private UnityEngine.Object geomFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Geom")]
        private bool isVisibleGeom;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Model")]
        private bool isIsolated = true;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("LOD")]
        private float lodFarSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("LOD")]
        private float lodNearSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("LOD")]
        private float lodPolygonSize = -1;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Rendering")]
        private Color color = UnityEngine.Color.white;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Rendering")]
        private DrawRejectionLevel drawRejectionLevel = DrawRejectionLevel.Default;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Rendering")]
        private DrawMode drawMode = DrawMode.Normal;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Rendering")]
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
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshRenderer)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 352;

        /// <inheritdoc />
        public override ushort Version => 9;

        /// <inheritdoc />
        public override void PostOnLoaded()
        {
            base.PostOnLoaded();

            if (this.modelFile != null)
            {
                // TODO make better
                var instance = UnityEngine.Object.Instantiate(this.modelFile) as GameObject;
                instance.transform.position = this.SceneProxyTransform.position;
                instance.transform.rotation = this.SceneProxyTransform.rotation;
                instance.transform.localScale = this.SceneProxyTransform.localScale;
                instance.transform.SetParent(this.SceneProxyTransform, true);
            }
        }

        /// <inheritdoc />
        public override void OnAssetsImported(AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.modelFilePath, out this.modelFile);
            tryGetAsset(this.geomFilePath, out this.geomFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("modelFile", Core.PropertyInfoType.FilePtr, DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.modelFile))));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("geomFile", Core.PropertyInfoType.FilePtr, DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.geomFile))));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isVisibleGeom", Core.PropertyInfoType.Bool, this.isVisibleGeom));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isIsolated", Core.PropertyInfoType.Bool, this.isIsolated));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodFarSize", Core.PropertyInfoType.Float, this.lodFarSize));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodNearSize", Core.PropertyInfoType.Float, this.lodNearSize));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodPolygonSize", Core.PropertyInfoType.Float, this.lodPolygonSize));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("color", Core.PropertyInfoType.Color, FoxUtils.UnityColorToFoxColorRGBA(this.color)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawRejectionLevel", Core.PropertyInfoType.Int32, this.drawRejectionLevel));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawMode", Core.PropertyInfoType.Int32, this.drawMode));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("rejectFarRangeShadowCast", Core.PropertyInfoType.Int32, this.rejectFarRangeShadowCast));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(
            Core.PropertyInfo propertyData,
            Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "modelFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(
                        DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "geomFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(
                        DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "isVisibleGeom":
                    this.isVisibleGeom = DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData);
                    break;
                case "isIsolated":
                    this.isIsolated = DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData);
                    break;
                case "lodFarSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "lodNearSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "lodPolygonSize":
                    this.lodFarSize = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "color":
                    var foxColor = DataSetUtils.GetStaticArrayPropertyValue<Core.ColorRGBA>(propertyData);
                    this.color = new Color(foxColor.Red, foxColor.Green, foxColor.Blue, foxColor.Alpha);
                    break;
                case "drawRejectionLevel":
                    this.drawRejectionLevel = (DrawRejectionLevel)DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData);
                    break;
                case "drawMode":
                    this.drawMode = (DrawMode)DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData);
                    break;
                case "rejectFarRangeShadowCast":
                    this.rejectFarRangeShadowCast = (RejectFarRangeShadowCast)DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData);
                    break;
            }
        }
    }
}