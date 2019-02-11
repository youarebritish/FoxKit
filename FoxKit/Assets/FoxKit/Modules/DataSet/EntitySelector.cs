namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.Lua;

    using FoxLib;

    using OdinSerializer;

    /// <summary>
    /// Manages Selection of Entities.
    /// </summary>
    [Serializable, ExposeClassToLua]
    public class EntitySelector : Entity
    {
        /// <summary>
        /// The selected Entities.
        /// </summary>
        [OdinSerialize, NonSerialized,
         PropertyInfo(
             Core.PropertyInfoType.EntityPtr,
             0,
             1,
             Core.ContainerType.DynamicArray,
             PropertyExport.Never,
             PropertyExport.Never,
             typeof(Entity))]
        private readonly List<Entity> entities = new List<Entity>();

        /// <summary>
        /// Get the selected Entities.
        /// </summary>
        /// <returns>The selected Entities.</returns>
        public IEnumerable<Entity> GetEntities()
        {
            return this.entities;
        }
    }
}