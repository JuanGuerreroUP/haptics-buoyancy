using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSphere : AbstractFloatingObject
{
    override public float CalcVolume(float waterLevel)
    {
        float r = this.transform.localScale.x;
        float h = this.GetH(waterLevel);
        return ((Mathf.PI * Mathf.Pow(h, 2))/3) * ((3*r)-h);
    }
}
