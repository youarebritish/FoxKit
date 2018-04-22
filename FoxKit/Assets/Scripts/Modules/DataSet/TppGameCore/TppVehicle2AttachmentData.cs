using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        public byte VehicleTypeCode;
        public byte AttachmentImplTypeIndex;
        public UnityEngine.Object AttachmentFile;
        public byte AttachmentInstanceCount;
        public string BodyCnpName;
        public string AttachmentBoneName;
    }
}