namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="GameObjectLocator"/> with the type TppHostage2.
    /// </summary>
    public class TppHostage2LocatorParameter : GameObjectLocatorParameter
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 56)]
        private string identifier;

        /// <inheritdoc />
        public override short ClassId => 32;
    }
}