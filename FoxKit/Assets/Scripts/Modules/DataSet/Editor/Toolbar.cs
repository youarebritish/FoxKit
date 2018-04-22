using UnityEditor;
using UnityEngine;
using System.Linq;
using FoxKit.Modules.DataSet.Editor.Command;
using System;
using System.Reflection;
using System.Collections.Generic;
using FoxKit.Utils;

namespace FoxKit.Modules.DataSet.Editor
{
    public class Toolbar : EditorWindow
    {
        public string Name = "FoxKit";
        private List<IToolbarCommand> commands;

        [MenuItem("Window/FoxKit/Toolbar")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(Toolbar)) as Toolbar;
            window.Initialize();
        }

        protected void Initialize()
        {
            titleContent = new GUIContent(Name);
            minSize = new Vector2(minSize.x, 40);

            var commands = from type in Assembly.GetAssembly(typeof(IToolbarCommand)).GetTypes()
                           where (typeof(IToolbarCommand)).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
                           select Activator.CreateInstance(type) as IToolbarCommand;
            this.commands = commands.ToList();

            foreach (var command in this.commands)
            {
                Debug.Log(command.GetType().ToString());
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            foreach (var command in commands)
            {
                if (FoxKitUiUtils.ToolButton(command.Icon, command.Tooltip))
                {
                    command.Execute();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}