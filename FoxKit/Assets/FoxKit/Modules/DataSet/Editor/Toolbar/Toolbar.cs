using UnityEditor;
using UnityEngine;
using System.Linq;
using FoxKit.Modules.DataSet.Editor.Command;
using System;
using FoxKit.Utils;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Editor.Toolbar
{
    public abstract class Toolbar : EditorWindow
    {
        private readonly List<IToolbarCommand> commands = new List<IToolbarCommand>();

        // TODO: Find a better place for this
        private static readonly Type[] ToolbarTypes = ReflectionUtils.GetAssignableConcreteClasses(typeof(Toolbar)).ToArray();

        /// <summary>
        /// We need to call Initialize() during OnEnable(), otherwise all state will be lost after a recompile.
        /// </summary>
        private void OnEnable()
        {
            Initialize();
        }

        public static TToolbar Create<TToolbar>(string name) where TToolbar : Toolbar
        {
            return GetWindow<TToolbar>(name, ToolbarTypes) as TToolbar;
        }

        protected void Initialize()
        {
            this.commands.Clear();

            minSize = new Vector2(minSize.x, 40);
            maxSize = new Vector2(maxSize.x, minSize.y);

            var commands = (from type in ReflectionUtils.GetAssignableConcreteClasses(typeof(IToolbarCommand))
                            select Activator.CreateInstance(type) as IToolbarCommand);

            foreach (var command in commands)
            {
                var thisType = GetType();
                if (command.ToolbarType == thisType)
                {
                    this.commands.Add(command);
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var command in commands)
            {
                if (FoxKitUiUtils.ToolButton(command.Icon, command.Tooltip))
                {
                    command.Execute();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnDisable()
        {
            commands.Clear();
        }
    }
}