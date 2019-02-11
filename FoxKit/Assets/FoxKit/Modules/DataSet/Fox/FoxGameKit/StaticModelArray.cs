using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public enum StaticModelArray_DrawRejectionLevel : int
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

    public enum StaticModelArray_DrawMode : int
    {
        Normal = 0,
        ShadowOnly = 1,
        DisableShadow = 2
    }

    public enum StaticModelArray_RejectFarRangeShadowCast : int
    {
        NoReject = 0,
        Reject = 1,
        Default = 2
    }

    public partial class StaticModelArray
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(LODGroup)).image as Texture2D;
        
        /// <inheritdoc />
        public override void OnLoaded(CreateSceneProxyDelegate createSceneProxy)
        {
            base.OnLoaded(createSceneProxy);
            this.CreateSceneProxy(createSceneProxy);
        }

        /// <inheritdoc />
        public override void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
            base.PostOnLoaded(getSceneProxy);

            if (modelFile == null)
            {
                return;
            }

            int index = 0;
            var sceneProxy = getSceneProxy(this.Name);
            foreach(var transform in this.transforms)
            {
                var instanceProxy = new GameObject($"{this.Name}_{index.ToString("0000")}");
                instanceProxy.transform.SetParent(sceneProxy.transform);
                instanceProxy.transform.position = transform.GetColumn(3);
                instanceProxy.transform.position = new Vector3(-instanceProxy.transform.position.x, instanceProxy.transform.position.y, instanceProxy.transform.position.z);

                // Extract rotation.
                Vector3 forward;
                forward.x = transform.m02;
                forward.y = transform.m12;
                forward.z = transform.m22;

                Vector3 upwards;
                upwards.x = transform.m01;
                upwards.y = transform.m11;
                upwards.z = transform.m21;

                instanceProxy.transform.rotation = Quaternion.LookRotation(forward, upwards);
                instanceProxy.transform.rotation = new Quaternion(instanceProxy.transform.rotation.x, -instanceProxy.transform.rotation.y, instanceProxy.transform.rotation.z, instanceProxy.transform.rotation.w);

                var model = Object.Instantiate(this.modelFile) as GameObject;
                model.transform.SetParent(instanceProxy.transform);
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;

                index++;
            }
        }

        /// <inheritdoc />
        public override void OnUnloaded(DestroySceneProxyDelegate destroySceneProxy)
        {
            base.OnUnloaded(destroySceneProxy);
            this.DestroySceneProxy(destroySceneProxy);
        }

        private void CreateSceneProxy(CreateSceneProxyDelegate createSceneProxy)
        {
            if (this.modelFile == null)
            {
                return;
            }

            var sceneProxy = createSceneProxy();
            sceneProxy.transform.position = Vector3.zero;
        }

        private void DestroySceneProxy(DestroySceneProxyDelegate destroySceneProxy)
        {
            destroySceneProxy();
        }
    }
}
