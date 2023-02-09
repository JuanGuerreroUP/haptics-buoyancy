using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSphere : AbstractFloatingObject
{
    override public float GetObjVolume()
    {
        float r = this.transform.localScale.x;
        float h = 0;
        return ((Mathf.PI * Mathf.Pow(h, 2))/3) * ((3*r)-h);
    }
}
