using UnityEditor;

namespace FoxKit.Modules.DataSet.Editor.Toolbar
{
    public class AiToolbar : Toolbar
    {
        [MenuItem("Window/FoxKit/Ai")]
        public static void ShowWindow()
        {
            var window = GetWindow<AiToolbar>("Ai", ToolbarTypes) as AiToolbar;
            window.Initialize();
        }
    }
}