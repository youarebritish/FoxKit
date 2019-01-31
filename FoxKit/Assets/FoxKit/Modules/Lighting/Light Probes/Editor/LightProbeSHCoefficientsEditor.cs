namespace FoxKit.Modules.Lighting.Atmosphere
{
    using FoxKit.Modules.Lighting.LightProbes;
    using System;
    using System.Collections.Generic;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(LightProbeSHCoefficientsAsset))]
    public class LightProbeSHCoefficientsEditor : Editor
    {
        PreviewRenderUtility previewUtility;
        LightProbeSHCoefficientsAsset previewObject;

        private Material material;

        static Mesh s_SphereMesh;

        public float previewExposure = 1.0f;

        public int previewCoefficientSetIndex = 0;

        static Mesh previewMesh => s_SphereMesh ?? (s_SphereMesh = Resources.GetBuiltinResource(typeof(Mesh), "New-Plane.fbx") as Mesh);

        void OnEnable()
        {
            this.previewUtility = new PreviewRenderUtility(false, true);
            previewUtility.cameraFieldOfView = 30.0f;
            previewUtility.camera.nearClipPlane = 0.01f;
            previewUtility.camera.farClipPlane = 20.0f;
            previewUtility.camera.transform.position = new Vector3(0, 0, 2);
            previewUtility.camera.transform.LookAt(Vector3.up);

            this.previewObject = (LightProbeSHCoefficientsAsset)this.target;
        }

        void Awake()
        {
            var shader = Shader.Find("GrModelShaders_dx11/SH_SphereMap");
            this.material = new Material(shader)
                           {
                               hideFlags = HideFlags.HideAndDontSave
                           };
        }

        void OnDisable()
        {
            this.previewUtility.Cleanup();
            this.previewUtility = null;
            this.previewObject = null;
        }

        public override bool HasPreviewGUI()
        {
            return this.previewObject.LightProbes.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (this.material != null)
            {/*
                this.material.SetFloat("_Exposure", this.previewExposure);
                this.material.SetMatrixArray(
                    "_ParamSH",
                    new[]
                        {
                            this.previewObject.Coefficients[previewCoefficientSetIndex].TermR, this.previewObject.Coefficients[previewCoefficientSetIndex].TermG,
                            this.previewObject.Coefficients[previewCoefficientSetIndex].TermB, this.previewObject.Coefficients[previewCoefficientSetIndex].SkyOcclusion
                        });*/
            }

            this.previewUtility.BeginPreview(r, background);
            this.previewUtility.DrawMesh(previewMesh, Matrix4x4.identity, this.material, 0);
            this.previewUtility.camera.Render();
            this.previewUtility.EndAndDrawPreview(r);
        }

        public override void OnPreviewSettings()
        {
            if (s_ExposureLow == null)
                InitIcons();

            GUI.enabled = true;

            GUILayout.Box(s_ExposureLow, s_PreLabel, GUILayout.MaxWidth(20));
            GUI.changed = false;
            this.previewExposure = GUILayout.HorizontalSlider(this.previewExposure, 0f, 1f, GUILayout.MaxWidth(100));
            //this.previewCoefficientSetIndex = Mathf.RoundToInt(GUILayout.HorizontalSlider(this.previewCoefficientSetIndex, 0, this.previewObject.Coefficients.Count - 1, GUILayout.MaxWidth(100)));
        }

        static GUIContent s_MipMapLow;

        static GUIContent s_MipMapHigh;

        static GUIContent s_ExposureLow;

        static GUIStyle s_PreLabel;

        static void InitIcons()
        {
            s_ExposureLow = EditorGUIUtility.IconContent("SceneViewLighting");
            s_PreLabel = "preLabel";
        }
    }
}