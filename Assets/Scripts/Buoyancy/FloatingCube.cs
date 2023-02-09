using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : AbstractFloatingObject
{

    public override float GetObjVolume()
    {
        float width;
        width = this.transform.localScale.x;
        return Mathf.Pow(width, 3) ; //g/m3
    }
    //olozada@up.edu.mx
}
