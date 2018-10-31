namespace FoxKit.Modules.DataSet.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.UI;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure out.
    /// </summary>
    [Serializable]
    public class UiGraphEntry : Data
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 120, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Object> files = new List<UnityEngine.Object>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 136, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Object> rawFiles = new List<UnityEngine.Object>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<string> filesPaths = new List<string>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<string> rawFilesPaths = new List<string>();

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(LayoutElement)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}