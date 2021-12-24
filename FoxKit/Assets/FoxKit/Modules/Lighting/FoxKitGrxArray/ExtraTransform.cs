using FoxKit.Utils;
using System.IO;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

namespace FoxKit.GrxArray.GrxArrayTool
{
    public class ExtraTransform
    {
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Translation { get; set; }

        public virtual void Read(BinaryReader reader)
        {
            Scale = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Rotation = FoxUtils.FoxToUnity(new FoxLib.Core.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Translation = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
        }

        public virtual void Write(BinaryWriter writer)
        {
            FoxLib.Core.Vector3 newScale = FoxUtils.UnityToFox(Scale);
            writer.Write(-newScale.X); writer.Write(newScale.Y); writer.Write(newScale.Z);
            FoxLib.Core.Quaternion newQuat = FoxUtils.UnityToFox(Rotation);
            writer.Write(newQuat.X); writer.Write(newQuat.Y); writer.Write(newQuat.Z); writer.Write(newQuat.W);
            FoxLib.Core.Vector3 newTranslation= FoxUtils.UnityToFox(Translation);
            writer.Write(newTranslation.X); writer.Write(newTranslation.Y); writer.Write(newTranslation.Z);
        }
        public virtual void Log()
        {
        }
    }
}
