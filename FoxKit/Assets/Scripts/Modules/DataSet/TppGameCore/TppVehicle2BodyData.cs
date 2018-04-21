using FoxKit.Modules.DataSet.FoxCore;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2BodyData : Data
    {
        public byte VehicleTypeIndex;
        public byte ProxyVehicleTypeIndex;
        public byte BodyImplTypeIndex;
        public string PartsFile;            // TODO FilePtr
        public byte BodyInstanceCount;
        public TppVehicle2WeaponParameter WeaponParams;
        public List<string> FovaFiles = new List<string>();      // TODO FilePtr
    }
}