
using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoxKit.Modules.DataSet.Fox
{
    public partial class GeoTrap : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(EventTrigger)).image as Texture2D;
    }
}
