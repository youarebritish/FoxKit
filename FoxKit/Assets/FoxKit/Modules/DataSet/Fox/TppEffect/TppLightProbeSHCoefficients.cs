using FoxKit.Modules.Lighting.LightProbes;

namespace FoxKit.Modules.DataSet.Fox.TppEffect
{
    public partial class TppLightProbeSHCoefficients
    {
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
