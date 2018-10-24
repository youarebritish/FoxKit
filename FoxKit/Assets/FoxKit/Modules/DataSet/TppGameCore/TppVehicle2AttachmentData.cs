namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure this out.
    /// </summary>
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt8, 177)]
        private byte vehicleTypeCode;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt8, 178)]
        private byte attachmentImplTypeIndex;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 136)]
        private UnityEngine.Object attachmentFile;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt8, 176)]
        private byte attachmentInstanceCount;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 160)]
        private string bodyCnpName = string.Empty;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 168)]
        private string attachmentBoneName = string.Empty;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityPtr, 120, ptrType: typeof(TppVehicle2WeaponParameter), container: Core.ContainerType.DynamicArray)]
        private List<Entity> weaponParams = new List<Entity>();
        
        /// <inheritdoc />
        public override short ClassId => 120;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}