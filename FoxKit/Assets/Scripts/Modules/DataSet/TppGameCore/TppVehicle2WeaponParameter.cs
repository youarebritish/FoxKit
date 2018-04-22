using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2WeaponParameter : DataElement<TppVehicle2AttachmentData>
    {
        public string AttackId;
        public string EquipId;
        public string BulletId;
        public byte WeaponImplTypeIndex;
        public float FireInterval;
        public UnityEngine.Object WeaponFile;
        public UnityEngine.Object AmmoFile;
        public string OwnerCnpName;
        public string WeaponBoneName;
        public string TurretBoneName;
        public float MinPitch;
        public float MaxPitch;
        public float RotSpeed;
    }
}