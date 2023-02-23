using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSphere : AbstractFloatingObject
{
    public override float GetDisplacedVolume()
    {
        throw new System.NotImplementedException();
    }

    override public float GetObjVolume()
    {
        float r = 1;// this.GetCollider().radius;
        float h = 0;
        return ((Mathf.PI * Mathf.Pow(h, 2))/3) * ((3*r)-h);
    }
}
