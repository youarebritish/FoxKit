using UnityEditor;

namespace FoxKit.Modules.DataSet.Editor.Toolbar
{
    public class FoxCoreToolbar : Toolbar
    {
        [MenuItem("Window/FoxKit/FoxCore")]
        public static void ShowWindow()
        {
            var window = GetWindow<FoxCoreToolbar>("FoxCore", ToolbarTypes) as FoxCoreToolbar;
            window.Initialize();
        }
    }
}