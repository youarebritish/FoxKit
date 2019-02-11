namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using UnityEditor;
    using UnityEngine;

    public enum StaticModel_DrawRejectionLevel : int
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        NoReject = 7,
        Default = 8
    }

    public enum StaticModel_DrawMode : int
    {
        Normal = 0,
        ShadowOnly = 1,
        DisableShadow = 2
    }

    public enum StaticModel_RejectFarRangeShadowCast : int
    {
        NoReject = 0,
        Reject = 1,
        Default = 2
    }

    public partial class StaticModel
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshFilter)).image as Texture2D;

        public UnityEngine.Object ModelFile
        {
            get
            {
                return this.modelFile;
            }
            set
            {
                // TODO if loaded, change model
                this.modelFile = value;
            }
        }

        public StaticModel()
            : base()
        {
            // Default values taken from SOC. Figure out what these actually do.
            this.lodFarSize = -1;
            this.lodNearSize = -1;
            this.lodPolygonSize = -1;
            this.color = Color.white;
            this.drawRejectionLevel = StaticModel_DrawRejectionLevel.Default;
            this.drawMode = StaticModel_DrawMode.Normal;
            this.rejectFarRangeShadowCast = StaticModel_RejectFarRangeShadowCast.Default;
            this.Transform = new TransformEntity { Owner = this };
        }
        
        public override void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
            base.PostOnLoaded(getSceneProxy);

            if (this.ModelFile == null)
            {
                return;
            }

            var model = Object.Instantiate(this.ModelFile) as UnityEngine.GameObject;

            var sceneProxy = getSceneProxy(this.Name);
            sceneProxy.DrawLocatorGizmo = false;
            sceneProxy.gameObject.isStatic = true;

            model.transform.SetParent(sceneProxy.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;

            var modelProxy = model.AddComponent<SceneProxyChild>();
            modelProxy.Owner = sceneProxy;
            modelProxy.SetModel(this.ModelFile);
        }
    }
}
