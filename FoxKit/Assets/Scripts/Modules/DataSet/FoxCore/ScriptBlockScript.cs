using System;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class ScriptBlockScript : Data
    {
        public UnityEngine.Object Script;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "script")
            {
                var path = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var success = DataSetUtils.TryGetFile(path, out Script);
            }
        }
    }
}