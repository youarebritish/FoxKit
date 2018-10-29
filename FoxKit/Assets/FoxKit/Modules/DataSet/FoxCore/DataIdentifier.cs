namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    
    using FoxLib;

    using OdinSerializer;
    
    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// An Entity which allows other Entities to be referenced by string identifiers.
    /// </summary>
    [Serializable]
    public class DataIdentifier : Data
    {
        /// <summary>
        /// Globally unique identifier for this DataIdentifier. No two DataIdentifiers may share the same identifier!
        /// </summary>
        /// <example>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 120)]
        private string identifier = string.Empty;

        /// <summary>
        /// Maps identifiers to Entities, allowing them to be referenced externally.
        /// </summary>
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 128, container: Core.ContainerType.StringMap)]
        private Dictionary<string, EntityLink> links = new Dictionary<string, EntityLink>();

        public override short ClassId => 168;

        public string Identifier => this.identifier;

        public Dictionary<string, EntityLink> Links => this.links;
    }
}