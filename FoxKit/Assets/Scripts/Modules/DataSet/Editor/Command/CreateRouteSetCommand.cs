using FoxKit.Modules.RouteBuilder;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Editor.Command
{
    public class CreateRouteSetCommand : IToolbarCommand
    {
        public Texture Icon => icon;        
        public string Tooltip => "Create a new RouteSet.";

        private static readonly Texture icon = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
        
        public void Execute()
        {
            CreateRouteSetEditor.CreateRouteSet();
        }
    }
}