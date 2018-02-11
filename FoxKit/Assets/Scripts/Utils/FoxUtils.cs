using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FoxUtils
{
    public static Vector3 FoxToUnity(FoxLib.Core.Vector3 foxVector)
    {
        return new Vector3(foxVector.Z, foxVector.Y, foxVector.X);
    }
}
