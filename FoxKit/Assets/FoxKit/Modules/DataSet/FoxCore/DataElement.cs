namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine entities which are attached to another entity and have no independent existence.
    /// </summary>
    /// <typeparam name="TOwner">
    /// Type of Entity to which this DataElement can be attached.
    /// </typeparam>
    [Serializable]
    public abstract class DataElement : Entity
    {
        /// <summary>
        /// The owner.
        /// </summary>
        [OdinSerialize, NonSerialized, PropertyInfo(Core.PropertyInfoType.EntityHandle, 48, readable: PropertyExport.EditorOnly, writable: PropertyExport.EditorOnly)]
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
    }
}