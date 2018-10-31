namespace FoxKit.Modules.DataSet.Sdx
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Don't know what this is, but it looks like every DataSetFile needs one.
    /// </summary>
    [Serializable]
    public class TexturePackLoadConditioner : Data
    {
        /// <summary>
        /// The texture pack path.
        /// </summary>
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.Path, 120)]
        private UnityEngine.Object texturePackPath;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Texture)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 72;
    }
}