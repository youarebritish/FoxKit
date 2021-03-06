using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public enum DecalArray_ProjectionMode : int
    {
        Orthographic = 0,
        Perspective = 1,
        Cylindrical = 2
    }

    public enum DecalArray_ProjectionTarget : int
    {
        AnySurface = 0,
        TargetsOnly = 1
    }

    public enum DecalArray_PolygonDataSource : int
    {
        CharaCollisionData = 0,
        RecoilCollisionData = 1
    }

    public enum DecalArray_DrawRejectionLevel : int
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        NoReject = 5
    }

    public partial class DecalArray
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Tilemap)).image as Texture2D;
    }
}
