using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtilities 
{
    public static Vector2 AngleToVector2(float degrees)
    {
        return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad));
    }
}
