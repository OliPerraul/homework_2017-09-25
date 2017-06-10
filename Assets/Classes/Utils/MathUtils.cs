using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{

    /// Rounds to nearest multiple of given factor
    public static float mRound(float value, float factor)
    {
        return Mathf.Round(value / factor) * factor;
    }


}
