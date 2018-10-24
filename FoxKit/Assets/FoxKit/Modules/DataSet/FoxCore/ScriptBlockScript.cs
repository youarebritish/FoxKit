namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;
    
    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// The script block script.
    /// </summary>
    [Serializable]
    public class ScriptBlockScript : Data
    {
        /// <summary>
        /// The Lua script to execute when the ScriptBlock activates.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 24)]
        private UnityEngine.Object script;
        
        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MonoScript)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 88;
    }
}