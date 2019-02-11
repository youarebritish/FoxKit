using FoxKit.Modules.Lighting.LightProbes;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.TppEffect
{
    public partial class TppLightProbeSHCoefficients
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(LightingDataAsset)).image as Texture2D;

        public LightProbeSHCoefficientsAsset LpshFile
        {
            get
            {
                return this.lpshFile as LightProbeSHCoefficientsAsset;
            }
            set
            {
                this.lpshFile = value;
            }
        }
    }
}
