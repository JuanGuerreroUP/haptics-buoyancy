using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : AbstractFloatingObject
{

    public override float CalcVolume(float waterLevel)
    {
        float hSub, width;
        hSub = GetH(waterLevel);
        width = this.transform.localScale.x;
        return Mathf.Pow(width, 2) * hSub;
    }
    //olozada@up.edu.mx
}
