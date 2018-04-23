using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppOcean : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxBool Enable { get; set; }
        public FoxBool Wireframe { get; set; }
        public FoxFloat BaseHeight { get; set; }
        public FoxUInt32 GridNumX { get; set; }
        public FoxUInt32 GridNumY { get; set; }
        public FoxFloat ScreenMarginX { get; set; }
        public FoxFloat ScreenMarginY { get; set; }
        public FoxFloat WaveLengthMin { get; set; }
        public FoxFloat WaveLengthMax { get; set; }
        public FoxFloat WaveDispersion { get; set; }
        public FoxFloat WindSpeed { get; set; }
        public FoxPath WaveParamTexture { get; set; }
        public FoxPath WhitecapTexture { get; set; }
        public FoxFloat HorizonDistance { get; set; }
        public FoxFloat LightCaptureDistance { get; set; }
        public FoxUInt32 RandomSeed { get; set; }
        public FoxEntityLink CollisionDatas { get; set; }
    }
}
