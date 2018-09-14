using FoxKit.Modules.DataSet.PartsBuilder;
using FoxKit.Modules.DataSet.Sdx;

using Sirenix.OdinInspector;

using UnityEngine;

public class StaticModelInspectorTest : MonoBehaviour
{
    [FoldoutGroup("Data"), ReadOnly]
    public string referencePath = "StaticModel0000";

    [FoldoutGroup("TransformData"), ReadOnly]
    public GameObject parent;

    [FoldoutGroup("TransformData")]
    public Vector3 translate;

    [FoldoutGroup("TransformData")]
    public Quaternion rotQuat;

    [FoldoutGroup("TransformData")]
    public Vector3 scale;

    [FoldoutGroup("TransformData")]
    public bool inheritTransform;

    [FoldoutGroup("TransformData")]
    public bool visibility;

    [FoldoutGroup("TransformData")]
    public bool selection;

    [FoldoutGroup("TransformData"), ReadOnly]
    public Matrix4x4 worldMatrix;

    [FoldoutGroup("TransformData"), ReadOnly]
    public Matrix4x4 worldTransform;

    [FoldoutGroup("StaticModel"), InlineEditor(InlineEditorModes.LargePreview)]
    public Mesh modelFile;

    [FoldoutGroup("StaticModel")]
    public Object geomFile;

    [FoldoutGroup("StaticModel")]
    public bool isVisibleGeom;

    [FoldoutGroup("StaticModel")]
    public bool isIsolated;

    [FoldoutGroup("StaticModel")]
    public float lodFarSize;

    [FoldoutGroup("StaticModel")]
    public float lodNearSize;

    [FoldoutGroup("StaticModel")]
    public float lodPolygonSize;

    [FoldoutGroup("StaticModel")]
    public Color color;

    [FoldoutGroup("StaticModel")]
    public DrawRejectionLevel drawRejectionLevel;

    [FoldoutGroup("StaticModel")]
    public DrawMode drawMode;

    [FoldoutGroup("StaticModel")]
    public RejectFarRangeShadowCast rejectFarRangeShadowCast;
}
