using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.TppGameKit
{
    [Serializable]
    public class TppPermanentGimmickMortarParameter : TppPermanentGimmickParameter
    {
        public float RotationLimitLeftRight;
        public float RotationLimitLeftUp;
        public float RotationLimitLeftDown;
        public UnityEngine.Object DefaultShellPartsFile;
        public UnityEngine.Object FlareShellPartsFile;
    }
}