using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        public byte VehicleTypeCode;
        public byte AttachmentImplTypeIndex;
        public string AttachmentFile;           // TODO: FilePtr
        public byte AttachmentInstanceCount;
        public string BodyCnpName;
        public string AttachmentBoneName;
    }
}