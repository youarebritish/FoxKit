namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;

    using FoxLib;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine entities which are attached to another entity and have no independent existence.
    /// </summary>
    /// <typeparam name="TOwner">
    /// Type of Entity to which this DataElement can be attached.
    /// </typeparam>
    [Serializable]
    public abstract class DataElement : Entity//<TOwner> : Entity
        //where TOwner : Entity
    {
        /// <summary>
        /// The owner.
        /// </summary>
        [SerializeField]
        private Entity owner;

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        public Entity Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                this.owner = value;
            }
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            return new List<Core.PropertyInfo>
                       {
                           PropertyInfoFactory.MakeStaticArrayProperty("owner", Core.PropertyInfoType.EntityHandle, getEntityAddress(this.owner)),
                       };
        }
    }
}