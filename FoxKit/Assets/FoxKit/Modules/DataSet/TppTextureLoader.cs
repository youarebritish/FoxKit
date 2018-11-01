namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    /// <inheritdoc />
    [Serializable]
    public class TppTextureLoader : Data
    {
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.Path, 120, container: Core.ContainerType.StringMap)]
        private Dictionary<string, UnityEngine.Object> textures = new Dictionary<string, UnityEngine.Object>();

        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.Path, 160, container: Core.ContainerType.StringMap)]
        private Dictionary<string, UnityEngine.Object> forceLargeTextures = new Dictionary<string, UnityEngine.Object>();
    }
}