namespace FoxKit.Modules.DataSet
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    /// <inheritdoc />
    [Serializable]
    public class TppSkyEffectControler : Data
    {
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 120)]
        private FoxCore.EntityLink cameraLight = new EntityLink();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 160)]
        private uint hour;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 164)]
        private uint minute;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 168)]
        private uint second;
    }
}