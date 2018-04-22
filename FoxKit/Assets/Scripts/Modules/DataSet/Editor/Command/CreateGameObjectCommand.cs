using UnityEngine;

namespace FoxKit.Modules.DataSet.Editor.Command
{
    public class CreateGameObjectCommand : IToolbarCommand
    {
        public Texture Icon => icon;        
        public string Tooltip => "Create a new GameObject.";

        private static readonly Texture icon = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
        
        public void Execute()
        {
            var go = new GameObject();
        }
    }
}