using UnityEditor;

namespace FoxKit.Modules.DataSet.Editor.Toolbar
{
    public class AiToolbar : Toolbar
    {
        [MenuItem("Window/FoxKit/Ai")]
        public static void ShowWindow()
        {
            var window = Create<AiToolbar>("Ai");
            window.Initialize();
        }
    }
}