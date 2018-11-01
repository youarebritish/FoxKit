namespace FoxKit.Modules.Gr.GrTexture.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;
    using FoxKit.Modules.Gr.GrTexture.Utils;
    using FoxKit.Modules.Gr.GrTexture.Importer;
    using FoxKit.Modules.Gr.GrTexture.Exporter;
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using System.Reflection;
    using static GrTextureEditorUtils;

    /// <summary>
    /// Custom editor for Texture2Ds.
    /// </summary>
    //[CustomEditor(typeof(Texture2D))]
    public class GrTexture2DEditor : Editor
    {
        GrTextureUserData textureData;
        AssetImporter assetImporter;

        Editor defaultEditor;

        InspectorType inspectorType = InspectorType.Default;

        Material sRGBPreviewMaterial;
        Material normalPreviewMaterial;
        Material linearPreviewMaterial;
        Material defaultPreviewMaterial;

        int sRGBAlphaPropertyID = Shader.PropertyToID("_Alpha");

        Material normalMaterial;

        Vector4 linearSwizzle = new Vector4(0, 1, 0, 0);
        int linearSwizzlePropertyID = Shader.PropertyToID("_Swizzle");
        int linearFullPropertyID = Shader.PropertyToID("_Full");

        int mipLevelPropertyID = Shader.PropertyToID("_MipLevel");

        int sRGBOptionIndex = 0;

        string[] linearOptions = { "XYZ", "  X  ", "  Y  ", "  Z  " };
        int linearOptionIndex = 0;

        int normalOptionIndex = 0;

        int mipLevel = 0;

        //default styles
        GUIContent smallZoom;
        GUIStyle previewLabel;
        GUIStyle previewSlider;
        GUIStyle previewSliderThumbnail;
        GUIContent largeZoom;

        private void OnEnable()
        {
            var target = base.target;
            var assetPath = AssetDatabase.GetAssetPath(target);
            assetImporter = AssetImporter.GetAtPath(assetPath);

            switch (System.IO.Path.GetExtension(assetPath).ToLower())
            {
                case ".ftex":
                    inspectorType = InspectorType.FTEX;
                    break;
                case ".dds":
                    inspectorType = InspectorType.DDS;
                    break;
                default:
                    defaultEditor = Editor.CreateEditor(target);
                    inspectorType = InspectorType.Default;
                    break;
            }

            switch (inspectorType)
            {
                case InspectorType.FTEX:
                case InspectorType.DDS:
                    textureData = ParseTextureData(assetImporter.userData);

                    sRGBPreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/2D/sRGB"));
                    normalPreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/2D/Normal"));
                    linearPreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/2D/Linear"));
                    defaultPreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/2D/Default"));

                    smallZoom = EditorGUIUtility.IconContent("PreTextureMipMapLow");
                    previewLabel = CreatePreviewLabelStyle(); //new GUIStyle("preLabel");
                    previewSlider = CreatePreviewSliderStyle(); //"preSlider";
                    previewSliderThumbnail = CreatePreviewSliderThumbnailStyle(); //"preSliderThumb";
                    largeZoom = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
                    break;
            }
        }

        public override void OnInspectorGUI()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnInspectorGUI();
                    return;
                default:
                    return;
            }
        }

        public override bool HasPreviewGUI()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    return defaultEditor.HasPreviewGUI();
                default:
                    return true;
            }
        }

        public override void OnPreviewSettings()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnPreviewSettings();
                    break;
                default:
                    var target = (Texture2D)base.target;

                    if (target.mipmapCount > 1)
                    {
                        GUILayout.Box(smallZoom, previewLabel);
                        GUI.changed = false;
                        mipLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(mipLevel, target.mipmapCount - 1, 0, /*previewSlider, new GUIStyle(), previewSliderThumbnail ,*/ GUILayout.MaxWidth(64)));
                        GUILayout.Box(largeZoom, previewLabel);
                    }

                    switch (textureData.textureType)
                    {
                        case FoxLib.GrTexture.TextureType.SRGB:
                            if (GUILayout.Button("     "))
                            {
                                sRGBOptionIndex++;

                                if (sRGBOptionIndex > 1)
                                    sRGBOptionIndex = 0;
                            }
                            break;
                        case FoxLib.GrTexture.TextureType.Linear:
                            if (GUILayout.Button(linearOptions[linearOptionIndex]))
                            {
                                linearOptionIndex++;

                                if (linearOptionIndex > 3)
                                    linearOptionIndex = 0;
                            }

                            linearSwizzle.x = Convert.ToSingle(linearOptionIndex == 1);
                            linearSwizzle.y = Convert.ToSingle(linearOptionIndex == 2);
                            linearSwizzle.z = Convert.ToSingle(linearOptionIndex == 3);

                            linearPreviewMaterial.SetVector(linearSwizzlePropertyID, linearSwizzle);
                            linearPreviewMaterial.SetInt(linearFullPropertyID, Convert.ToInt32(!(linearOptionIndex == 0)));
                            break;
                        case FoxLib.GrTexture.TextureType.Normal:
                            if (GUILayout.Button("     "))
                            {
                                normalOptionIndex++;

                                if (normalOptionIndex > 1)
                                    normalOptionIndex = 0;
                            }
                            break;
                    }
                    break;
            }

            return;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnPreviewGUI(r, background);
                    break;
                default:
                    var target = (Texture2D)base.target;

                    // I fiercely hate doing this, but I can't figure out another way.
                    textureData = ParseTextureData(assetImporter.userData);

                    switch (textureData.textureType)
                    {
                        case FoxLib.GrTexture.TextureType.SRGB:
                            sRGBPreviewMaterial.SetFloat(mipLevelPropertyID, mipLevel);
                            sRGBPreviewMaterial.SetInt(sRGBAlphaPropertyID, sRGBOptionIndex);

                            EditorGUI.DrawPreviewTexture(r, target, sRGBPreviewMaterial, ScaleMode.ScaleToFit, 0);
                            break;
                        case FoxLib.GrTexture.TextureType.Normal:
                            normalMaterial = normalOptionIndex > 0 ? defaultPreviewMaterial : normalPreviewMaterial;
                            normalMaterial.SetFloat(mipLevelPropertyID, mipLevel);
                            EditorGUI.DrawPreviewTexture(r, target, normalMaterial, ScaleMode.ScaleToFit, 0);
                            break;
                        case FoxLib.GrTexture.TextureType.Linear:
                            linearPreviewMaterial.SetFloat(mipLevelPropertyID, mipLevel);

                            EditorGUI.DrawPreviewTexture(r, target, linearPreviewMaterial, ScaleMode.ScaleToFit, 0);
                            break;
                        default:
                            sRGBPreviewMaterial.SetFloat(mipLevelPropertyID, mipLevel);

                            EditorGUI.DrawPreviewTexture(r, target, sRGBPreviewMaterial, ScaleMode.ScaleToFit, 0);
                            break;
                    }
                    break;
            }

            return;
        }
    }

    /// <summary>
    /// Custom editor for Texture3s.
    /// </summary>
    [CustomEditor(typeof(Texture3D))]
    public class GrTexture3DEditor : Editor
    {
        GrTextureUserData textureData;
        AssetImporter assetImporter;
        SerializedObject serializedImporter;

        Editor defaultEditor;

        InspectorType inspectorType = InspectorType.Default;

        Material flatPreviewMaterial;
        Material slicePreviewMaterial;

        float slice;

        int slicePropertyID = Shader.PropertyToID("_Slice");

        int sliceOptionIndex;

        //default styles
        GUIStyle previewLabel;
        GUIStyle previewSlider;
        GUIStyle previewSliderThumbnail;

        private void OnEnable()
        {
            GUI.enabled = true;

            var target = base.target;
            var assetPath = AssetDatabase.GetAssetPath(target);
            assetImporter = AssetImporter.GetAtPath(assetPath);

            serializedImporter = new SerializedObject(assetImporter);

            switch (System.IO.Path.GetExtension(assetPath).ToLower())
            {
                case ".ftex":
                    inspectorType = InspectorType.FTEX;
                    break;
                case ".asset":
                    inspectorType = InspectorType.DDS;
                    break;
                default:
                    defaultEditor = Editor.CreateEditor(assetImporter);
                    inspectorType = InspectorType.Default;
                    break;
            }

            switch (inspectorType)
            {
                case InspectorType.FTEX:
                case InspectorType.DDS:
                    textureData = ParseTextureData(assetImporter.userData);

                    flatPreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/3D/Flat"));
                    slicePreviewMaterial = new Material(Shader.Find("Hidden/FoxKit/GrTexturePreviews/3D/Slice"));

                    previewLabel = CreatePreviewLabelStyle(); //new GUIStyle("preLabel");
                    previewSlider = CreatePreviewSliderStyle(); //"preSlider";
                    previewSliderThumbnail = CreatePreviewSliderThumbnailStyle(); //"preSliderThumb";
                    break;
            }
        }

        public override void OnInspectorGUI()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnInspectorGUI();
                    break;
                default:
                    switch (inspectorType)
                    {
                        case InspectorType.DDS:
                            GUI.enabled = true;
                            break;
                    }
                    break;
            }
            return;
        }

        public override bool HasPreviewGUI()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    return defaultEditor.HasPreviewGUI();
                default:
                    return true;
            }
        }

        public override void OnPreviewSettings()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnPreviewSettings();
                    break;
                default:
                    var target = (Texture3D)base.target;

                    switch (sliceOptionIndex)
                    {
                        case 1:
                            GUILayout.Box(" 0 ", previewLabel);
                            slice = GUILayout.HorizontalSlider(slice, 0, 1, /*previewSlider, new GUIStyle(), previewSliderThumbnail ,*/ GUILayout.MaxWidth(64));
                            GUILayout.Box(" 1 ", previewLabel);
                            break;
                        default:
                            break;
                    }

                    if (GUILayout.Button("     "))
                    {
                        sliceOptionIndex++;

                        if (sliceOptionIndex > 1)
                            sliceOptionIndex = 0;
                    }
                    break;
            }

            return;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnPreviewGUI(r, background);
                    break;
                default:
                    var target = (Texture3D)base.target;

                    switch (sliceOptionIndex)
                    {
                        case 0:
                            EditorGUI.DrawPreviewTexture(r, target, flatPreviewMaterial, ScaleMode.ScaleToFit, 16);
                            break;
                        default:
                            slicePreviewMaterial.SetFloat(slicePropertyID, slice);

                            EditorGUI.DrawPreviewTexture(r, target, slicePreviewMaterial, ScaleMode.ScaleToFit, 0);
                            break;
                    }
                    break;
            }

            return;
        }
    }

    /// <summary>
    /// Custom editor for GrTextureImporters.
    /// </summary>
    [CustomEditor(typeof(GrTextureImporter))]
    public class GrTextureImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(((GrTextureImporter)base.target).userData);
        }
    }

    /// <summary>
    /// Custom editor for IHVImageFormatImporters.
    /// </summary>
    [CustomEditor(typeof(IHVImageFormatImporter))]
    public class GrTextureIHVImporterEditor : AssetImporterEditor
    {
        SerializedProperty importerIsSRGB;

        InspectorType inspectorType;
        GrTextureUserData textureData;

        Editor defaultEditor;

        string assetPath;

        public override void OnEnable()
        {
            var target = (IHVImageFormatImporter)base.target;
            var assetTarget = base.assetTarget;

            assetPath = AssetDatabase.GetAssetPath(assetTarget);

            importerIsSRGB = serializedObject.FindProperty("m_sRGBTexture");

            textureData = ParseTextureData(target.userData);

            switch (System.IO.Path.GetExtension(assetPath).ToLower())
            {
                case ".dds":
                    if ((base.assetTarget as Texture) == null)
                    {
                        inspectorType = InspectorType.Failed;
                    }
                    else
                    {
                        inspectorType = InspectorType.DDS;
                    }

                    break;
                default:
                    defaultEditor = Editor.CreateEditor(target);
                    inspectorType = InspectorType.Default;
                    break;
            }
        }

        public override void OnInspectorGUI()
        {
            switch (inspectorType)
            {
                case InspectorType.Default:
                    defaultEditor.OnInspectorGUI();
                    break;
                case InspectorType.Failed:
                    const string buttonName = "Import 3D texture";
                    EditorGUILayout.HelpBox($"Importation has failed. If you are attempting to import a 3D texture, please select the \"{buttonName}\" button below.", MessageType.Warning);

                    if(GUILayout.Button(buttonName))
                    {
                        var texAsset = System.IO.Path.ChangeExtension(assetPath, ".asset");
                        if (!System.IO.File.Exists(texAsset))
                        {
                            var textureData = DirectXTexHelper.LoadDDS(assetPath);
                            var texture = GrTextureUtils.CreateTexture3D(textureData.width, textureData.height, textureData.depth, GrTextureUtils.GetTextureFormat(textureData.format, textureData.depth), textureData.pixels);
                            AssetDatabase.CreateAsset(texture, texAsset);
                        }
                    }
                    break;
                default:
                    EditorGUILayout.LabelField( ((IHVImageFormatImporter)base.target).userData );

                    EditorGUI.BeginChangeCheck();

                    textureData.textureType = UnflattenTextureType((FlatTextureType)EditorGUILayout.EnumPopup(FlattenTextureType(textureData.textureType)));

                    ((IHVImageFormatImporter)base.target).userData = CreateTextureData(textureData);

                    if (EditorGUI.EndChangeCheck())
                    {
                        SetColorSpace();

                        base.ApplyAndImport();
                    }

                    break;
            }
            return;
        }

        private void SetColorSpace()
        {
            base.serializedObject.FindProperty("m_sRGBTexture").boolValue = !GrTextureUtils.GetLinear(textureData.textureType);
        }
    }

    public class GrTextureMenuItems
    {
        [MenuItem("CONTEXT/Texture2D/Convert")]
        static void Convert2D(MenuCommand cmd)
        {
            var target = (Texture2D)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            WriteTexture(target, getExportPath);
        }

        [MenuItem("CONTEXT/Texture2D/Quick Convert")]
        static void QuickConvert2D(MenuCommand cmd)
        {
            var target = (Texture2D)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            WriteTexture(target, getExportPath);
        }

        [MenuItem("CONTEXT/Cubemap/Convert Cubemap")]
        static void ConvertCube(MenuCommand cmd)
        {
            var target = (Cubemap)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            WriteTexture(target, getExportPath);
        }

        [MenuItem("CONTEXT/Cubemap/Quick Convert Cubemap")]
        static void QuickConvertCube(MenuCommand cmd)
        {
            var target = (Cubemap)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            WriteTexture(target, getExportPath);
        }

        [MenuItem("CONTEXT/Texture3D/Convert")]
        static void Convert3D(MenuCommand cmd)
        {
            var target = (Texture3D)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => EditorUtility.SaveFilePanel("Convert", string.Empty, target.name + assetExtension, assetExtension);

            WriteTexture(target, getExportPath);
        }

        [MenuItem("CONTEXT/Texture3D/Quick Convert")]
        static void QuickConvert3D(MenuCommand cmd)
        {
            var target = (Texture3D)cmd.context;

            //string getExportPath(string assetPath, string assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            Func<string, string, string> getExportPath = (assetPath, assetExtension) => System.IO.Path.ChangeExtension(assetPath, assetExtension);

            WriteTexture(target, getExportPath);
        }

        private static void WriteTexture(Texture2D target, Func<string, string, string> getExportPath)
        {
            var data = FoxKit.Modules.Gr.GrTexture.Utils.DirectXTexHelper.Flip2DImage((uint)target.width, (uint)target.height, (uint)target.mipmapCount, GrTextureUtils.GetDXGIFormat(target.format), target.GetRawTextureData());
            WriteToConvertedFormat(target, (uint)target.width, (uint)target.height, 1, GrTextureUtils.GetTextureFormat(target.format), (uint)target.mipmapCount, data, getExportPath);
        }

        private static void WriteTexture(Cubemap target, Func<string, string, string> getExportPath)
        {
            List<byte> rawData = new List<byte>();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < target.mipmapCount; j++)
                {
                    var colors = target.GetPixels( (CubemapFace)i, j );

                    for (int h = 0; h < colors.Length; h++)
                    {
                        byte[][] rawBytes = { BitConverter.GetBytes(colors[h].r), BitConverter.GetBytes(colors[h].g), BitConverter.GetBytes(colors[h].b), BitConverter.GetBytes(colors[h].a) };
                        rawData.AddRange(rawBytes[0]);
                        rawData.AddRange(rawBytes[1]);
                        rawData.AddRange(rawBytes[2]);
                        rawData.AddRange(rawBytes[3]);
                    }
                }
            }

            var bytes = rawData.ToArray();
            var data = FoxKit.Modules.Gr.GrTexture.Utils.DirectXTexHelper.Flip2DImage((uint)target.width, (uint)target.height, (uint)target.mipmapCount, GrTextureUtils.GetDXGIFormat(target.format), bytes);
            WriteToConvertedFormat(target, (uint)target.width, (uint)target.height, 1, GrTextureUtils.GetTextureFormat(target.format), (uint)target.mipmapCount, data, getExportPath);
        }

        private static void WriteTexture(Texture3D target, Func<string, string, string> getExportPath)
        {
            var colours = target.GetPixels32();
            var newColours = new Color32[colours.Length];
            for (var i = 0; i < colours.Length; i++)
            {
                var invertedGIndex = (colours.Length - 1) - i;
                //colours[i] = new Color32(colours[i].b, colours[invertedGIndex].g, colours[i].r, colours[i].a);
                newColours[i].r = colours[i].b;
                newColours[i].g = colours[invertedGIndex].g;
                newColours[i].b = colours[i].r;
                newColours[i].a = colours[i].a;
            }

            WriteToConvertedFormat(target, (uint)target.width, (uint)target.height, (uint)target.depth, GrTextureUtils.GetTextureFormat(TextureFormat.BGRA32), 1, Color32ArrayToByteArray(newColours), getExportPath);
        }

        private static void WriteToConvertedFormat(Texture target, uint width, uint height, uint depth, ushort format, uint mipmapCount, byte[] textureData, Func<string, string, string> getExportPath)
        {
            var assetPath = AssetDatabase.GetAssetPath(target);
            var assetExtension = System.IO.Path.GetExtension(assetPath);
            var inverseAssetExtension = assetExtension == ".ftex" ? ".dds" : ".ftex";

            var exportPath = getExportPath(assetPath, inverseAssetExtension);

            if (string.IsNullOrEmpty(exportPath))
            {
                return;
            }

            switch (assetExtension)
            {
                case ".dds":
                    GrTextureExporter.ExportGrTexture(assetPath, (ushort)width, (ushort)height, (ushort)depth, format, (byte)mipmapCount, textureData, exportPath);
                    break;
                default:
                    DirectXTexHelper.SaveAsDDS(width, height, depth, GrTextureUtils.GetDXGIFormat(format), mipmapCount, textureData, exportPath);
                    break;
            }
        }

        // Credit: user3263058, 2014-02-5
        private static byte[] Color32ArrayToByteArray(Color32[] colors)
        {
            if (colors == null || colors.Length == 0)
                return null;

            int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
            int length = lengthOfColor32 * colors.Length;
            byte[] bytes = new byte[length];

            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                Marshal.Copy(ptr, bytes, 0, length);
            }
            finally
            {
                if (handle != default(GCHandle))
                    handle.Free();
            }

            return bytes;
        }
    }

    internal static class GrTextureEditorUtils
    {
        public enum InspectorType
        {
            Failed,
            Default,
            DDS,
            FTEX
        }

        public enum FlatTextureType
        {
            Linear,
            SRGB,
            Normal
        }

        public static FlatTextureType FlattenTextureType(FoxLib.GrTexture.TextureType source)
        {
            FlatTextureType result;

            switch (source)
            {
                case FoxLib.GrTexture.TextureType.Linear:
                    result = FlatTextureType.Linear;
                    break;
                case FoxLib.GrTexture.TextureType.Normal:
                    result = FlatTextureType.Normal;
                    break;
                default:
                    result = FlatTextureType.SRGB;
                    break;
            }

            return result;
        }

        public static FoxLib.GrTexture.TextureType UnflattenTextureType(FlatTextureType source)
        {
            FoxLib.GrTexture.TextureType result;

            switch (source)
            {
                case FlatTextureType.Linear:
                    result = FoxLib.GrTexture.TextureType.Linear;
                    break;
                case FlatTextureType.Normal:
                    result = FoxLib.GrTexture.TextureType.Normal;
                    break;
                default:
                    result = FoxLib.GrTexture.TextureType.SRGB;
                    break;
            }

            return result;
        }

        public struct GrTextureUserData
        {
            public byte nrtFlag;
            public FoxLib.GrTexture.TextureType textureType;
            public FoxLib.GrTexture.UnknownFlags unknownFlags;
        }

        public static GrTextureUserData ParseTextureData(string userData)
        {
            string[] separators = { "NrtFlag: ", ", TextureType: ", ", UnknownFlags: " };
            var flags = userData.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            GrTextureUserData grTextureData;
            try
            {
                if (!byte.TryParse(flags[0], out grTextureData.nrtFlag))
                {
                    Debug.Log($"Error: Incorrect nrt flag value: {flags[0]}");
                    grTextureData.nrtFlag = 2;
                }
                else
                {
                    if (!(grTextureData.nrtFlag == 0x0 || grTextureData.nrtFlag == 0x2))
                    {
                        Debug.Log($"Error: Incorrect nrt flag value: {grTextureData.nrtFlag}");
                        grTextureData.nrtFlag = 2;
                    }
                }

                if (!FoxLib.GrTexture.TextureType.TryParse(flags[1], out grTextureData.textureType))
                {
                    Debug.Log($"Error: Incorrect texture type value: {flags[1]}");
                    grTextureData.textureType = FoxLib.GrTexture.TextureType.Linear;
                }

                if (!FoxLib.GrTexture.UnknownFlags.TryParse(flags[2], out grTextureData.unknownFlags))
                {
                    Debug.Log($"Error: Incorrect unknown flags: {flags[2]}");
                    grTextureData.unknownFlags = FoxLib.GrTexture.UnknownFlags.Default;
                }
            }
            catch
            {
                grTextureData = CreateTextureData();
            }

            return grTextureData;
        }

        public static string CreateTextureData(GrTextureUserData userData)
        {
            return "NrtFlag: " + userData.nrtFlag + ", TextureType: " + userData.textureType + ", UnknownFlags: " + userData.unknownFlags;
        }

        public static GrTextureUserData CreateTextureData()
        {
            GrTextureUserData userData;
            userData.nrtFlag = 2;
            userData.textureType = FoxLib.GrTexture.TextureType.Linear;
            userData.unknownFlags = FoxLib.GrTexture.UnknownFlags.Default;
            return userData;
        }

        public static GUIStyle CreatePreviewLabelStyle()
        {
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.clipping = TextClipping.Clip;
            style.fixedHeight = 17;
            style.padding = new RectOffset(1, 1, 0, 0);
            style.richText = false;

            return style;
        }

        public static GUIStyle CreatePreviewSliderThumbnailStyle()
        {
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleLeft;
            style.border = new RectOffset(5, 5, 0, 0);
            style.clipping = TextClipping.Clip;
            style.fixedHeight = 17;
            style.fixedWidth = 12;
            style.overflow = new RectOffset(0, 0, -4, -1);
            style.richText = false;

            return style;
        }

        public static GUIStyle CreatePreviewSliderStyle()
        {
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleLeft;
            style.border = new RectOffset(5, 5, 0, 0);
            style.clipping = TextClipping.Clip;
            style.fixedHeight = 17;
            style.margin = new RectOffset(3, 3, 0, 0);
            style.overflow = new RectOffset(0, 0, -7, -5);
            style.padding = new RectOffset(-1, -1, 0, 0);
            style.richText = false;

            return style;
        }
    }
}