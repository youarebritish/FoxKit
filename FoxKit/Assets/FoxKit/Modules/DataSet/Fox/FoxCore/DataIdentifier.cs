namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    public partial class DataIdentifier
    {
        public string Identifier => this.identifier;

        public Dictionary<string, EntityLink> Links => this.links;
    }
}