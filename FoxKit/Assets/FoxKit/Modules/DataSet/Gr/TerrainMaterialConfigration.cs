namespace FoxKit.Modules.DataSet.Gr
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using UnityEngine;

    [Serializable]
    public class TerrainMaterialConfigration : Data
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 120, container: Core.ContainerType.DynamicArray)]
        private List<uint> slot0 = new List<uint>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 136, container: Core.ContainerType.DynamicArray)]
        private List<uint> slot1 = new List<uint>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 152, container: Core.ContainerType.DynamicArray)]
        private List<uint> slot2 = new List<uint>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 168, container: Core.ContainerType.DynamicArray)]
        private List<uint> slot3 = new List<uint>();
    }
}