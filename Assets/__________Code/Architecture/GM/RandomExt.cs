using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExt 
{
    public static float RandomRange(Vector2 range) =>
        Random.Range(range.x, range.y);
    public static int RandomRange(Vector2Int range) =>
        Random.Range(range.x, range.y);
}
