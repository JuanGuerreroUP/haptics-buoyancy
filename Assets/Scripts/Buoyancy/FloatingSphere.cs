using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSphere : AbstractFloatingObject
{
    public override float GetDisplacedVolume()
    {
        float d = this.transformHelper.Scale.x;
        float r = d / 2;
        float h = GetH();
        if (h > r) {
            return GetSphereVolume(r, r) + GetCylinderVolume(r, h - r);
        }
        else {
            return GetSphereVolume(r, h);
        }
    }

    private float GetSphereVolume(float r, float h)
    {
        return ((Mathf.PI * Mathf.Pow(h, 2)) / 3) * ((3 * r) - h);
    }

    private float GetCylinderVolume(float r, float h)
    {
        return Mathf.PI * Mathf.Pow(r, 2) * h;
    }

    override public float GetObjVolume()
    {
        float d = this.transformHelper.Scale.x;
        float r = d / 2;
        return GetSphereVolume(r, d);
    }
}
