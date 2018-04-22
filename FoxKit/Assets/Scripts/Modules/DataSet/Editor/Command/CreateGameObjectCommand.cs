using FoxKit.Modules.DataSet.Editor.Toolbar;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Editor.Command
{
    public class CreateGameObjectCommand : IToolbarCommand
    {
        public Texture Icon => icon;        
        public string Tooltip => "Create a new GameObject.";
        public Type ToolbarType => typeof(FoxCoreToolbar);

        private static readonly Texture icon = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
        
        public void Execute()
        {
            var go = new GameObject();
        }
    }
}