using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoxKit.Modules.DataSet.Fox
{
    public partial class GeoModuleCondition : GeoTrapCondition
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(EventSystem)).image as Texture2D;
    }
}
