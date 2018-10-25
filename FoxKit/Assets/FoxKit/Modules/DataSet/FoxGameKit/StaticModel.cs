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
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 304)]
        private UnityEngine.Object modelFile;

        /// <summary>
        /// The collision file.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 328)]
        private UnityEngine.Object geomFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 352)]
        private bool isVisibleGeom;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 353)]
        private bool isIsolated = true;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 360)]
        private float lodFarSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 364)]
        private float lodNearSize = -1;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 368)]
        private float lodPolygonSize = -1;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Color, 368)]
        private Color color = UnityEngine.Color.white;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 384, enumType: typeof(DrawRejectionLevel))]
        private DrawRejectionLevel drawRejectionLevel = DrawRejectionLevel.Default;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 388, enumType: typeof(DrawMode))]
        private DrawMode drawMode = DrawMode.Normal;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 392, enumType: typeof(RejectFarRangeShadowCast))]
        private RejectFarRangeShadowCast rejectFarRangeShadowCast = RejectFarRangeShadowCast.Default;
        
        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshRenderer)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 352;

        /// <inheritdoc />
        public override ushort Version => 9;

        public UnityEngine.Object ModelFile
        {
            get
            {
                return this.modelFile;
            }
            set
            {
                this.modelFile = value;
            }
        }

        /// <inheritdoc />
        public override void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
            base.PostOnLoaded(getSceneProxy);

            if (this.modelFile != null)
            {
                // TODO make better
                var instance = PrefabUtility.InstantiatePrefab(this.modelFile) as GameObject;
                var sceneProxy = getSceneProxy(this.Name);

                if (this.modelFile is DefaultAsset)
                {
                    Debug.LogError($"Model file {this.modelFile.name} is not a recognized format. Did it import correctly?");
                    return;
                }

                instance.transform.position = sceneProxy.transform.position;
                instance.transform.rotation = sceneProxy.transform.rotation;
                instance.transform.localScale = sceneProxy.transform.localScale;
                instance.transform.SetParent(sceneProxy.transform, true);
            }
        }
    }
}