using System;
using UnityEngine;

public class StageBlockController : MonoBehaviour
{
    public Color SmallBlockBounds;

    public int SmallBlockCount;
    public int SmallBlockSize;

    private void OnDrawGizmos()
    {
        DrawSmallBlocksBounds();
    }

    private void DrawSmallBlocksBounds()
    {
        Gizmos.color = SmallBlockBounds;
        for (int i = 0; i <= SmallBlockCount; i++)
        {
            float length = SmallBlockSize * SmallBlockCount / 2;

            var startPosition1 = new Vector3((-i * 128) + 4096, 0, length);
            var endPosition1 = new Vector3((-i * 128) + 4096, 0, -length);            

            Gizmos.DrawLine(startPosition1, endPosition1);

            var startPosition2 = new Vector3(startPosition1.z, 0, startPosition1.x);
            var endPosition2 = new Vector3(endPosition1.z, 0, endPosition1.x);

            Gizmos.DrawLine(startPosition2, endPosition2);
        }
    }
}
