using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static float MapRange(float num, float inputStart, float inputEnd, float outputStart, float outputEnd)
    {
        return ((num - inputStart) * ((outputEnd - outputStart) / (inputEnd - inputStart))) + outputStart;
    }
}
