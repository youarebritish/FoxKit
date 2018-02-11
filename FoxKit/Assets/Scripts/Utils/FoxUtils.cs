using UnityEngine;

public static class FoxUtils
{
    public static Vector3 FoxToUnity(FoxLib.Core.Vector3 foxVector)
    {
        return new Vector3(foxVector.Z, foxVector.Y, foxVector.X);
    }

    public static FoxLib.Core.Vector3 UnityToFox(Vector3 unityVector)
    {
        return new FoxLib.Core.Vector3(unityVector.z, unityVector.y, unityVector.x);
    }
}
