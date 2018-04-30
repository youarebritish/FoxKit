namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;

    using FoxKit.Modules.DataSet.GameCore;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="T:FoxKit.Modules.DataSet.TppGameCore.TppVehicle2LocatorParameter" /> Entity.
    /// </summary>
    [Serializable]
    public class TppVehicle2LocatorParameter : GameObjectLocatorParameter
    {
        /// <inheritdoc />
        protected override short ClassId => 28;
    }
}